using Base.DAL.Models.BaseModels;
using Base.Shared.Responses;
using Base.DAL.Models.SystemModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Base.DAL.Contexts
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            string? _userId = null;
            if (_httpContextAccessor.HttpContext is not null)
            {
                var reqservices = _httpContextAccessor.HttpContext.RequestServices;
                if (reqservices is not null)
                    using (var scope = reqservices.CreateScope())
                    {
                        var services = scope.ServiceProvider;
                        if (services is not null)
                            try
                            {
                                var _userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                                if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                                {
                                    var user = _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User).Result;
                                    _userId = user?.Id;
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                    }
            }

            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((BaseEntity)entityEntry.Entity).DateOfUpdate = DateTime.Now;
                ((BaseEntity)entityEntry.Entity).UpdatedById = _userId;

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).DateOfCreattion = DateTime.Now;
                    ((BaseEntity)entityEntry.Entity).CreatedById = _userId;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        #region DBSets
        #region BaseModels
        /*public DbSet<UserProfile> UserProfiles { get; set; }*/
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<OtpEntry> OtpEntries { get; set; }
        public DbSet<BlacklistedToken> BlacklistedTokens { get; set; }

        #endregion

        #region SystemModels
        public DbSet<Alert> Alerts { get; set; }
        public DbSet<Crop> Crops { get; set; }
        public DbSet<CropGrowthStage> CropGrowthStages { get; set; }
        public DbSet<CropSeason> CropSeasons { get; set; }
        public DbSet<CropStageRequirment> CropStageRequirments { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<Farm> Farms { get; set; }
        public DbSet<ReadingType> ReadingTypes { get; set; }
        public DbSet<SensorReading> SensorReadings { get; set; }
        public DbSet<Zone> Zones { get; set; }
        public DbSet<ZoneCrop> ZoneCrops { get; set; } 
        #endregion
        #endregion

    }
}
