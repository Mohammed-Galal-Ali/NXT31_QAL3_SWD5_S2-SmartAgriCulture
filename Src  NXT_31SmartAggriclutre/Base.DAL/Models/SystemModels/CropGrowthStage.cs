using Base.DAL.Models.BaseModels;

namespace Base.DAL.Models.SystemModels
{
    public class CropGrowthStage : BaseEntity
    {
        public string StageName { get; set; } = null!;
        public int Order { get; set; }
        public int DurationDays { get; set; }
        public string Description { get; set; }

        public string CropId { get; set; }    //FK Ref For Crop

        //Relations

        public virtual Crop Crop { get; set; }
        public virtual ICollection<Alert> Alerts { get; set; } = new HashSet<Alert>();
        public virtual ICollection<ZoneCrop> ZoneCrops { get; set; } = new HashSet<ZoneCrop>();
        public virtual ICollection<CropStageRequirment> CropStageRequirments { get; set; } = new HashSet<CropStageRequirment>();

    }
}
