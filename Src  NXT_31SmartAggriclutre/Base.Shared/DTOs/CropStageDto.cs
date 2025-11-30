using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Shared.DTOs
{
    public class CropStageDto
    {
        public string Id { get; set; } = string.Empty;
        public string StageName { get; set; } = string.Empty;
        public int Order { get; set; }
        public int DurationDays { get; set; }
        public string Description { get; set; } = string.Empty;
        public List<StageRequirementDto> Requirements { get; set; } = new();
    }

    public class CreateStageRequest
    {
        [Required]
        public required string StageName { get; set; } = string.Empty;
        [Required]
        public required int Order { get; set; }
        [Required]
        public required int DurationDays { get; set; }
        public string Description { get; set; } = string.Empty;
        [Required]
        public required string CropId { get; set; } = string.Empty;
    }

    public class UpdateStageRequest
    {
        public string? StageName { get; set; }
        public int? Order { get; set; }
        public int? DurationDays { get; set; }
        public string? Description { get; set; }
    }
}
