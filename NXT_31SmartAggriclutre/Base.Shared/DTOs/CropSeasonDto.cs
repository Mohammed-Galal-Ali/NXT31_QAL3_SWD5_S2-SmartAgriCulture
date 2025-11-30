using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Shared.DTOs
{
    // DTOs/Crops/CropSeasonDto.cs (اختصارًا)
    public class CropSeasonDto
    {
        public string Id { get; set; } = string.Empty;
        public string SeasonName { get; set; } = string.Empty;
        public int PlantingStartMonth { get; set; } 
        public string ExpectedRangeDays { get; set; } = string.Empty;
    }
    public class CreateSeasonRequest
    {
        public string SeasonName { get; set; } = null!;
        public int PlantingStartMonth { get; set; }
        public int PlantingEndMonth { get; set; }
        public int HarvestStartMonth { get; set; }
        public int HarvestEndMonth { get; set; }
        public string ExpectedRangeDays { get; set; }   //Max Value 20
        public string Notes { get; set; }

        public string CropId { get; set; }  //FK Ref For Crop
    }

    public class UpdateSeasonRequest
    {
        public string SeasonName { get; set; } = null!;
        public int PlantingStartMonth { get; set; }
        public int PlantingEndMonth { get; set; }
        public int HarvestStartMonth { get; set; }
        public int HarvestEndMonth { get; set; }
        public string ExpectedRangeDays { get; set; }   //Max Value 20
        public string Notes { get; set; }
    }
}
