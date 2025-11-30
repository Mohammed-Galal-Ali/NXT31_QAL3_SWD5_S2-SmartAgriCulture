using Base.DAL.Models.BaseModels;

namespace Base.DAL.Models.SystemModels
{
    public class SensorReading : BaseEntity
    {
        public double Value { get; set; }

        public DateTime TimeStampUtc { get; set; }

        public bool IsAnomaly { get; set; } = false;   //Default Value 

        public string EquipmentId { get; set; } // FK Ref for Equipment

        public string ReadingTypeId { get; set; } //FK Ref For Reading Types


        //Relations 
        public virtual ReadingType ReadingType { get; set; }
        public virtual Equipment Equipment { get; set; }

    }
}
