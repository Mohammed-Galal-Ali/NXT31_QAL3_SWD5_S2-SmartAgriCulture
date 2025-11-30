using Base.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Shared.DTOs
{
    public class CropDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int StagesCount { get; set; }
        public int SeasonsCount { get; set; }
        public DateTime DateOfCreation { get; set; }
    }

    // DTOs/Crops/CropDetailsDto.cs  ← لما نجيب محصول بالتفصيل
    public class CropDetailsDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public List<CropStageDto> GrowthStages { get; set; } = new();
        public List<CropSeasonDto> Seasons { get; set; } = new();
        public DateTime DateOfCreation { get; set; }
        public DateTime DateOfUpdate { get; set; }
    }

    // DTOs/Crops/CropListDto.cs
    public class CropListDto
    {
        public List<CropDto> Crops { get; set; } = new();
        public int TotalCount { get; set; }
    }

    // DTOs/Crops/CreateCropRequest.cs
    public class CreateCropRequest
    {
        [Required]
        public required string Name { get; set; } = string.Empty;
    }

    // DTOs/Crops/UpdateCropRequest.cs
    public class UpdateCropRequest
    {
        public string? Name { get; set; }
    }
}