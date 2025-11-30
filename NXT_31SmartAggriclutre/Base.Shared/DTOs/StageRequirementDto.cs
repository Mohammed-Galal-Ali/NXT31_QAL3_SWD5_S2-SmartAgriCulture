using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Shared.DTOs
{
    public class StageRequirementDto
    {
        public string Id { get; set; } = string.Empty;
        public string ReadingTypeCode { get; set; } = string.Empty;
        public string ReadingTypeName { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
        public double OptimalMin { get; set; }
        public double OptimalMax { get; set; }
    }

    public class CreateRequirementRequest
    {
        [Required]
        public required string ReadingTypeId { get; set; } = string.Empty;
        [Required]
        public required double MinValue { get; set; }
        [Required]
        public required double MaxValue { get; set; }
        [Required]
        public required double OptimalMin { get; set; }
        [Required]
        public required double OptimalMax { get; set; }
        [Required]
        public required string CropGrowthStageId { get; set; } = string.Empty;
    }
}
