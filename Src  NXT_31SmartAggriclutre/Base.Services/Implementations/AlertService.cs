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
    public class AlertService : IAlertService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserProfileService _userProfileService;

        public AlertService(IUnitOfWork unitOfWork, IUserProfileService userProfileService)
        {
            _unitOfWork = unitOfWork;
            _userProfileService = userProfileService;
        }

        public async Task<AlertListDto> GetAllByZoneAsync(string zoneId, string currentUserId, DateTime? fromDate = null, DateTime? toDate = null, int page = 1, int pageSize = 50)
        {
            var repo = _unitOfWork.Repository<Alert>();
            var query = new BaseSpecification<Alert>(a => a.ZoneId == zoneId);
            query.AllIncludes.Add(q => q.Include(a => a.Equipment).Include(a => a.ReadingType).Include(a => a.Crop).Include(a => a.CropGrowthStage).Include(a => a.Zone));

            await ValidateZoneAccessAsync(zoneId, currentUserId);

            if (fromDate.HasValue) query.Where(a => a.DateOfCreattion >= fromDate.Value);
            if (toDate.HasValue) query.Where(a => a.DateOfCreattion <= toDate.Value);

            var total = await repo.CountAsync(query);

            query.AddOrderByDesc(a => a.DateOfCreattion);
            query.ApplyPaging((page - 1) * pageSize, pageSize);

            var list = await repo.ListAsync(query, true);
            var dtos = list.ToAlertDtoSet();

            return new AlertListDto
            {
                Alerts = dtos.ToList(),
                TotalCount = total
            };
        }

        public async Task<AlertDto?> GetByIdAsync(string id, string currentUserId)
        {
            var repo = _unitOfWork.Repository<Alert>();
            var query = new BaseSpecification<Alert>(a => a.Id == id);
            query.AllIncludes.Add(q => q.Include(a => a.Equipment).Include(a => a.ReadingType).Include(a => a.Crop).Include(a => a.CropGrowthStage).Include(a => a.Zone).ThenInclude(z => z.Farm));

            var alert = await repo.GetEntityWithSpecAsync(query);

            if (alert == null) return null;

            if (!await IsAdminAsync(currentUserId) && alert.Zone?.Farm?.UserId != currentUserId)
                return null;

            return alert.ToAlertDto();
        }

        public async Task<AlertDto> CreateAsync(CreateAlertRequest request, string creatorId)
        {
            await ValidateZoneAccessAsync(request.ZoneId, creatorId);

            var alert = request.ToAlert();
            //alert.Id = Guid.NewGuid().ToString();
            alert.IsAcknowledged = false;
            var repo = _unitOfWork.Repository<Alert>();
            await repo.AddAsync(alert);
            await _unitOfWork.CompleteAsync();

            // Optional: أرسل إشعار (email/push) للمزارع هنا عبر IEmailService

            return alert.ToAlertDto();
        }

        public async Task<bool> MarkAsResolvedAsync(string id, string userId)
        {
            var repo = _unitOfWork.Repository<Alert>();
            var alert = await repo.GetByIdAsync(id);
            if (alert == null) return false;

            await ValidateZoneAccessAsync(alert.ZoneId, userId);

            alert.IsAcknowledged = true;
            alert.AcknowledgedAt = DateTime.UtcNow;

            await repo.UpdateAsync(alert);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        private async Task ValidateZoneAccessAsync(string zoneId, string userId)
        {
            // نفس الكود من ZoneService أو EquipmentService
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
