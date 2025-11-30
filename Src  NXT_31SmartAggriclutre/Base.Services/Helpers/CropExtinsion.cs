using Base.DAL.Models.SystemModels;
using Base.Shared.DTOs;
using System.Security.Cryptography.X509Certificates;

namespace Base.Services.Helpers
{
    public static class CropExtinsion
    {
        public static CropDto ToCropDto(this Crop entity)
        {
            if (entity is null)
            {
                return new CropDto();
            }

            return new CropDto
            {
                Id = entity.Id,
                Name = entity.Name,
                StagesCount = entity.CropGrowthStages?.Count ?? 0,
                SeasonsCount = entity.CropSeasons?.Count ?? 0,
                DateOfCreation = entity.DateOfCreattion,
            };
        }

        public static HashSet<CropDto> ToCropDtoSet(this IEnumerable<Crop> entities)
        {
            if (entities == null)
                return new HashSet<CropDto>();

            return entities.Select(e => e.ToCropDto()).ToHashSet();
        }

        public static Crop ToCrop(this CreateCropRequest Dto)
        {
            if (Dto is null)
            {
                return new Crop() { Name = string.Empty };
            }

            return new Crop
            {
                Name = Dto.Name
            };
        }

        public static void ToCrop(this UpdateCropRequest Dto, Crop entity)
        {
            entity.Name = Dto.Name ?? entity.Name;
        }

        public static CropDetailsDto ToCropDetailsDto(this Crop entity)
        {
            if (entity is null)
            {
                return new CropDetailsDto();
            }

            return new CropDetailsDto
            {
                Id = entity.Id,
                Name = entity.Name,
                GrowthStages = entity.CropGrowthStages.ToCropStageDtoSet().ToList(),
                Seasons = entity.CropSeasons.ToCropSeasonDtoSet().ToList(),
                DateOfCreation = entity.DateOfCreattion,
                DateOfUpdate = entity.DateOfUpdate,
            };
        }
    }
}
