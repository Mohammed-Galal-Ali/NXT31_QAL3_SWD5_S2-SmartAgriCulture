using Base.DAL.Models.BaseModels;
using Base.DAL.Models.SystemModels;
using Base.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Services.Helpers
{
    public static class FarmExtinsion
    {
        public static FarmDto ToFarmDto(this Farm farm)
        {
            if (farm is null)
            {
                return new FarmDto();
            }

            return new FarmDto
            {
                Id = farm.Id,
                Name = farm.Name,
                Code = farm.Code,
                Lat = farm.Lat,
                Lon = farm.Lon,
                Address = farm.Address,
                OwnerId = farm.UserId ?? "NA",
                OwnerName = farm.Owner?.FullName ?? "NA",
                ZonesCount = farm.Zones?.Count ?? 0,
                ActiveCropsCount = farm.Zones?.SelectMany(z => z.ZoneCrops).Count(zc => zc.IsActive) ?? 0,
                DateOfCreation = farm.DateOfCreattion,
                DateOfUpdate = farm.DateOfUpdate
            };
        }

        public static HashSet<FarmDto> ToFarmDtoSet(this IEnumerable<Farm> entities)
        {
            if (entities == null)
                return new HashSet<FarmDto>();

            return entities.Select(e => e.ToFarmDto()).ToHashSet();
        }

        public static Farm ToFarm(this CreateFarmRequest Dto)
        {
            if (Dto is null)
            {
                return new Farm();
            }

            return new Farm
            {
                Name = Dto.Name,
                Code = Dto.Code,
                Lat = Dto.Lat,
                Lon = Dto.Lon,
                Address = Dto.Address,
                UserId = Dto.OwnerId
            };
        }

        public static void ToFarm(this UpdateFarmRequest Dto, Farm farm)
        {
            farm.Name = Dto.Name ?? farm.Name;
            farm.Code = Dto.Code ?? farm.Code;
            farm.Lat = Dto.Lat ?? farm.Lat;
            farm.Lon = Dto.Lon ?? farm.Lon;
            farm.Address = Dto.Address ?? farm.Address;
        }
    }
}
