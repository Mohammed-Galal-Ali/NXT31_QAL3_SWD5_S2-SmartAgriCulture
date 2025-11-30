using Base.DAL.Config.BaseConfig;
using Base.DAL.Models.SystemModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Base.DAL.Config.SystemConfig
{
    public class EquipmentConfiguration : BaseEntityConfigurations<Equipment>
    {
        public override void Configure(EntityTypeBuilder<Equipment> builder)
        {
            base.Configure(builder);


            builder.Property(e => e.SerialNumber).HasMaxLength(100);
            builder.Property(e => e.EquipmentModel).HasMaxLength(100);
            builder.Property(e => e.Status).IsRequired();




            //  Equipment <---> SensorReadings

            builder.HasMany(e => e.SensorReadings)
                   .WithOne(sr => sr.Equipment)
                   .HasForeignKey(sr => sr.EquipmentId)
                   .OnDelete(DeleteBehavior.NoAction)
;

            //  Equipment <---> Alerts
            builder.HasMany(e => e.Alerts)
                   .WithOne(a => a.Equipment)
                   .HasForeignKey(a => a.EquipmentId)
                   .OnDelete(DeleteBehavior.NoAction)
;

            //  Equipment <---> Zones
            builder.HasOne(e => e.Zone)
                   .WithMany(z => z.Equipments)
                   .HasForeignKey(e => e.ZoneId)
                   .OnDelete(DeleteBehavior.NoAction)
;

            //  Equipment <---> ReadingTypes
            builder.HasOne(e => e.ReadingType)
                   .WithMany(rt => rt.Equipments)
                   .HasForeignKey(e => e.ReadingTypeId)
                   .OnDelete(DeleteBehavior.NoAction)
;



        }
    }
}
