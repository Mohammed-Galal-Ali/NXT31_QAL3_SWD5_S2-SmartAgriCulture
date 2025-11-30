using Base.DAL.Config.BaseConfig;
using Base.DAL.Models.SystemModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Base.DAL.Config.SystemConfig
{
    public class CropConfiguration : BaseEntityConfigurations<Crop>
    {
        public override void Configure(EntityTypeBuilder<Crop> builder)
        {
            base.Configure(builder);


            builder.Property(c => c.Name).IsRequired();

            //Crop <---> CropSeasons
            builder.HasMany(c => c.CropSeasons)
                   .WithOne(cs => cs.Crop)
                   .HasForeignKey(cs => cs.CropId)
                   .OnDelete(DeleteBehavior.NoAction);
            // Crop <---> Alerts

            builder.HasMany(c => c.Alerts)
                   .WithOne(a => a.Crop)
                   .HasForeignKey(a => a.CropId)
                   .OnDelete(DeleteBehavior.NoAction);

            // Crop <--> ZoneCrops

            builder.HasMany(c => c.ZoneCrops)
                    .WithOne(zc => zc.Crop)
                    .HasForeignKey(zc => zc.CropId)
                    .OnDelete(DeleteBehavior.NoAction);

            //Crop <---> CropGrowthStage

            builder.HasMany(c => c.CropGrowthStages)
                   .WithOne(cgs => cgs.Crop)
                   .HasForeignKey(cgs => cgs.CropId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
