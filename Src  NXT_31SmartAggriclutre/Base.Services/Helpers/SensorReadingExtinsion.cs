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
    public static class SensorReadingExtinsion
    {
        // SensorReadings
        public static SensorReading ToSensorReading(this CreateSensorReadingRequest req)
        {
            return new SensorReading
            {
                EquipmentId = req.EquipmentId,
                ReadingTypeId = req.ReadingTypeId,
                Value = req.Value,
                TimeStampUtc = DateTime.UtcNow // أو req.Timestamp
            };
        }

        public static SensorReadingDto ToSensorReadingDto(this SensorReading sr)
        {
            return new SensorReadingDto
            {
                Id = sr.Id,
                EquipmentId = sr.EquipmentId,
                Value = sr.Value,
                Timestamp = sr.TimeStampUtc,
                ReadingType = sr.Equipment?.ReadingType?.DisplayName ?? ""
            };
        }

        public static IEnumerable<SensorReadingDto> ToSensorReadingDtoSet(this IEnumerable<SensorReading> readings)
        {
            return readings.Select(sr => sr.ToSensorReadingDto());
        }

        // Alerts
        public static Alert ToAlert(this CreateAlertRequest req)
        {
            return new Alert
            {
                ZoneId = req.ZoneId,
                EquipmentId = req.EquipmentId,
                CropId = req.CropId,
                StageId = req.CropGrowthStageId,
                ReadingTypeId = req.ReadingTypeId,
                ThresholdType = Enum.TryParse<ThresholdType>(req.AlertType, out var alertType) ? alertType : ThresholdType.Unknown,
                Message = req.Message,
                Value = 00, // This should be set based on the context
                IsAcknowledged = false,
            };
        }

        public static AlertDto ToAlertDto(this Alert a)
        {
            return new AlertDto
            {
                Id = a.Id,
                ZoneId = a.ZoneId,
                EquipmentId = a.EquipmentId,
                CropId = a.CropId,
                CropName = a.Crop?.Name ?? "",
                CropGrowthStageId = a.StageId,
                StageName = a.CropGrowthStage?.StageName ?? "",
                ReadingTypeId = a.ReadingTypeId,
                ReadingTypeName = a.ReadingType?.DisplayName ?? "",
                Value  = a.Value,
                AlertType = a.ThresholdType.ToString(),
                Message = a.Message,
                Timestamp = a.DateOfCreattion,
                IsResolved = a.IsAcknowledged,
                ResolvedAt = a.IsAcknowledged ? a.AcknowledgedAt : null
            };
        }

        public static IEnumerable<AlertDto> ToAlertDtoSet(this IEnumerable<Alert> alerts)
        {
            return alerts.Select(a => a.ToAlertDto());
        }
    }
}
