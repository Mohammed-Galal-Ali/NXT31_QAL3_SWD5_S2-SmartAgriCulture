using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Shared.DTOs
{
    // للـ SensorReadings
    public class CreateSensorReadingRequest
    {
        public string EquipmentId { get; set; } = null!;
        public string ReadingTypeId { get; set; } //FK Ref For Reading Types
        public double Value { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow; // optional، لو مش مرسل يستخدم Now
    }

    public class SensorReadingDto
    {
        public string Id { get; set; } = null!;
        public string EquipmentId { get; set; } = null!;
        public double Value { get; set; }
        public DateTime Timestamp { get; set; }
        public string ReadingType { get; set; } = null!; // من Equipment.ReadingType.DisplayName
    }

    public class SensorReadingListDto
    {
        public List<SensorReadingDto> Readings { get; set; } = new();
        public int TotalCount { get; set; }
    }

    // للـ Alerts
    public class CreateAlertRequest
    {
        public string SensorReadingId { get; set; }
        public string ZoneId { get; set; } = null!;
        public string? EquipmentId { get; set; }
        public string? CropId { get; set; }
        public string? CropGrowthStageId { get; set; }
        public string? ReadingTypeId { get; set; }
        public string AlertType { get; set; } = null!; // High/Low
        public string Message { get; set; } = null!;
        public double Value { get; set; }
    }

    public class AlertDto
    {
        public string Id { get; set; } = null!;
        public string ZoneId { get; set; } = null!;
        public string? EquipmentId { get; set; }
        public string? CropId { get; set; }
        public string? CropName { get; set; }
        public string? CropGrowthStageId { get; set; }
        public string? StageName { get; set; }
        public string? ReadingTypeId { get; set; }
        public string? ReadingTypeName { get; set; }
        public double Value { get; set; }
        //public string? SensorReadingId { get; set; }
        public string AlertType { get; set; } = null!;
        public string Message { get; set; } = null!;
        public string Severity { get; set; } = null!;
        public DateTime Timestamp { get; set; }
        public bool IsResolved { get; set; }
        public DateTime? ResolvedAt { get; set; }
    }

    public class AlertListDto
    {
        public List<AlertDto> Alerts { get; set; } = new();
        public int TotalCount { get; set; }
    }
}
