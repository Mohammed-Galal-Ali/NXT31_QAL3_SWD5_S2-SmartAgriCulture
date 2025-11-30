using Base.DAL.Enums;
using Base.DAL.Models.BaseModels;

namespace Base.DAL.Models.SystemModels
{
    public class Alert : BaseEntity
    {
        public double Value { get; set; }

        public string Message { get; set; } = null!; //Max Value 1000  
        public bool IsAcknowledged { get; set; } = false; //Default Value False
        public DateTime AcknowledgedAt { get; set; }
        public ThresholdType ThresholdType { get; set; }  //Enum

        public string EquipmentId { get; set; }            //FK Ref For Equipment
        public string ZoneId { get; set; }                 //FK Ref For Zone
        public string ReadingTypeId { get; set; }          //FK Ref For ReadingType
        public string CropId { get; set; }                 //FK Ref For Crop
        public string StageId { get; set; }                //FK Ref For CropGrowthStage

        //Relations

        public virtual Equipment Equipment { get; set; }
        public virtual Zone Zone { get; set; }
        public virtual ReadingType ReadingType { get; set; }
        public virtual Crop Crop { get; set; }
        public virtual CropGrowthStage CropGrowthStage { get; set; }

    }
}
