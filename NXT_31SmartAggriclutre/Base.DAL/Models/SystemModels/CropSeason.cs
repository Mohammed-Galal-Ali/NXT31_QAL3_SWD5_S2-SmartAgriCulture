using Base.DAL.Models.BaseModels;

namespace Base.DAL.Models.SystemModels
{
    public class CropSeason : BaseEntity
    {
        public string SeasonName { get; set; } = null!;
        public int PlantingStartMonth { get; set; }
        public int PlantingEndMonth { get; set; }
        public int HarvestStartMonth { get; set; }
        public int HarvestEndMonth { get; set; }
        public string ExpectedRangeDays { get; set; }   //Max Value 20
        public string Notes { get; set; }

        public string CropId { get; set; }  //FK Ref For Crop

        //Relations

        public virtual Crop Crop { get; set; }


    }
}
