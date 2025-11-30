using Base.DAL.Models.BaseModels;

namespace Base.DAL.Models.SystemModels
{
    public class CropStageRequirment : BaseEntity
    {
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
        public double OptimalMin { get; set; }
        public double OptimalMax { get; set; }

        public string CropGrowthStageId { get; set; } //FK Ref For CropGrowthStage

        public string ReadingTypeId { get; set; } //FK Ref For ReadingType

        //Relations
        public virtual CropGrowthStage CropGrowthStage { get; set; }
        public virtual ReadingType ReadingType { get; set; }
    }
}
