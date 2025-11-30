using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Shared.DTOs
{
    public class FarmDto
    {
        public string Id { get; set; } = string.Empty;
        public  string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public  double Lat { get; set; }
        public  double Lon { get; set; }
        public  string Address { get; set; } = string.Empty; // تم تصحيح الإملاء من Addresse → Address
        public string OwnerId { get; set; } = string.Empty;
        public string OwnerName { get; set; } = string.Empty;
        public int ZonesCount { get; set; }
        public int ActiveCropsCount { get; set; }
        public DateTime DateOfCreation { get; set; }
        public DateTime DateOfUpdate { get; set; }
    }

    // DTOs/Farms/FarmListDto.cs
    public class FarmListDto
    {
        public List<FarmDto> Farms { get; set; } = new();
        public int TotalCount { get; set; }
    }

    // DTOs/Farms/CreateFarmRequest.cs
    public class CreateFarmRequest
    {
        [Required]
        public required string Name { get; set; } = string.Empty;
        [Required]
        public required string Code { get; set; } = string.Empty;
        [Required]
        public required double Lat { get; set; }
        [Required]
        public required double Lon { get; set; }
        [Required] 
        public required string Address { get; set; } = string.Empty;
        public string? OwnerId { get; set; } // لو Admin بيضيف لحد تاني، وإلا هيبقى المستخدم الحالي
    }

    // DTOs/Farms/UpdateFarmRequest.cs
    public class UpdateFarmRequest
    {
        public string? Name { get; set; }
        public string? Code { get; set; }
        public double? Lat { get; set; }
        public double? Lon { get; set; }
        public string? Address { get; set; }
    }
}
