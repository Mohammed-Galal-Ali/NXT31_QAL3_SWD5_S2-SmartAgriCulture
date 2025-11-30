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
    public class SensorReadingService : ISensorReadingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAlertService _alertService; // لإنشاء Alerts تلقائيًا
        private readonly IUserProfileService _userProfileService;

        public SensorReadingService(IUnitOfWork unitOfWork, IAlertService alertService, IUserProfileService userProfileService)
        {
            _unitOfWork = unitOfWork;
            _alertService = alertService;
            _userProfileService = userProfileService;
        }

        public async Task<SensorReadingListDto> GetAllByEquipmentAsync(string equipmentId, string currentUserId, DateTime? fromDate = null, DateTime? toDate = null, int page = 1, int pageSize = 50)
        {
            var repo = _unitOfWork.Repository<SensorReading>();
            var query = new BaseSpecification<SensorReading>(sr => sr.EquipmentId == equipmentId);
            query.AllIncludes.Add(q => q.Include(sr => sr.Equipment).ThenInclude(e => e.ReadingType).Include(sr => sr.Equipment).ThenInclude(e => e.Zone));

            await ValidateEquipmentAccessAsync(equipmentId, currentUserId);

            if (fromDate.HasValue) query.Where(sr => sr.DateOfCreattion >= fromDate.Value);
            if (toDate.HasValue) query.Where(sr => sr.DateOfCreattion <= toDate.Value);

            var total = await repo.CountAsync(query);

            query.AddOrderByDesc(sr => sr.DateOfCreattion);
            query.ApplyPaging((page - 1) * pageSize, pageSize);

            var list = await repo.ListAsync(query, true);
            var dtos = list.ToSensorReadingDtoSet();

            return new SensorReadingListDto
            {
                Readings = dtos.ToList(),
                TotalCount = total
            };
        }

        public async Task<SensorReadingDto?> GetByIdAsync(string id, string currentUserId)
        {
            var repo = _unitOfWork.Repository<SensorReading>();
            var query = new BaseSpecification<SensorReading>(sr => sr.Id == id);
            query.AllIncludes.Add(q => q.Include(sr => sr.Equipment).ThenInclude(e => e.ReadingType).Include(sr => sr.Equipment).ThenInclude(e => e.Zone).ThenInclude(z => z.Farm));

            var reading = await repo.GetEntityWithSpecAsync(query);

            if (reading == null) return null;

            if (!await IsAdminAsync(currentUserId) && reading.Equipment?.Zone?.Farm?.UserId != currentUserId)
                return null;

            return reading.ToSensorReadingDto();
        }

        public async Task<SensorReadingDto> CreateAsync(CreateSensorReadingRequest request, string creatorId)
        {
            var equipmentRepo = _unitOfWork.Repository<Equipment>();
            var equipmentQuery = new BaseSpecification<Equipment>(e => e.Id == request.EquipmentId);
            equipmentQuery.AllIncludes.Add(q => q.Include(e => e.Zone).ThenInclude(z => z.Farm).Include(e => e.ReadingType));
            var equipment = await equipmentRepo.GetEntityWithSpecAsync(equipmentQuery);

            if (equipment == null) throw new NotFoundException("Equipment not found");

            await ValidateEquipmentAccessAsync(request.EquipmentId, creatorId);

            var reading = request.ToSensorReading();

            var readingRepo = _unitOfWork.Repository<SensorReading>();
            await readingRepo.AddAsync(reading);
            await _unitOfWork.CompleteAsync();

            // التحقق من النطاق وإنشاء Alert إذا خارج
            await CheckAndCreateAlertAsync(reading, equipment);

            return reading.ToSensorReadingDto();
        }

        private async Task CheckAndCreateAlertAsync(SensorReading reading, Equipment equipment)
        {
            // جيب الـ ZoneCrop النشط للـ Zone
            var zoneCropRepo = _unitOfWork.Repository<ZoneCrop>();
            var zoneCropQuery = new BaseSpecification<ZoneCrop>(zc => zc.ZoneId == equipment.ZoneId && zc.IsActive);
            zoneCropQuery.AllIncludes.Add(q => q.Include(zc => zc.CropGrowthStage).ThenInclude(s => s.CropStageRequirments.Where(r => r.ReadingTypeId == equipment.ReadingTypeId)));
            var zoneCrop = await zoneCropRepo.GetEntityWithSpecAsync(zoneCropQuery);

            if (zoneCrop == null || zoneCrop.CropGrowthStage == null) return; // لا يوجد محصول نشط، لا تحقق

            var requirement = zoneCrop.CropGrowthStage.CropStageRequirments.FirstOrDefault();
            if (requirement == null) return; // لا متطلبات لهذا الـ ReadingType

            bool isOutOfRange = false;
            string alertType = "";
            string message = "";

            if (reading.Value > requirement.MaxValue)
            {
                isOutOfRange = true;
                alertType = "High";
                message = $"Value {reading.Value} exceeds max {requirement.MaxValue} for {equipment.ReadingType?.DisplayName}";
            }
            else if (reading.Value < requirement.MinValue)
            {
                isOutOfRange = true;
                alertType = "Low";
                message = $"Value {reading.Value} below min {requirement.MinValue} for {equipment.ReadingType?.DisplayName}";
            }

            if (isOutOfRange)
            {
                var alertRequest = new CreateAlertRequest
                {
                    ZoneId = equipment.ZoneId,
                    EquipmentId = equipment.Id,
                    CropId = zoneCrop.CropId,
                    CropGrowthStageId = zoneCrop.CropGrowthStageId,
                    ReadingTypeId = equipment.ReadingTypeId,
                    SensorReadingId = reading.Id,
                    AlertType = alertType, // High/Low
                    Message = message,
                };
                await _alertService.CreateAsync(alertRequest, equipment.Zone?.Farm?.UserId ?? "system");
            }
        }

        private async Task ValidateEquipmentAccessAsync(string equipmentId, string userId)
        {
            var equipmentRepo = _unitOfWork.Repository<Equipment>();
            var query = new BaseSpecification<Equipment>(e => e.Id == equipmentId);
            query.AllIncludes.Add(q => q.Include(e => e.Zone).ThenInclude(z => z.Farm));
            var equipment = await equipmentRepo.GetEntityWithSpecAsync(query);

            if (equipment == null) throw new NotFoundException("Equipment not found");

            if (!await IsAdminAsync(userId) && equipment.Zone?.Farm?.UserId != userId)
                throw new UnauthorizedAccessException("Not authorized to access this equipment");
        }

        private async Task<bool> IsAdminAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return false;
            var user = await _userProfileService.GetByIdAsync(userId);
            return user?.UserType == UserTypes.SystemAdmin;
        }
    }
}
