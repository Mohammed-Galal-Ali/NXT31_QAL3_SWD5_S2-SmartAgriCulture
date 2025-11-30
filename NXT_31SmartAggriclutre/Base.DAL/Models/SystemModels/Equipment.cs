using Base.DAL.Enums;
using Base.DAL.Models.BaseModels;

namespace Base.DAL.Models.SystemModels
{
    public class Equipment : BaseEntity
    {
        public string SerialNumber { get; set; } //has Max Value 100

        public string EquipmentModel { get; set; } //has Max Value 100

        public EquipmentsStatus Status { get; set; }

        public DateTime InstalledAt { get; set; }

        public DateTime LastReadingAt { get; set; }

        public string ZoneId { get; set; } //FK ref For Zone
        public string ReadingTypeId { get; set; } //FK ref For Reading Type

        //Relations

        public virtual ICollection<Alert> Alerts { get; set; } = new HashSet<Alert>();
        public virtual ICollection<SensorReading> SensorReadings { get; set; } = new HashSet<SensorReading>();
        public virtual ReadingType ReadingType { get; set; }
        public virtual Zone Zone { get; set; }

    }
}
