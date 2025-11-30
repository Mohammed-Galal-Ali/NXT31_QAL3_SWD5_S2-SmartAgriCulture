using Base.DAL.Enums;
using Base.DAL.Models.SystemModels;
using Base.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Services.Helpers
{
    public static class EquipmentExtinsion
    {
        public static Equipment ToEquipment(this CreateEquipmentRequest req)
        {
            return new Equipment
            {
                ZoneId = req.ZoneId,
                ReadingTypeId = req.ReadingTypeId,
                EquipmentModel = req.EquipmentModel,
                SerialNumber = req.SerialNumber,
                InstalledAt = req.InstallationDate,
                Status = req.IsActive ? EquipmentsStatus.Active : EquipmentsStatus.Inactive 
            };
        }

        public static void ToEquipment(this UpdateEquipmentRequest req, Equipment eq)
        {
            eq.ReadingTypeId = req.ReadingTypeId ?? eq.ReadingTypeId;
            eq.Status = req.IsActive ? EquipmentsStatus.Active : EquipmentsStatus.Inactive;
        }

        public static EquipmentDto ToEquipmentDto(this Equipment eq)
        {
            return new EquipmentDto
            {
                Id = eq.Id,
                ZoneId = eq.ZoneId,
                ReadingTypeId = eq.ReadingTypeId,
                ReadingTypeName = eq.ReadingType?.DisplayName ?? "",
                EquipmentModel = eq.EquipmentModel,
                SerialNumber = eq.SerialNumber,
                InstallationDate = eq.InstalledAt,
                IsActive = eq.Status == EquipmentsStatus.Active 
            };
        }

        public static IEnumerable<EquipmentDto> ToEquipmentDtoSet(this IEnumerable<Equipment> equipments)
        {
            return equipments.Select(e => e.ToEquipmentDto());
        }
    }
}
