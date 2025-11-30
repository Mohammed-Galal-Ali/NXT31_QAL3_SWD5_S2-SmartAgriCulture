using Base.DAL.Config.BaseConfig;
using Base.DAL.Models.SystemModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Base.DAL.Config.SystemConfig
{
    public class CropGrowthStageConfiguration : BaseEntityConfigurations<CropGrowthStage>
    {
        public override void Configure(EntityTypeBuilder<CropGrowthStage> builder)
        {
            base.Configure(builder);

            builder.Property(b => b.StageName).IsRequired().HasColumnType("varchar").HasMaxLength(100);
            builder.Property(b => b.Order).IsRequired();
            builder.Property(b => b.DurationDays).IsRequired();


            // crop growth stage <---> Crop
            builder.HasOne(cgs => cgs.Crop)
                   .WithMany(c => c.CropGrowthStages)
                   .HasForeignKey(cgs => cgs.CropId)
                   .OnDelete(DeleteBehavior.NoAction)
;


            // crop growth stage <---> CropStageRequirments

            builder.HasMany(cgs => cgs.CropStageRequirments)
                   .WithOne(csr => csr.CropGrowthStage)
                   .HasForeignKey(csr => csr.CropGrowthStageId)
                   .OnDelete(DeleteBehavior.NoAction)
;
            // crop growth stage <---> Alerts
            builder.HasMany(cgs => cgs.Alerts)
                   .WithOne(a => a.CropGrowthStage)
                   .HasForeignKey(a => a.StageId)
                   .OnDelete(DeleteBehavior.NoAction)
;

            // crop growth stage <---> ZoneCrops

            builder.HasMany(cgs => cgs.ZoneCrops)
                   .WithOne(zc => zc.CropGrowthStage)
                   .HasForeignKey(zc => zc.CropGrowthStageId)
                   .OnDelete(DeleteBehavior.NoAction)
;

        }
    }
}
