using Base.DAL.Config.BaseConfig;
using Base.DAL.Models.SystemModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Base.DAL.Config.SystemConfig
{
    public class ZoneCropConfiguration : BaseEntityConfigurations<ZoneCrop>
    {
        public override void Configure(EntityTypeBuilder<ZoneCrop> builder)
        {
            base.Configure(builder);


            builder.Property(b => b.PlantedAt).IsRequired();
            builder.Property(b => b.IsActive).HasDefaultValue(true);
            builder.Property(b => b.ActualHarvestAt).HasColumnType("date");
            builder.Property(b => b.ExpectedHarvestAt).HasColumnType("date");


            // zonecrops <--->  zone
            builder.HasOne(zc => zc.Zone)
                   .WithMany(z => z.ZoneCrops)
                   .HasForeignKey(zc => zc.ZoneId)
                   .OnDelete(DeleteBehavior.NoAction)
;

            // zonecrops <---> crop
            builder.HasOne(c => c.Crop)
                   .WithMany(zc => zc.ZoneCrops)
                   .HasForeignKey(zc => zc.CropId)
                   .OnDelete(DeleteBehavior.NoAction)
;

            builder.HasOne(cgs => cgs.CropGrowthStage)
                   .WithMany(zc => zc.ZoneCrops)
                   .HasForeignKey(zc => zc.CropGrowthStageId)
                   .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
