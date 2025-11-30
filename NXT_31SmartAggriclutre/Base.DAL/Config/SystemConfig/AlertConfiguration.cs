using Base.DAL.Config.BaseConfig;
using Base.DAL.Models.SystemModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Base.DAL.Config.SystemConfig
{
    public class AlertConfiguration : BaseEntityConfigurations<Alert>
    {
        public override void Configure(EntityTypeBuilder<Alert> builder)
        {
            base.Configure(builder);
            builder.Property(b => b.Message).IsRequired().HasColumnType("varchar(1000)");
            builder.Property(b => b.IsAcknowledged).IsRequired().HasDefaultValue(false);


            // alert <---> equipments 
            builder.HasOne(e => e.Equipment)
                   .WithMany(a => a.Alerts)
                   .HasForeignKey(a => a.EquipmentId)
                   .OnDelete(DeleteBehavior.NoAction);
            // alert <--->  zone 

            builder.HasOne(z => z.Zone)
                   .WithMany(a => a.Alerts)
                   .HasForeignKey(a => a.ZoneId)
                   .OnDelete(DeleteBehavior.NoAction);
            // reading type <--->  alerts

            builder.HasOne(rt => rt.ReadingType)
                   .WithMany(a => a.Alerts)
                   .HasForeignKey(a => a.ReadingTypeId)
                   .OnDelete(DeleteBehavior.NoAction);

            // crops <--->  alerts
            builder.HasOne(c => c.Crop)
                   .WithMany(a => a.Alerts)
                   .HasForeignKey(a => a.CropId)
                   .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
