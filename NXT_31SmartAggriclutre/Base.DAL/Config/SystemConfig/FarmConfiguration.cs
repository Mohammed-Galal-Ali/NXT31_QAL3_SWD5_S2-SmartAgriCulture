using Base.DAL.Config.BaseConfig;
using Base.DAL.Models.SystemModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Base.DAL.Config.SystemConfig
{
    public class FarmConfiguration : BaseEntityConfigurations<Farm>
    {
        public override void Configure(EntityTypeBuilder<Farm> builder)
        {
            base.Configure(builder);

            builder.Property(f => f.Name).IsRequired();
            builder.Property(f => f.Code).IsRequired();
            builder.Property(f => f.Lat).IsRequired();
            builder.Property(f => f.Lon).IsRequired();
            builder.Property(f => f.Address).IsRequired();



            builder.HasMany(f => f.Zones)
                   .WithOne(z => z.Farm)
                   .HasForeignKey(z => z.FarmId)
                   .OnDelete(DeleteBehavior.NoAction)
;
        }
    }
}
