using Base.DAL.Models.SystemModels;
using Base.Shared.DTOs;

namespace Base.Services.Helpers
{
    public static class CropGrowthStageExtinsion
    {
        public static CropStageDto ToCropStageDto(this CropGrowthStage entity)
        {
            if (entity is null)
            {
                return new CropStageDto();
            }

            return new CropStageDto
            {
                Id = entity.Id,
                StageName = entity.StageName,
                Order = entity.Order,
                DurationDays = entity.DurationDays,
                Description = entity.Description,
                Requirements = entity.CropStageRequirments.ToStageRequirementDtoSet().ToList(),
            };
        }

        public static HashSet<CropStageDto> ToCropStageDtoSet(this IEnumerable<CropGrowthStage> entities)
        {
            if (entities == null)
                return new HashSet<CropStageDto>();

            return entities.Select(e => e.ToCropStageDto()).ToHashSet();
        }

        public static CropGrowthStage ToCropGrowthStage(this CreateStageRequest Dto)
        {
            if (Dto is null)
            {
                return new CropGrowthStage();
            }

            return new CropGrowthStage
            {
                StageName = Dto.StageName,
                Order = Dto.Order,
                DurationDays = Dto.DurationDays,
                Description = Dto.Description,
                CropId = Dto.CropId
            };
        }

        public static void ToCropGrowthStage(this UpdateStageRequest Dto, CropGrowthStage entity)
        {
            entity.StageName = Dto.StageName ?? entity.StageName;
            entity.Order = Dto.Order ?? entity.Order;
            entity.DurationDays = Dto.DurationDays ?? entity.DurationDays;
            entity.Description = Dto.Description ?? entity.Description;
        }
    }
}
