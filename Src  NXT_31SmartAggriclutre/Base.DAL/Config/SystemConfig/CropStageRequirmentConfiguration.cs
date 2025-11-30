using Base.DAL.Config.BaseConfig;
using Base.DAL.Models.SystemModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Base.DAL.Config.SystemConfig
{
    public class CropStageRequirmentConfiguration : BaseEntityConfigurations<CropStageRequirment>
    {
        public override void Configure(EntityTypeBuilder<CropStageRequirment> builder)
        {

            base.Configure(builder);




            // crop stage requierments <---> crop growth stage
            builder.HasOne(cgs => cgs.CropGrowthStage)
                   .WithMany(csg => csg.CropStageRequirments)
                   .HasForeignKey(csg => csg.CropGrowthStageId);

            // croop stage requirements <--->  readingtype 

            builder.HasOne(rt => rt.ReadingType)
                   .WithMany(csg => csg.cropStageRequirments)
                   .HasForeignKey(csg => csg.ReadingTypeId)
                   .OnDelete(DeleteBehavior.NoAction)
;

        }
    }
}
