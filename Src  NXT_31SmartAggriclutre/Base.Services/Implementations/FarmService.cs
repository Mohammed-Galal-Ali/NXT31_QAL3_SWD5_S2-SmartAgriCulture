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
    public class FarmService : IFarmService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserProfileService _userProfileService;

        public FarmService(IUnitOfWork unitOfWork, IUserProfileService userProfileService)
        {
            _unitOfWork = unitOfWork;
            _userProfileService = userProfileService;
        }

        public async Task<FarmListDto> GetAllAsync(string? currentUserId, string? search, int page, int pageSize)
        {
            var rebo = _unitOfWork.Repository<Farm>();
            var query = new BaseSpecification<Farm>();
            query.AllIncludes.Add(e => e.Include(f => f.Owner).Include(f => f.Zones).ThenInclude(z => z.ZoneCrops));

            // لو مش Admin، يشوف بس مزارعه هو
            if (!await IsAdminAsync(currentUserId))
                query = query.Where(f => f.UserId == currentUserId);

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(f => f.Name.Contains(search) || f.Code.Contains(search));

            var total = await rebo.CountAsync(query);

            query.AddOrderByDesc(f => f.DateOfCreattion);
            query.ApplyPaging((page - 1) * pageSize, pageSize);

            var list = await rebo.ListAsync(query, true);

            var dtos = list.ToFarmDtoSet();

            return new FarmListDto
            {
                Farms = dtos.ToList(),
                TotalCount = total
            };
        }

        public async Task<FarmDto?> GetByIdAsync(string id, string currentUserId)
        {
            var rebo = _unitOfWork.Repository<Farm>();
            var query = new BaseSpecification<Farm>(f => f.Id == id);
            query.AllIncludes.Add(e => e.Include(f => f.Owner).Include(f => f.Zones).ThenInclude(z => z.ZoneCrops));

            var farm = await rebo.GetEntityWithSpecAsync(query);

            if (farm == null) return null;

            // Authorization: Admin أو صاحب المزرعة
            if (!await IsAdminAsync(currentUserId) && farm.UserId != currentUserId)
                return null;

            return farm.ToFarmDto();
        }

        public async Task<FarmDto> CreateAsync(CreateFarmRequest request, string creatorId)
        {
            var isAdmin = await IsAdminAsync(creatorId);

            var farm = request.ToFarm();
            farm.UserId = isAdmin && string.IsNullOrWhiteSpace(request.OwnerId) ? request.OwnerId : creatorId;
            var rebo = _unitOfWork.Repository<Farm>();
            await rebo.AddAsync(farm);
            await _unitOfWork.CompleteAsync();

            return farm.ToFarmDto();
        }

        public async Task<FarmDto?> UpdateAsync(string id, UpdateFarmRequest request, string updaterId)
        {
            var rebo = _unitOfWork.Repository<Farm>();
            var farm = await rebo.GetByIdAsync(id);
            if (farm == null) return null;

            if (!await IsAdminAsync(updaterId) && farm.UserId != updaterId)
                throw new UnauthorizedAccessException("You are Not Authorized to update this Farm");

            request.ToFarm(farm);
            await rebo.UpdateAsync(farm);
            await _unitOfWork.CompleteAsync();

            return farm.ToFarmDto();
        }

        public async Task<bool> DeleteAsync(string id, string userId)
        {
            var rebo = _unitOfWork.Repository<Farm>();
            var farm = await rebo.GetByIdAsync(id);
            if (farm == null) return false;

            if (!await IsAdminAsync(userId) && farm.UserId != userId)
                return false;

            // Soft Delete أو Hard Delete حسب رغبتك
            await rebo.DeleteAsync(farm);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        private async Task<bool> IsAdminAsync(string? userId)
        {
            if (string.IsNullOrEmpty(userId)) return false;
            var user = await _userProfileService.GetByIdAsync(userId);
            return user?.UserType == UserTypes.SystemAdmin;
        }
    }
}
