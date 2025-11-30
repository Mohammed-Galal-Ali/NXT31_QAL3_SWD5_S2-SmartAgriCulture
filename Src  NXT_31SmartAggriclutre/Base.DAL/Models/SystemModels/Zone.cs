using Base.DAL.Models.BaseModels;

namespace Base.DAL.Models.SystemModels
{
    public class Zone : BaseEntity
    {
        public string Name { get; set; } = null!;

        public double Area { get; set; }

        public string SoilType { get; set; }   //Max value 100

        public string FarmId { get; set; }         //FK ref to Farm

        //Relation
        public virtual Farm Farm { get; set; }
        public virtual ICollection<ZoneCrop> ZoneCrops { get; set; } = new HashSet<ZoneCrop>();
        public virtual ICollection<Alert> Alerts { get; set; } = new HashSet<Alert>();
        public virtual ICollection<Equipment> Equipments { get; set; } = new HashSet<Equipment>();
    }
}
