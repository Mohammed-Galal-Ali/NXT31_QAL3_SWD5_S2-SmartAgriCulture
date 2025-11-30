using Base.DAL.Config.BaseConfig;
using Base.DAL.Models.SystemModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Base.DAL.Config.SystemConfig
{
    public class CropSeasonConfiguration : BaseEntityConfigurations<CropSeason>
    {
        public override void Configure(EntityTypeBuilder<CropSeason> builder)
        {
            base.Configure(builder);

            builder.Property(b => b.SeasonName).HasColumnType("varchar").HasMaxLength(100).IsRequired();
            builder.Property(b => b.PlantingStartMonth).IsRequired();
            builder.Property(b => b.PlantingEndMonth).IsRequired();
            builder.Property(b => b.HarvestStartMonth).IsRequired();
            builder.Property(b => b.HarvestEndMonth).IsRequired();
            builder.Property(b => b.PlantingStartMonth).HasColumnType("varchar(20)");

            //cropseason <--->  crop 
            builder.HasOne(cs => cs.Crop)
                   .WithMany(c => c.CropSeasons)
                   .HasForeignKey(cs => cs.CropId)
                   .OnDelete(DeleteBehavior.NoAction);


        }
    }
}
