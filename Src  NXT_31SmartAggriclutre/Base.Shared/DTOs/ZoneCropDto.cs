using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Shared.DTOs
{
    public class CreateZoneCropRequest
    {
        public string ZoneId { get; set; } = null!;
        public string CropId { get; set; } = null!;
        public string? CropGrowthStageId { get; set; } // المرحلة الحالية
        public DateOnly PlantingDate { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class UpdateZoneCropRequest
    {
        public string? CropGrowthStageId { get; set; }
        public bool IsActive { get; set; }
    }

    public class ZoneCropDto
    {
        public string Id { get; set; } = null!;
        public string ZoneId { get; set; } = null!;
        public string CropId { get; set; } = null!;
        public string CropName { get; set; } = null!;
        public string? CropGrowthStageId { get; set; }
        public string? StageName { get; set; }
        public DateOnly PlantingDate { get; set; }
        public bool IsActive { get; set; }
    }

    public class ZoneCropListDto
    {
        public List<ZoneCropDto> ZoneCrops { get; set; } = new();
        public int TotalCount { get; set; }
    }
}
