using Base.DAL.Models.SystemModels;
using Base.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Services.Helpers
{
    internal static class ZoneCropExtinsion
    {
        public static ZoneCrop ToZoneCrop(this CreateZoneCropRequest req)
        {
            return new ZoneCrop
            {
                Id = Guid.NewGuid().ToString(),
                ZoneId = req.ZoneId,
                CropId = req.CropId,
                CropGrowthStageId = req.CropGrowthStageId,
                PlantedAt = req.PlantingDate,
                IsActive = req.IsActive
            };
        }

        public static void ToZoneCrop(this UpdateZoneCropRequest req, ZoneCrop zc)
        {
            zc.CropGrowthStageId = req.CropGrowthStageId ?? zc.CropGrowthStageId;
            zc.IsActive = req.IsActive;
        }

        public static ZoneCropDto ToZoneCropDto(this ZoneCrop zc)
        {
            return new ZoneCropDto
            {
                Id = zc.Id,
                ZoneId = zc.ZoneId,
                CropId = zc.CropId,
                CropName = zc.Crop?.Name ?? "",
                CropGrowthStageId = zc.CropGrowthStageId,
                StageName = zc.CropGrowthStage?.StageName ?? "",
                PlantingDate = zc.PlantedAt,
                IsActive = zc.IsActive
            };
        }

        public static IEnumerable<ZoneCropDto> ToZoneCropDtoSet(this IEnumerable<ZoneCrop> crops)
        {
            return crops.Select(c => c.ToZoneCropDto());
        }
    }
}
