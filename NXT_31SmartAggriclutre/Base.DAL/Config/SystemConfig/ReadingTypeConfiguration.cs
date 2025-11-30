using Base.DAL.Config.BaseConfig;
using Base.DAL.Models.SystemModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Base.DAL.Config.SystemConfig
{
    public class ReadingTypeConfiguration : BaseEntityConfigurations<ReadingType>
    {
        public override void Configure(EntityTypeBuilder<ReadingType> builder)
        {
            base.Configure(builder);

            builder.Property(rt => rt.Code).HasMaxLength(50).IsRequired();
            builder.HasIndex(rt => rt.Code).IsUnique();
            builder.Property(rt => rt.Category).HasMaxLength(50);
            builder.Property(rt => rt.DisplayName).HasMaxLength(100).IsRequired();
            builder.Property(rt => rt.Unit).HasMaxLength(20).IsRequired();


            // ReadingTypes <---> CropGrowthStage
            builder.HasMany(rt => rt.cropStageRequirments)
                   .WithOne(csr => csr.ReadingType)
                   .HasForeignKey(csr => csr.ReadingTypeId)
                   .OnDelete(DeleteBehavior.NoAction)
;

            // ReadingTypes <---> Alerts

            builder.HasMany(rt => rt.Alerts)
                   .WithOne(a => a.ReadingType)
                   .HasForeignKey(a => a.ReadingTypeId)
                   .OnDelete(DeleteBehavior.NoAction)
;

            // ReadingTypes <---> Equipments
            builder.HasMany(rt => rt.Equipments)
                   .WithOne(e => e.ReadingType)
                   .HasForeignKey(e => e.ReadingTypeId)
                   .OnDelete(DeleteBehavior.NoAction)
;

            // ReadingTypes <---> SensorReadings
            builder.HasMany(rt => rt.SensorReadings)
                   .WithOne(sr => sr.ReadingType)
                   .HasForeignKey(sr => sr.ReadingTypeId)
                   .OnDelete(DeleteBehavior.NoAction)
;

        }
    }
}
