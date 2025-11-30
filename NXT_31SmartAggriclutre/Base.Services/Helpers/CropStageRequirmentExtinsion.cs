using Base.DAL.Models.SystemModels;
using Base.Shared.DTOs;

namespace Base.Services.Helpers
{
    public static class CropStageRequirmentExtinsion
    {
        public static StageRequirementDto ToStageRequirementDto(this CropStageRequirment entity)
        {
            if (entity is null)
            {
                return new StageRequirementDto();
            }

            return new StageRequirementDto
            {
                Id = entity.Id,
                ReadingTypeCode = entity.ReadingType?.Code ?? "NA",
                ReadingTypeName = entity.ReadingType?.DisplayName ?? "NA",
                Unit = entity.ReadingType?.Unit ?? "NA",
                MinValue = entity.MinValue,
                MaxValue = entity.MaxValue,
                OptimalMin = entity.OptimalMin,
                OptimalMax = entity.OptimalMax,
            };
        }

        public static HashSet<StageRequirementDto> ToStageRequirementDtoSet(this IEnumerable<CropStageRequirment> entities)
        {
            if (entities == null)
                return new HashSet<StageRequirementDto>();

            return entities.Select(e => e.ToStageRequirementDto()).ToHashSet();
        }

        public static CropStageRequirment ToCropStageRequirment(this CreateRequirementRequest Dto)
        {
            if (Dto is null)
            {
                return new CropStageRequirment();
            }

            return new CropStageRequirment
            {
                ReadingTypeId = Dto.ReadingTypeId,
                MinValue = Dto.MinValue,
                MaxValue = Dto.MaxValue,
                OptimalMin = Dto.OptimalMin,
                OptimalMax = Dto.OptimalMax,
                CropGrowthStageId = Dto.CropGrowthStageId
            };
        }
    }
}
