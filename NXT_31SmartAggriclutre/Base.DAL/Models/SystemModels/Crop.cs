using Base.DAL.Models.BaseModels;

namespace Base.DAL.Models.SystemModels
{
    public class Crop : BaseEntity
    {
        public required string Name { get; set; }

        //Relations
        public virtual ICollection<ZoneCrop> ZoneCrops { get; set; } = new HashSet<ZoneCrop>();
        public virtual ICollection<CropGrowthStage> CropGrowthStages { get; set; } = new HashSet<CropGrowthStage>();
        public virtual ICollection<CropSeason> CropSeasons { get; set; } = new HashSet<CropSeason>();
        public virtual ICollection<Alert> Alerts { get; set; } = new HashSet<Alert>();
    }
}
