using Base.DAL.Models.SystemModels;
using Base.Repo.Interfaces;
using Base.Services.Helpers;
using Base.Services.Interfaces;
using Base.Shared.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using RepositoryProject.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Services.Implementations
{
    public class CropService : ICropService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserProfileService _userProfileService;

        public CropService(IUnitOfWork unitOfWork, IUserProfileService userProfileService)
        {
            _unitOfWork = unitOfWork;
            _userProfileService = userProfileService;
        }

        public async Task<CropListDto> GetAllAsync(string? search, int page, int pageSize)
        {
            var rebo = _unitOfWork.Repository<Crop>();
            var query = new BaseSpecification<Crop>();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(c => c.Name.Contains(search));

            var total = await rebo.CountAsync(query);

            query.AddOrderByDesc(f => f.Name);
            query.ApplyPaging((page - 1) * pageSize, pageSize);

            var list = await rebo.ListAsync(query, true);
            var dtos = list.ToCropDtoSet();

            return new CropListDto
            {
                Crops = dtos.ToList(),
                TotalCount = total
            };
        }

        public async Task<CropDetailsDto?> GetByIdAsync(string id)
        {
            var rebo = _unitOfWork.Repository<Crop>();
            var query = new BaseSpecification<Crop>(c => c.Id == id);
            query.AllIncludes.Add(c => c.Include(c => c.CropGrowthStages)
                    .ThenInclude(s => s.CropStageRequirments)
                        .ThenInclude(r => r.ReadingType)
                .Include(c => c.CropSeasons));
            var crop = await rebo.GetEntityWithSpecAsync(query, true);

            return crop == null ? null : crop.ToCropDetailsDto();
        }

        public async Task<CropDto> CreateAsync(CreateCropRequest request, string userId)
        {
            var rebo = _unitOfWork.Repository<Crop>();
            var crop = request.ToCrop();
            crop.Id = Guid.NewGuid().ToString();

            await rebo.AddAsync(crop);
            await _unitOfWork.CompleteAsync();

            return crop.ToCropDto();
        }

        public async Task<CropDto?> UpdateAsync(string id, UpdateCropRequest request, string userId)
        {
            var rebo = _unitOfWork.Repository<Crop>();
            var crop = await rebo.GetByIdAsync(id);
            if (crop == null) return null;

            if (!string.IsNullOrEmpty(request.Name))
                crop.Name = request.Name;

            await rebo.UpdateAsync(crop);
            await _unitOfWork.CompleteAsync();
            return crop.ToCropDto();
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var rebo = _unitOfWork.Repository<Crop>();
            var query = new BaseSpecification<Crop>(c => c.Id == id);
            query.AllIncludes.Add(c => c.Include(c => c.CropGrowthStages));
            var crop = await rebo.GetEntityWithSpecAsync(query);

            if (crop == null) return false;
            if (crop.CropGrowthStages.Any()) return false; // ممنوع لو في مراحل

            await rebo.DeleteAsync(crop);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        // === Stages ===
        public async Task<CropStageDto> AddStageAsync(CreateStageRequest request, string userId)
        {
            if (string.IsNullOrWhiteSpace(request.CropId)) return null!;
            var rebo = _unitOfWork.Repository<CropGrowthStage>();
            var stage = request.ToCropGrowthStage();
            stage.Id = Guid.NewGuid().ToString();

            await rebo.AddAsync(stage);
            await _unitOfWork.CompleteAsync();

            return stage.ToCropStageDto();
        }

        public async Task<CropStageDto?> UpdateStageAsync(string stageId, UpdateStageRequest request, string userId)
        {
            var rebo = _unitOfWork.Repository<CropGrowthStage>();
            var stage = await rebo.GetByIdAsync(stageId);
            if (stage == null) return null;

            request.ToCropGrowthStage(stage);

            await rebo.UpdateAsync(stage);
            await _unitOfWork.CompleteAsync();
            return stage.ToCropStageDto();
        }

        public async Task<bool> DeleteStageAsync(string stageId)
        {
            var rebo = _unitOfWork.Repository<CropGrowthStage>();
            var query = new BaseSpecification<CropGrowthStage>(s => s.Id == stageId);
            query.AllIncludes.Add(s => s.Include(s => s.CropStageRequirments));
            var stage = await rebo.GetEntityWithSpecAsync(query);

            if (stage == null) return false;
            if (stage.CropStageRequirments.Any()) return false;

            await rebo.DeleteAsync(stage);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<CropStageDto?> GetStageByIdAsync(string id)
        {
            var rebo = _unitOfWork.Repository<CropGrowthStage>();
            var query = new BaseSpecification<CropGrowthStage>(c => c.Id == id);
            var stage = await rebo.GetEntityWithSpecAsync(query, true);

            return stage == null ? null : stage.ToCropStageDto();
        }
        // === Requirements ===
        public async Task<StageRequirementDto> AddRequirementAsync(CreateRequirementRequest request, string userId)
        {
            if (string.IsNullOrWhiteSpace(request.CropGrowthStageId)) return null!;
            var rebo = _unitOfWork.Repository<CropStageRequirment>();
            var req = request.ToCropStageRequirment();
            req.Id = Guid.NewGuid().ToString();

            await rebo.AddAsync(req);
            await _unitOfWork.CompleteAsync();

            var dto = req.ToStageRequirementDto();
            var ReadingTypesrepo = _unitOfWork.Repository<ReadingType>();
            var readingType = await ReadingTypesrepo.GetByIdAsync(request.ReadingTypeId, true);
            if (readingType != null)
            {
                dto.ReadingTypeCode = readingType.Code;
                dto.ReadingTypeName = readingType.DisplayName;
                dto.Unit = readingType.Unit;
            }
            return dto;
        }

        public async Task<bool> DeleteRequirementAsync(string requirementId)
        {
            var rebo = _unitOfWork.Repository<CropStageRequirment>();
            var req = await rebo.GetByIdAsync(requirementId);
            if (req == null) return false;

            await rebo.DeleteAsync(req);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        // === Seasons ===
        public async Task<CropSeasonDto> AddSeasonAsync(CreateSeasonRequest request, string userId)
        {
            if (string.IsNullOrWhiteSpace(request.CropId)) return null!;
            var rebo = _unitOfWork.Repository<CropSeason>();
            var season = request.ToCropSeason();
            season.Id = Guid.NewGuid().ToString();

            await rebo.AddAsync(season);
            await _unitOfWork.CompleteAsync();
            return season.ToCropSeasonDto();
        }

        public async Task<CropSeasonDto?> UpdateSeasonAsync(string seasonId, UpdateSeasonRequest request, string userId)
        {
            var rebo = _unitOfWork.Repository<CropSeason>();
            var season = await rebo.GetByIdAsync(seasonId);
            if (season == null) return null;

            request.ToCropSeason(season);

            await rebo.UpdateAsync(season);
            await _unitOfWork.CompleteAsync();
            return season.ToCropSeasonDto();
        }

        public async Task<bool> DeleteSeasonAsync(string seasonId)
        {
            var rebo = _unitOfWork.Repository<CropSeason>();
            var season = await rebo.GetByIdAsync(seasonId);
            if (season == null) return false;

            await rebo.DeleteAsync(season);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
