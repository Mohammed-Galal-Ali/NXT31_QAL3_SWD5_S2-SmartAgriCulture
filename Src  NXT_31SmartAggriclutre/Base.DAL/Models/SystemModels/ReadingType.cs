using Base.DAL.Models.BaseModels;

namespace Base.DAL.Models.SystemModels
{
    public class ReadingType : BaseEntity
    {
        public string Code { get; set; } = null!;  //Max Value 50     
        public string Category { get; set; }       //Max Value 50 

        public string DisplayName { get; set; } = null!;    //Max Value 100
        public string Unit { get; set; } = null!;          //Max Value 20


        //Relations

        public virtual ICollection<CropStageRequirment> cropStageRequirments { get; set; } = new HashSet<CropStageRequirment>();
        public virtual ICollection<Alert> Alerts { get; set; } = new HashSet<Alert>();
        public virtual ICollection<SensorReading> SensorReadings { get; set; } = new HashSet<SensorReading>();
        public virtual ICollection<Equipment> Equipments { get; set; } = new HashSet<Equipment>();
    }
}
