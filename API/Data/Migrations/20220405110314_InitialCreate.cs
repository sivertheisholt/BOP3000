using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Activity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Identifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Success = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Success = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                name: "CountryIso",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwoLetterCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThreeLetterCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumericCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryIso", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Log",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Requirement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requirement", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SteamAppid = table.Column<int>(type: "int", nullable: false),
                    RequiredAge = table.Column<int>(type: "int", nullable: false),
                    IsFree = table.Column<bool>(type: "bit", nullable: false),
                    ControllerSupport = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DetailedDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AboutTheGame = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShortDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupportedLanguages = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HeaderImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LegalNotice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Background = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dlc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Developers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Publishers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppInfoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppData_AppInfo_AppInfoId",
                        column: x => x.AppInfoId,
                        principalTable: "AppInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppListInfo",
                columns: table => new
                {
                    AppListInfoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppListId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppListInfo", x => x.AppListInfoId);
                    table.ForeignKey(
                        name: "FK_AppListInfo_AppList_AppListId",
                        column: x => x.AppListId,
                        principalTable: "AppList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
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
                name: "ActivityLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AppUserId = table.Column<int>(type: "int", nullable: false),
                    ActivityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityLog_Activity_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivityLog_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
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
                    UserId = table.Column<int>(type: "int", nullable: false)
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
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
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
                    UserId = table.Column<int>(type: "int", nullable: false),
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
                name: "AppUserProfile",
                columns: table => new
                {
                    AppUserId = table.Column<int>(type: "int", nullable: false),
                    Birthday = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryIsoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserProfile", x => x.AppUserId);
                    table.ForeignKey(
                        name: "FK_AppUserProfile_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppUserProfile_CountryIso_CountryIsoId",
                        column: x => x.CountryIsoId,
                        principalTable: "CountryIso",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Lobby",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaxUsers = table.Column<int>(type: "int", nullable: false),
                    AdminUid = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LobbyDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    GameName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GameType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FinishedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LogId = table.Column<int>(type: "int", nullable: true),
                    LobbyRequirementId = table.Column<int>(type: "int", nullable: true),
                    Users = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Finished = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lobby", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lobby_Log_LogId",
                        column: x => x.LogId,
                        principalTable: "Log",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Lobby_Requirement_LobbyRequirementId",
                        column: x => x.LobbyRequirementId,
                        principalTable: "Requirement",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Achievements",
                columns: table => new
                {
                    AppDataId = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Achievements", x => x.AppDataId);
                    table.ForeignKey(
                        name: "FK_Achievements_AppData_AppDataId",
                        column: x => x.AppDataId,
                        principalTable: "AppData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    AppDataId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => new { x.Id, x.AppDataId });
                    table.ForeignKey(
                        name: "FK_Category_AppData_AppDataId",
                        column: x => x.AppDataId,
                        principalTable: "AppData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContentDescriptors",
                columns: table => new
                {
                    AppDataId = table.Column<int>(type: "int", nullable: false),
                    Ids = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentDescriptors", x => x.AppDataId);
                    table.ForeignKey(
                        name: "FK_ContentDescriptors_AppData_AppDataId",
                        column: x => x.AppDataId,
                        principalTable: "AppData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Genre",
                columns: table => new
                {
                    GenreId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppDataId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genre", x => x.GenreId);
                    table.ForeignKey(
                        name: "FK_Genre_AppData_AppDataId",
                        column: x => x.AppDataId,
                        principalTable: "AppData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LinuxRequirements",
                columns: table => new
                {
                    AppDataId = table.Column<int>(type: "int", nullable: false),
                    Minimum = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Recommended = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinuxRequirements", x => x.AppDataId);
                    table.ForeignKey(
                        name: "FK_LinuxRequirements_AppData_AppDataId",
                        column: x => x.AppDataId,
                        principalTable: "AppData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MacRequirements",
                columns: table => new
                {
                    AppDataId = table.Column<int>(type: "int", nullable: false),
                    Minimum = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Recommended = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MacRequirements", x => x.AppDataId);
                    table.ForeignKey(
                        name: "FK_MacRequirements_AppData_AppDataId",
                        column: x => x.AppDataId,
                        principalTable: "AppData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Metacritic",
                columns: table => new
                {
                    AppDataId = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Metacritic", x => x.AppDataId);
                    table.ForeignKey(
                        name: "FK_Metacritic_AppData_AppDataId",
                        column: x => x.AppDataId,
                        principalTable: "AppData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Movy",
                columns: table => new
                {
                    MovyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Thumbnail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Highlight = table.Column<bool>(type: "bit", nullable: false),
                    AppDataId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movy", x => x.MovyId);
                    table.ForeignKey(
                        name: "FK_Movy_AppData_Id",
                        column: x => x.Id,
                        principalTable: "AppData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PackageGroup",
                columns: table => new
                {
                    PackageGroupId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SelectionText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SaveText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayType = table.Column<int>(type: "int", nullable: false),
                    IsRecurringSubscription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppDataId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageGroup", x => x.PackageGroupId);
                    table.ForeignKey(
                        name: "FK_PackageGroup_AppData_AppDataId",
                        column: x => x.AppDataId,
                        principalTable: "AppData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PcRequirements",
                columns: table => new
                {
                    AppDataId = table.Column<int>(type: "int", nullable: false),
                    Minimum = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Recommended = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PcRequirements", x => x.AppDataId);
                    table.ForeignKey(
                        name: "FK_PcRequirements_AppData_AppDataId",
                        column: x => x.AppDataId,
                        principalTable: "AppData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Platforms",
                columns: table => new
                {
                    AppDataId = table.Column<int>(type: "int", nullable: false),
                    Windows = table.Column<bool>(type: "bit", nullable: false),
                    Mac = table.Column<bool>(type: "bit", nullable: false),
                    Linux = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Platforms", x => x.AppDataId);
                    table.ForeignKey(
                        name: "FK_Platforms_AppData_AppDataId",
                        column: x => x.AppDataId,
                        principalTable: "AppData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Price",
                columns: table => new
                {
                    AppDataId = table.Column<int>(type: "int", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Initial = table.Column<int>(type: "int", nullable: false),
                    Final = table.Column<int>(type: "int", nullable: false),
                    DiscountPercent = table.Column<int>(type: "int", nullable: false),
                    InitialFormatted = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FinalFormatted = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Price", x => x.AppDataId);
                    table.ForeignKey(
                        name: "FK_Price_AppData_AppDataId",
                        column: x => x.AppDataId,
                        principalTable: "AppData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Recommendations",
                columns: table => new
                {
                    AppDataId = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recommendations", x => x.AppDataId);
                    table.ForeignKey(
                        name: "FK_Recommendations_AppData_AppDataId",
                        column: x => x.AppDataId,
                        principalTable: "AppData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReleaseDate",
                columns: table => new
                {
                    AppDataId = table.Column<int>(type: "int", nullable: false),
                    ComingSoon = table.Column<bool>(type: "bit", nullable: false),
                    Date = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReleaseDate", x => x.AppDataId);
                    table.ForeignKey(
                        name: "FK_ReleaseDate_AppData_AppDataId",
                        column: x => x.AppDataId,
                        principalTable: "AppData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Screenshot",
                columns: table => new
                {
                    ScreenshotId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<int>(type: "int", nullable: false),
                    PathThumbnail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PathFull = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppDataId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Screenshot", x => x.ScreenshotId);
                    table.ForeignKey(
                        name: "FK_Screenshot_AppData_AppDataId",
                        column: x => x.AppDataId,
                        principalTable: "AppData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SupportInfo",
                columns: table => new
                {
                    AppDataId = table.Column<int>(type: "int", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportInfo", x => x.AppDataId);
                    table.ForeignKey(
                        name: "FK_SupportInfo_AppData_AppDataId",
                        column: x => x.AppDataId,
                        principalTable: "AppData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppUserConnections",
                columns: table => new
                {
                    AppUserProfileId = table.Column<int>(type: "int", nullable: false),
                    SteamConnected = table.Column<bool>(type: "bit", nullable: false),
                    DiscordConnected = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserConnections", x => x.AppUserProfileId);
                    table.ForeignKey(
                        name: "FK_AppUserConnections_AppUserProfile_AppUserProfileId",
                        column: x => x.AppUserProfileId,
                        principalTable: "AppUserProfile",
                        principalColumn: "AppUserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppUserData",
                columns: table => new
                {
                    AppUserProfileId = table.Column<int>(type: "int", nullable: false),
                    Upvotes = table.Column<int>(type: "int", nullable: false),
                    Downvotes = table.Column<int>(type: "int", nullable: false),
                    Followers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Following = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserFavoriteGames = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FinishedLobbies = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserData", x => x.AppUserProfileId);
                    table.ForeignKey(
                        name: "FK_AppUserData_AppUserProfile_AppUserProfileId",
                        column: x => x.AppUserProfileId,
                        principalTable: "AppUserProfile",
                        principalColumn: "AppUserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppUserPhoto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublicId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppUserProfileId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserPhoto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppUserPhoto_AppUserProfile_AppUserProfileId",
                        column: x => x.AppUserProfileId,
                        principalTable: "AppUserProfile",
                        principalColumn: "AppUserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Highlighted",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AchievementsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Highlighted", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Highlighted_Achievements_AchievementsId",
                        column: x => x.AchievementsId,
                        principalTable: "Achievements",
                        principalColumn: "AppDataId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Mp4",
                columns: table => new
                {
                    MovyId = table.Column<int>(type: "int", nullable: false),
                    Resolution480 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Max = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mp4", x => x.MovyId);
                    table.ForeignKey(
                        name: "FK_Mp4_Movy_MovyId",
                        column: x => x.MovyId,
                        principalTable: "Movy",
                        principalColumn: "MovyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Webm",
                columns: table => new
                {
                    MovyId = table.Column<int>(type: "int", nullable: false),
                    Resolution480 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Max = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Webm", x => x.MovyId);
                    table.ForeignKey(
                        name: "FK_Webm_Movy_MovyId",
                        column: x => x.MovyId,
                        principalTable: "Movy",
                        principalColumn: "MovyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sub",
                columns: table => new
                {
                    SubId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PackageId = table.Column<int>(type: "int", nullable: false),
                    PercentSavingsText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PercentSavings = table.Column<int>(type: "int", nullable: false),
                    OptionText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OptionDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CanGetFreeLicense = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsFreeLicense = table.Column<bool>(type: "bit", nullable: false),
                    PriceInCentsWithDiscount = table.Column<int>(type: "int", nullable: false),
                    PackageGroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sub", x => x.SubId);
                    table.ForeignKey(
                        name: "FK_Sub_PackageGroup_PackageGroupId",
                        column: x => x.PackageGroupId,
                        principalTable: "PackageGroup",
                        principalColumn: "PackageGroupId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DiscordProfile",
                columns: table => new
                {
                    AppUserConnectionsId = table.Column<int>(type: "int", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccessToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Expires = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordProfile", x => x.AppUserConnectionsId);
                    table.ForeignKey(
                        name: "FK_DiscordProfile_AppUserConnections_AppUserConnectionsId",
                        column: x => x.AppUserConnectionsId,
                        principalTable: "AppUserConnections",
                        principalColumn: "AppUserProfileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SteamProfile",
                columns: table => new
                {
                    AppUserConnectionsId = table.Column<int>(type: "int", nullable: false),
                    SteamId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SteamProfile", x => x.AppUserConnectionsId);
                    table.ForeignKey(
                        name: "FK_SteamProfile_AppUserConnections_AppUserConnectionsId",
                        column: x => x.AppUserConnectionsId,
                        principalTable: "AppUserConnections",
                        principalColumn: "AppUserProfileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLog_ActivityId",
                table: "ActivityLog",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLog_AppUserId",
                table: "ActivityLog",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppData_AppInfoId",
                table: "AppData",
                column: "AppInfoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppListInfo_AppListId",
                table: "AppListInfo",
                column: "AppListId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserPhoto_AppUserProfileId",
                table: "AppUserPhoto",
                column: "AppUserProfileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppUserProfile_CountryIsoId",
                table: "AppUserProfile",
                column: "CountryIsoId");

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
                name: "IX_Category_AppDataId",
                table: "Category",
                column: "AppDataId");

            migrationBuilder.CreateIndex(
                name: "IX_Genre_AppDataId",
                table: "Genre",
                column: "AppDataId");

            migrationBuilder.CreateIndex(
                name: "IX_Highlighted_AchievementsId",
                table: "Highlighted",
                column: "AchievementsId");

            migrationBuilder.CreateIndex(
                name: "IX_Lobby_LobbyRequirementId",
                table: "Lobby",
                column: "LobbyRequirementId");

            migrationBuilder.CreateIndex(
                name: "IX_Lobby_LogId",
                table: "Lobby",
                column: "LogId");

            migrationBuilder.CreateIndex(
                name: "IX_Movy_Id",
                table: "Movy",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PackageGroup_AppDataId",
                table: "PackageGroup",
                column: "AppDataId");

            migrationBuilder.CreateIndex(
                name: "IX_Screenshot_AppDataId",
                table: "Screenshot",
                column: "AppDataId");

            migrationBuilder.CreateIndex(
                name: "IX_Sub_PackageGroupId",
                table: "Sub",
                column: "PackageGroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityLog");

            migrationBuilder.DropTable(
                name: "AppListInfo");

            migrationBuilder.DropTable(
                name: "AppUserData");

            migrationBuilder.DropTable(
                name: "AppUserPhoto");

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
                name: "Category");

            migrationBuilder.DropTable(
                name: "ContentDescriptors");

            migrationBuilder.DropTable(
                name: "DiscordProfile");

            migrationBuilder.DropTable(
                name: "Genre");

            migrationBuilder.DropTable(
                name: "Highlighted");

            migrationBuilder.DropTable(
                name: "LinuxRequirements");

            migrationBuilder.DropTable(
                name: "Lobby");

            migrationBuilder.DropTable(
                name: "MacRequirements");

            migrationBuilder.DropTable(
                name: "Metacritic");

            migrationBuilder.DropTable(
                name: "Mp4");

            migrationBuilder.DropTable(
                name: "PcRequirements");

            migrationBuilder.DropTable(
                name: "Platforms");

            migrationBuilder.DropTable(
                name: "Price");

            migrationBuilder.DropTable(
                name: "Recommendations");

            migrationBuilder.DropTable(
                name: "ReleaseDate");

            migrationBuilder.DropTable(
                name: "Screenshot");

            migrationBuilder.DropTable(
                name: "SteamProfile");

            migrationBuilder.DropTable(
                name: "Sub");

            migrationBuilder.DropTable(
                name: "SupportInfo");

            migrationBuilder.DropTable(
                name: "Webm");

            migrationBuilder.DropTable(
                name: "Activity");

            migrationBuilder.DropTable(
                name: "AppList");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Achievements");

            migrationBuilder.DropTable(
                name: "Log");

            migrationBuilder.DropTable(
                name: "Requirement");

            migrationBuilder.DropTable(
                name: "AppUserConnections");

            migrationBuilder.DropTable(
                name: "PackageGroup");

            migrationBuilder.DropTable(
                name: "Movy");

            migrationBuilder.DropTable(
                name: "AppUserProfile");

            migrationBuilder.DropTable(
                name: "AppData");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "CountryIso");

            migrationBuilder.DropTable(
                name: "AppInfo");
        }
    }
}
