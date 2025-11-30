using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Base.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    UserType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Crops",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfCreattion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfUpdate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Crops", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReadingTypes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfCreattion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfUpdate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReadingTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Farms",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lat = table.Column<float>(type: "real", nullable: false),
                    Lon = table.Column<float>(type: "real", nullable: false),
                    Addresse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfCreattion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfUpdate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Farms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Farms_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OtpEntries",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodeHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    Purpose = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Attempts = table.Column<int>(type: "int", nullable: false),
                    ResendCount = table.Column<int>(type: "int", nullable: false),
                    LastResendAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfCreattion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfUpdate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtpEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OtpEntries_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TokenHash = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByIp = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    CreatedByUserAgent = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ExpiresAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevokedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RevokedByIp = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    ReplacedByTokenHash = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ReasonRevoked = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfCreattion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfUpdate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "CropGrowthStages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StageName = table.Column<string>(type: "varchar", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    DurationDays = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CropId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfCreattion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfUpdate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CropGrowthStages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CropGrowthStages_Crops_CropId",
                        column: x => x.CropId,
                        principalTable: "Crops",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CropSeasons",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SeasonName = table.Column<string>(type: "varchar", nullable: false),
                    PlantingStartMonth = table.Column<string>(type: "varchar(20)", nullable: false),
                    PlantingEndMonth = table.Column<int>(type: "int", nullable: false),
                    HarvestStartMonth = table.Column<int>(type: "int", nullable: false),
                    HarvestEndMonth = table.Column<int>(type: "int", nullable: false),
                    ExpectedRangeDays = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CropId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfCreattion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfUpdate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CropSeasons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CropSeasons_Crops_CropId",
                        column: x => x.CropId,
                        principalTable: "Crops",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Zones",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Area = table.Column<float>(type: "real", nullable: false),
                    SoilType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FarmId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfCreattion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfUpdate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Zones_Farms_FarmId",
                        column: x => x.FarmId,
                        principalTable: "Farms",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CropStageRequirments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MinValue = table.Column<double>(type: "float", nullable: false),
                    MaxValue = table.Column<double>(type: "float", nullable: false),
                    OptimalMin = table.Column<double>(type: "float", nullable: false),
                    OptimalMax = table.Column<double>(type: "float", nullable: false),
                    CropGrowthStageId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReadingTypeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfCreattion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfUpdate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CropStageRequirments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CropStageRequirments_CropGrowthStages_CropGrowthStageId",
                        column: x => x.CropGrowthStageId,
                        principalTable: "CropGrowthStages",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CropStageRequirments_ReadingTypes_ReadingTypeId",
                        column: x => x.ReadingTypeId,
                        principalTable: "ReadingTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Equipments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EquipmentModel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    InstalledAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    LastReadingAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    ZoneId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReadingTypeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfCreattion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfUpdate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Equipments_ReadingTypes_ReadingTypeId",
                        column: x => x.ReadingTypeId,
                        principalTable: "ReadingTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Equipments_Zones_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "Zones",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ZoneCrops",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PlantedAt = table.Column<DateOnly>(type: "date", nullable: false),
                    ExpectedHarvestAt = table.Column<DateOnly>(type: "date", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActualHarvestAt = table.Column<DateOnly>(type: "date", nullable: false),
                    YieldWeightKg = table.Column<double>(type: "float", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ZoneId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CropId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CropGrowthStageId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfCreattion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfUpdate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZoneCrops", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZoneCrops_CropGrowthStages_CropGrowthStageId",
                        column: x => x.CropGrowthStageId,
                        principalTable: "CropGrowthStages",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ZoneCrops_Crops_CropId",
                        column: x => x.CropId,
                        principalTable: "Crops",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ZoneCrops_Zones_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "Zones",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Alerts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<double>(type: "float", nullable: false),
                    Message = table.Column<string>(type: "varchar(1000)", nullable: false),
                    IsAcknowledged = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    AcknowledgedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThresholdType = table.Column<int>(type: "int", nullable: false),
                    EquipmentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ZoneId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReadingTypeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CropId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StageId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfCreattion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfUpdate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alerts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alerts_CropGrowthStages_StageId",
                        column: x => x.StageId,
                        principalTable: "CropGrowthStages",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Alerts_Crops_CropId",
                        column: x => x.CropId,
                        principalTable: "Crops",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Alerts_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Alerts_ReadingTypes_ReadingTypeId",
                        column: x => x.ReadingTypeId,
                        principalTable: "ReadingTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Alerts_Zones_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "Zones",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SensorReadings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<float>(type: "real", nullable: false),
                    TimeStampUtc = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsAnomaly = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    EquipmentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReadingTypeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfCreattion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfUpdate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorReadings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SensorReadings_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SensorReadings_ReadingTypes_ReadingTypeId",
                        column: x => x.ReadingTypeId,
                        principalTable: "ReadingTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_CropId",
                table: "Alerts",
                column: "CropId");

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_EquipmentId",
                table: "Alerts",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_ReadingTypeId",
                table: "Alerts",
                column: "ReadingTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_StageId",
                table: "Alerts",
                column: "StageId");

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_ZoneId",
                table: "Alerts",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CropGrowthStages_CropId",
                table: "CropGrowthStages",
                column: "CropId");

            migrationBuilder.CreateIndex(
                name: "IX_CropSeasons_CropId",
                table: "CropSeasons",
                column: "CropId");

            migrationBuilder.CreateIndex(
                name: "IX_CropStageRequirments_CropGrowthStageId",
                table: "CropStageRequirments",
                column: "CropGrowthStageId");

            migrationBuilder.CreateIndex(
                name: "IX_CropStageRequirments_ReadingTypeId",
                table: "CropStageRequirments",
                column: "ReadingTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_ReadingTypeId",
                table: "Equipments",
                column: "ReadingTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_ZoneId",
                table: "Equipments",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_Farms_UserId",
                table: "Farms",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OtpEntries_UserId",
                table: "OtpEntries",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ReadingTypes_Code",
                table: "ReadingTypes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_TokenHash",
                table: "RefreshTokens",
                column: "TokenHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SensorReadings_EquipmentId",
                table: "SensorReadings",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_SensorReadings_ReadingTypeId",
                table: "SensorReadings",
                column: "ReadingTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ZoneCrops_CropGrowthStageId",
                table: "ZoneCrops",
                column: "CropGrowthStageId");

            migrationBuilder.CreateIndex(
                name: "IX_ZoneCrops_CropId",
                table: "ZoneCrops",
                column: "CropId");

            migrationBuilder.CreateIndex(
                name: "IX_ZoneCrops_ZoneId",
                table: "ZoneCrops",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_Zones_FarmId",
                table: "Zones",
                column: "FarmId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alerts");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CropSeasons");

            migrationBuilder.DropTable(
                name: "CropStageRequirments");

            migrationBuilder.DropTable(
                name: "OtpEntries");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "SensorReadings");

            migrationBuilder.DropTable(
                name: "ZoneCrops");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Equipments");

            migrationBuilder.DropTable(
                name: "CropGrowthStages");

            migrationBuilder.DropTable(
                name: "ReadingTypes");

            migrationBuilder.DropTable(
                name: "Zones");

            migrationBuilder.DropTable(
                name: "Crops");

            migrationBuilder.DropTable(
                name: "Farms");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
