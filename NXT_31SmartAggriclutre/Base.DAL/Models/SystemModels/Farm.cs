using Base.DAL.Models.BaseModels;

namespace Base.DAL.Models.SystemModels
{
    public class Farm : BaseEntity
    {
        public string Name { get; set; } = null!;   //Not Null

        public string Code { get; set; } = null!;  //Not Null

        public double Lat { get; set; }

        public double Lon { get; set; }

        public string Address { get; set; } = null!; //Not Null

        //Realtions
        public virtual ICollection<Zone> Zones { get; set; } = new HashSet<Zone>();

        //public int Id { get; set; }
        public string? UserId { get; set; } // FK إلى ApplicationUser

        // Navigation property
        public virtual ApplicationUser? Owner { get; set; }

    }
}
