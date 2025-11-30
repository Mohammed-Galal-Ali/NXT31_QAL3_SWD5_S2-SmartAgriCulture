using Base.DAL.Models.SystemModels;
using Base.Services.Helpers;
using Base.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Services.Interfaces
{
    public interface ICropService
    {
        Task<CropListDto> GetAllAsync(string? search, int page = 1, int pageSize = 50);
        Task<CropDetailsDto?> GetByIdAsync(string id);
        Task<CropDto> CreateAsync(CreateCropRequest request, string userId);
        Task<CropDto?> UpdateAsync(string id, UpdateCropRequest request, string userId);
        Task<bool> DeleteAsync(string id);

        // Stages
        Task<CropStageDto> AddStageAsync(CreateStageRequest request, string userId);
        Task<CropStageDto?> UpdateStageAsync(string stageId, UpdateStageRequest request, string userId);
        Task<bool> DeleteStageAsync(string stageId);
        Task<CropStageDto?> GetStageByIdAsync(string id);

        // Requirements
        Task<StageRequirementDto> AddRequirementAsync(CreateRequirementRequest request, string userId);
        Task<bool> DeleteRequirementAsync(string requirementId);

        // === Seasons ===
        Task<CropSeasonDto> AddSeasonAsync(CreateSeasonRequest request, string userId);

        Task<CropSeasonDto?> UpdateSeasonAsync(string seasonId, UpdateSeasonRequest request, string userId);

        Task<bool> DeleteSeasonAsync(string seasonId);
    }
}
