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
    public class ZoneCropService : IZoneCropService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserProfileService _userProfileService;

        public ZoneCropService(IUnitOfWork unitOfWork, IUserProfileService userProfileService)
        {
            _unitOfWork = unitOfWork;
            _userProfileService = userProfileService;
        }

        public async Task<ZoneCropListDto> GetAllByZoneAsync(string zoneId, string currentUserId, int page = 1, int pageSize = 20)
        {
            var repo = _unitOfWork.Repository<ZoneCrop>();
            var query = new BaseSpecification<ZoneCrop>(zc => zc.ZoneId == zoneId);
            query.AllIncludes.Add(q => q.Include(zc => zc.Crop).Include(zc => zc.CropGrowthStage).Include(zc => zc.Zone));

            await ValidateZoneAccessAsync(zoneId, currentUserId);

            var total = await repo.CountAsync(query);

            query.AddOrderByDesc(zc => zc.PlantedAt);
            query.ApplyPaging((page - 1) * pageSize, pageSize);

            var list = await repo.ListAsync(query, true);
            var dtos = list.ToZoneCropDtoSet(); // Assume extension method in Helpers

            return new ZoneCropListDto
            {
                ZoneCrops = dtos.ToList(),
                TotalCount = total
            };
        }

        public async Task<ZoneCropDto?> GetByIdAsync(string id, string currentUserId)
        {
            var repo = _unitOfWork.Repository<ZoneCrop>();
            var query = new BaseSpecification<ZoneCrop>(zc => zc.Id == id);
            query.AllIncludes.Add(q => q.Include(zc => zc.Crop).Include(zc => zc.CropGrowthStage).Include(zc => zc.Zone).ThenInclude(z => z.Farm));

            var zoneCrop = await repo.GetEntityWithSpecAsync(query);

            if (zoneCrop == null) return null;

            if (!await IsAdminAsync(currentUserId) && zoneCrop.Zone?.Farm?.UserId != currentUserId)
                return null;

            return zoneCrop.ToZoneCropDto();
        }

        public async Task<ZoneCropDto> CreateAsync(CreateZoneCropRequest request, string creatorId)
        {
            await ValidateZoneAccessAsync(request.ZoneId, creatorId);

            var repo = _unitOfWork.Repository<ZoneCrop>();
            var zoneCrop = request.ToZoneCrop();
            await repo.AddAsync(zoneCrop);
            await _unitOfWork.CompleteAsync();

            return zoneCrop.ToZoneCropDto();
        }

        public async Task<ZoneCropDto?> UpdateAsync(string id, UpdateZoneCropRequest request, string updaterId)
        {
            var repo = _unitOfWork.Repository<ZoneCrop>();
            var zoneCrop = await repo.GetByIdAsync(id);
            if (zoneCrop == null) return null;

            await ValidateZoneAccessAsync(zoneCrop.ZoneId, updaterId);

            request.ToZoneCrop(zoneCrop);
            await repo.UpdateAsync(zoneCrop);
            await _unitOfWork.CompleteAsync();

            return zoneCrop.ToZoneCropDto();
        }

        public async Task<bool> DeleteAsync(string id, string userId)
        {
            var repo = _unitOfWork.Repository<ZoneCrop>();
            var zoneCrop = await repo.GetByIdAsync(id);
            if (zoneCrop == null) return false;

            await ValidateZoneAccessAsync(zoneCrop.ZoneId, userId);

            await repo.DeleteAsync(zoneCrop);
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
