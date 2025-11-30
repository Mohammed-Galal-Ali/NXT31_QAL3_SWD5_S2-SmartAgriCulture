using Base.DAL.Models.SystemModels;
using Base.Repo.Interfaces;
using Base.Services.Helpers;
using Base.Services.Interfaces;
using Base.Shared.DTOs;
using Base.Shared.Responses.Exceptions;
using Microsoft.EntityFrameworkCore;
using RepositoryProject.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Services.Implementations
{
    public class EquipmentService : IEquipmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserProfileService _userProfileService;

        public EquipmentService(IUnitOfWork unitOfWork, IUserProfileService userProfileService)
        {
            _unitOfWork = unitOfWork;
            _userProfileService = userProfileService;
        }

        public async Task<EquipmentListDto> GetAllByZoneAsync(string zoneId, string currentUserId, int page = 1, int pageSize = 20)
        {
            var repo = _unitOfWork.Repository<Equipment>();
            var query = new BaseSpecification<Equipment>(e => e.ZoneId == zoneId);
            query.AllIncludes.Add(q => q.Include(e => e.ReadingType).Include(e => e.Zone));

            await ValidateZoneAccessAsync(zoneId, currentUserId);

            var total = await repo.CountAsync(query);

            query.AddOrderByDesc(e => e.InstalledAt);
            query.ApplyPaging((page - 1) * pageSize, pageSize);

            var list = await repo.ListAsync(query, true);
            var dtos = list.ToEquipmentDtoSet(); // Extension in Helpers

            return new EquipmentListDto
            {
                Equipments = dtos.ToList(),
                TotalCount = total
            };
        }

        public async Task<EquipmentDto?> GetByIdAsync(string id, string currentUserId)
        {
            var repo = _unitOfWork.Repository<Equipment>();
            var query = new BaseSpecification<Equipment>(e => e.Id == id);
            query.AllIncludes.Add(q => q.Include(e => e.ReadingType).Include(e => e.Zone).ThenInclude(z => z.Farm));

            var equipment = await repo.GetEntityWithSpecAsync(query);

            if (equipment == null) return null;

            if (!await IsAdminAsync(currentUserId) && equipment.Zone?.Farm?.UserId != currentUserId)
                return null;

            return equipment.ToEquipmentDto();
        }

        public async Task<EquipmentDto> CreateAsync(CreateEquipmentRequest request, string creatorId)
        {
            await ValidateZoneAccessAsync(request.ZoneId, creatorId);

            var repo = _unitOfWork.Repository<Equipment>();
            var equipment = request.ToEquipment();
            equipment.Id = Guid.NewGuid().ToString(); // أو استخدم Identity لو موجود

            await repo.AddAsync(equipment);
            await _unitOfWork.CompleteAsync();

            return equipment.ToEquipmentDto();
        }

        public async Task<EquipmentDto?> UpdateAsync(string id, UpdateEquipmentRequest request, string updaterId)
        {
            var repo = _unitOfWork.Repository<Equipment>();
            var equipment = await repo.GetByIdAsync(id);
            if (equipment == null) return null;

            await ValidateZoneAccessAsync(equipment.ZoneId, updaterId);

            request.ToEquipment(equipment);
            await repo.UpdateAsync(equipment);
            await _unitOfWork.CompleteAsync();

            return equipment.ToEquipmentDto();
        }

        public async Task<bool> DeleteAsync(string id, string userId)
        {
            var repo = _unitOfWork.Repository<Equipment>();
            var equipment = await repo.GetByIdAsync(id);
            if (equipment == null) return false;

            await ValidateZoneAccessAsync(equipment.ZoneId, userId);

            // تحقق لو في قراءات مرتبطة (optional: soft delete إذا لزم)
            await repo.DeleteAsync(equipment);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        private async Task ValidateZoneAccessAsync(string zoneId, string userId)
        {
            var zoneRepo = _unitOfWork.Repository<Zone>();
            var query = new BaseSpecification<Zone>(z => z.Id == zoneId);
            query.AllIncludes.Add(q => q.Include(z => z.Farm));
            var zone = await zoneRepo.GetEntityWithSpecAsync(query);

            if (zone == null) throw new NotFoundException("Zone not found");

            if (!await IsAdminAsync(userId) && zone.Farm?.UserId != userId)
                throw new UnauthorizedAccessException("Not authorized to access this zone");
        }

        private async Task<bool> IsAdminAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return false;
            var user = await _userProfileService.GetByIdAsync(userId);
            return user?.UserType == UserTypes.SystemAdmin;
        }
    }
}