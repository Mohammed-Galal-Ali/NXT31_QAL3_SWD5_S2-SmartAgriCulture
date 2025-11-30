using Base.DAL.Models.BaseModels;

namespace Base.DAL.Models.SystemModels
{
    public class ZoneCrop : BaseEntity
    {
        public DateOnly PlantedAt { get; set; }
        public DateOnly ExpectedHarvestAt { get; set; }
        public string? Notes { get; set; }

        public DateOnly ActualHarvestAt { get; set; }
        public double YieldWeightKg { get; set; }
        public bool IsActive { get; set; } = true; //Default True


        public string ZoneId { get; set; }  //FK Ref For Zone
        public string CropId { get; set; }  //FK Ref For Crop
        public string CropGrowthStageId { get; set; }  //FK Ref For CropGrowthStage

        //Relation 
        public virtual Zone Zone { get; set; }
        public virtual Crop Crop { get; set; }
        public virtual CropGrowthStage CropGrowthStage { get; set; }

    }
}
