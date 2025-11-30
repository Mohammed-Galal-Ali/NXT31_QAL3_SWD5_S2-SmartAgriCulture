using Base.DAL.Config.BaseConfig;
using Base.DAL.Models.SystemModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Base.DAL.Config.SystemConfig
{
    public class SensorReadingConfiguration : BaseEntityConfigurations<SensorReading>
    {
        public override void Configure(EntityTypeBuilder<SensorReading> builder)
        {
            base.Configure(builder);

            builder.Property(sr => sr.Value).IsRequired();
            builder.Property(sr => sr.TimeStampUtc)
                   .HasColumnType("datetime")
                   .IsRequired();

            builder.Property(sr => sr.IsAnomaly).HasDefaultValue(false);

            //SensorReading <---> ReadingTypes
            builder.HasOne(sr => sr.ReadingType)
                   .WithMany(rt => rt.SensorReadings)
                   .HasForeignKey(sr => sr.ReadingTypeId)
                   .OnDelete(DeleteBehavior.NoAction)
;

            // sensor readings <---> equipments
            builder.HasOne(e => e.Equipment)
                   .WithMany(sr => sr.SensorReadings)
                   .HasForeignKey(sr => sr.EquipmentId)
                   .OnDelete(DeleteBehavior.NoAction)
;

        }
    }
}
