using Base.DAL.Models.SystemModels;
using Base.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Services.Helpers
{
    public static class ZoneExtinsion
    {
        public static ZoneDto ToZoneDto(this Zone zone)
        {
            if (zone is null)
            {
                return new ZoneDto();
            }

            return new ZoneDto
            {
                Id = zone.Id,
                Name = zone.Name,
                Area = zone.Area,
                SoilType = zone.SoilType,
                FarmId = zone.FarmId,
                FarmName = zone.Farm?.Name ?? "NA",
                EquipmentsCount = zone.Equipments?.Count ?? 0,
                ActiveCropsCount = zone.ZoneCrops?.Count(zc => zc.IsActive) ?? 0,
                DateOfCreation = zone.DateOfCreattion,
                DateOfUpdate = zone.DateOfUpdate
            };
        }

        public static HashSet<ZoneDto> ToZoneDtoSet(this IEnumerable<Zone> entities)
        {
            if (entities == null)
                return new HashSet<ZoneDto>();

            return entities.Select(e => e.ToZoneDto()).ToHashSet();
        }

        public static Zone ToZone(this CreateZoneRequest Dto)
        {
            if (Dto is null)
            {
                return new Zone();
            }

            return new Zone
            {
                Name = Dto.Name,
                Area = Dto.Area,
                SoilType = Dto.SoilType,
                FarmId = Dto.FarmId
            };
        }

        public static void ToZone(this UpdateZoneRequest Dto, Zone zone)
        {
            zone.Name = Dto.Name ?? zone.Name;
            zone.Area = Dto.Area ?? zone.Area;
            zone.SoilType = Dto.SoilType ?? zone.SoilType;
        }
    }
}
