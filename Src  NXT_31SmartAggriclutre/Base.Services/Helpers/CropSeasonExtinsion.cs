using Base.DAL.Models.SystemModels;
using Base.Shared.DTOs;

namespace Base.Services.Helpers
{
    public static class CropSeasonExtinsion
    {
        public static CropSeasonDto ToCropSeasonDto(this CropSeason entity)
        {
            if (entity is null)
            {
                return new CropSeasonDto();
            }

            return new CropSeasonDto
            {
                Id = entity.Id,
                SeasonName = entity.SeasonName,
                PlantingStartMonth = entity.PlantingStartMonth,
                ExpectedRangeDays = entity.ExpectedRangeDays
            };
        }

        public static HashSet<CropSeasonDto> ToCropSeasonDtoSet(this IEnumerable<CropSeason> entities)
        {
            if (entities == null)
                return new HashSet<CropSeasonDto>();

            return entities.Select(e => e.ToCropSeasonDto()).ToHashSet();
        }

        public static CropSeason ToCropSeason(this CreateSeasonRequest req)
        {
            return new CropSeason
            {
                CropId = req.CropId,
                SeasonName = req.SeasonName,
                PlantingStartMonth = req.PlantingStartMonth,
                PlantingEndMonth = req.PlantingEndMonth,
                HarvestStartMonth = req.HarvestStartMonth,
                HarvestEndMonth = req.HarvestEndMonth,
                ExpectedRangeDays = req.ExpectedRangeDays,
                Notes = req.Notes   
            };
        }

        public static void ToCropSeason(this UpdateSeasonRequest req, CropSeason season)
        {
            season.SeasonName = req.SeasonName;
            season.PlantingStartMonth = req.PlantingStartMonth;
            season.PlantingEndMonth = req.PlantingEndMonth;
            season.HarvestStartMonth = req.HarvestStartMonth;
            season.HarvestEndMonth = req.HarvestEndMonth;
            season.ExpectedRangeDays = req.ExpectedRangeDays;
            season.Notes = req.Notes;
        }
    }
}
