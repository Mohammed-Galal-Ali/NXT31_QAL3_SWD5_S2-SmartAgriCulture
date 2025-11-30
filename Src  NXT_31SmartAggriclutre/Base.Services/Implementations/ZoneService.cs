using Base.DAL.Models.SystemModels;
using Base.Repo.Interfaces;
using Base.Services.Helpers;
using Base.Services.Interfaces;
using Base.Shared.DTOs;
using Microsoft.EntityFrameworkCore;
using RepositoryProject.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Services.Implementations
{
    public class ZoneService : IZoneService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserProfileService _userProfileService;

        public ZoneService(IUnitOfWork unitOfWork, IUserProfileService userProfileService)
        {
            _unitOfWork = unitOfWork;
            _userProfileService = userProfileService;
        }
        // جلب كل المناطق الخاصة بمزرعة معينة (الأكثر استخدامًا)
        public async Task<ZoneListDto> GetAllByFarmAsync(string farmId, string currentUserId, int page = 1, int pageSize = 20)
        {
            var rebo = _unitOfWork.Repository<Zone>();
            var query = new BaseSpecification<Zone>(z => z.FarmId == farmId);
            query.AllIncludes.Add(q => q.Include(z => z.Farm)
                .Include(z => z.Equipments)
                .Include(z => z.ZoneCrops));

            // Authorization
            await ValidateFarmAccessAsync(farmId, currentUserId);

            var total = await rebo.CountAsync(query);

            query.AddOrderByDesc(f => f.Name);
            query.ApplyPaging((page - 1) * pageSize, pageSize);

            var list = await rebo.ListAsync(query, true);
            var dtos = list.ToZoneDtoSet();

            return new ZoneListDto
            {
                Zones = dtos.ToList(),
                TotalCount = total
            };
        }

        public async Task<ZoneListDto> GetAllAsync(string currentUserId, string? search, int page = 1, int pageSize = 20)
        {
            var rebo = _unitOfWork.Repository<Zone>();
            var query = new BaseSpecification<Zone>();
            query.AllIncludes.Add(q => q.Include(z => z.Farm).ThenInclude(f => f!.Owner));


            // غير الـ Admin يشوف بس مزارعه
            if (!await IsAdminAsync(currentUserId))
            {
                query = query.Where(z => z.Farm!.UserId == currentUserId);
            }

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(z => z.Name.Contains(search) || z.SoilType.Contains(search));

            var total = await rebo.CountAsync(query);

            query.AddOrderByDesc(z => z.DateOfCreattion);
            query.ApplyPaging((page - 1) * pageSize, pageSize);

            var list = await rebo.ListAsync(query, true);
            var dtos = list.ToZoneDtoSet();

            return new ZoneListDto
            {
                Zones = dtos.ToList(),
                TotalCount = total
            };
        }

        public async Task<ZoneDto?> GetByIdAsync(string id, string currentUserId)
        {
            var rebo = _unitOfWork.Repository<Zone>();
            var query = new BaseSpecification<Zone>(z => z.Id == id);
            query.AllIncludes.Add(q => q.Include(z => z.Farm)
                .ThenInclude(f => f!.Owner)
                .Include(z => z.Equipments)
                .Include(z => z.ZoneCrops));

            var zone = await rebo.GetEntityWithSpecAsync(query,true);

            if (zone == null) return null;

            // Authorization
            if (!await IsAdminAsync(currentUserId) && zone.Farm!.UserId != currentUserId)
                return null;

            return zone.ToZoneDto();
        }

        public async Task<ZoneDto> CreateAsync(CreateZoneRequest request, string creatorId)
        {
            await ValidateFarmAccessAsync(request.FarmId, creatorId);
            var repo = _unitOfWork.Repository<Zone>();
            var zone = request.ToZone();
            zone.Id = Guid.NewGuid().ToString();

            await repo.AddAsync(zone);
            await _unitOfWork.CompleteAsync();

            return zone.ToZoneDto();
        }

        public async Task<ZoneDto?> UpdateAsync(string id, UpdateZoneRequest request, string updaterId)
        {
            var rebo = _unitOfWork.Repository<Zone>();
            var query = new BaseSpecification<Zone>(z => z.Id == id);
            query.AllIncludes.Add(q => q.Include(z => z.Farm));
            var zone = await rebo.GetEntityWithSpecAsync(query);

            if (zone == null) return null;

            if (!await IsAdminAsync(updaterId) && zone.Farm!.UserId != updaterId)
                throw new UnauthorizedAccessException("غير مسموح لك بتعديل هذه المنطقة");

            request.ToZone(zone);

            await rebo.UpdateAsync(zone);
            await _unitOfWork.CompleteAsync();
            return zone.ToZoneDto();
        }

        public async Task<bool> DeleteAsync(string id, string userId)
        {
            var rebo = _unitOfWork.Repository<Zone>();
            var query = new BaseSpecification<Zone>(z => z.Id == id);
            query.AllIncludes.Add(q => q.Include(z => z.Farm).Include(z=>z.ZoneCrops));
            var zone = await rebo.GetEntityWithSpecAsync(query);

            if (zone == null) return false;

            if (!await IsAdminAsync(userId) && zone.Farm!.UserId != userId)
                return false;

            // لو فيها أجهزة أو محاصيل نشطة → Soft Delete أو منع الحذف
            if (zone.Equipments.Any() || zone.ZoneCrops.Any(zc => zc.IsActive))
                throw new InvalidOperationException("لا يمكن حذف منطقة بها أجهزة أو محاصيل نشطة");

            await rebo.DeleteAsync(zone);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        // Helper Methods
        private async Task ValidateFarmAccessAsync(string farmId, string userId)
        {
            var rebo = _unitOfWork.Repository<Farm>();
            var query = new BaseSpecification<Farm>(f => f.Id == farmId && f.UserId == userId);
            var check1 = (await rebo.CountAsync(query)) > 0;
            var check2 = await IsAdminAsync(userId);
            if (!(check1 || check2))
                throw new UnauthorizedAccessException("ليس لديك صلاحية الوصول لهذه المزرعة");
        }

        private async Task<bool> IsAdminAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return false;
            var user = await _userProfileService.GetByIdAsync(userId);
            return user?.UserType == UserTypes.SystemAdmin;
        }
    }
}
