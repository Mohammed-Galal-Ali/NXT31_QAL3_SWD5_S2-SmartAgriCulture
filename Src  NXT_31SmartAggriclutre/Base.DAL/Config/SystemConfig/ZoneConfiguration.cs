using Base.DAL.Config.BaseConfig;
using Base.DAL.Models.SystemModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Base.DAL.Config.SystemConfig
{
    public class ZoneConfiguration : BaseEntityConfigurations<Zone>
    {
        public override void Configure(EntityTypeBuilder<Zone> builder)
        {
            base.Configure(builder);

            builder.Property(z => z.Name).IsRequired();
            builder.Property(z => z.SoilType).HasMaxLength(100);

            //Zone<--> Farm 

            builder.HasOne(z => z.Farm)
                   .WithMany(f => f.Zones)
                   .HasForeignKey(z => z.FarmId)
                   .OnDelete(DeleteBehavior.NoAction)
;

            //Zone <---> Equipment
            builder.HasMany(z => z.Equipments)
                   .WithOne(e => e.Zone)
                   .HasForeignKey(e => e.ZoneId)
                   .OnDelete(DeleteBehavior.NoAction)
;

            //Zone <---> ZoneCrop
            builder.HasMany(z => z.ZoneCrops)
                   .WithOne(zc => zc.Zone)
                   .HasForeignKey(zc => zc.ZoneId)
                   .OnDelete(DeleteBehavior.NoAction)
;

            //Zone <---> Alert

            builder.HasMany(z => z.Alerts)
                   .WithOne(a => a.Zone)
                   .HasForeignKey(a => a.ZoneId)
                   .OnDelete(DeleteBehavior.NoAction)
;



        }
    }
}
