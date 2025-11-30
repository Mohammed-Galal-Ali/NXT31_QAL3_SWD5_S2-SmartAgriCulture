using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Shared.DTOs
{
    public class ZoneDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public double Area { get; set; }           // بالهكتار أو متر مربع (حسب اختيارك)
        public string SoilType { get; set; } = string.Empty;
        public string FarmId { get; set; } = string.Empty;
        public string FarmName { get; set; } = string.Empty;
        public int EquipmentsCount { get; set; }
        public int ActiveCropsCount { get; set; }
        public DateTime DateOfCreation { get; set; }
        public DateTime DateOfUpdate { get; set; }
    }


    // DTOs/Zones/ZoneListDto.cs
    public class ZoneListDto
    {
        public List<ZoneDto> Zones { get; set; } = new();
        public int TotalCount { get; set; }
    }

    // DTOs/Zones/CreateZoneRequest.cs
    public class CreateZoneRequest
    {
        [Required]
        public required string Name { get; set; } = string.Empty;
        [Required]
        public required double Area { get; set; }
        [Required]
        public required string SoilType { get; set; } = string.Empty;
        [Required]
        public required string FarmId { get; set; } = string.Empty;
    }

    // DTOs/Zones/UpdateZoneRequest.cs
    public class UpdateZoneRequest
    {
        public string? Name { get; set; }
        public double? Area { get; set; }
        public string? SoilType { get; set; }
    }

}
