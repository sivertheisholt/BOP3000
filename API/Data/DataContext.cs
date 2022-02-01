using API.Entities.GameRoom;
using API.Entities.Roles;
using API.Entities.SteamApp;
using API.Entities.SteamApp.Information;
using API.Entities.Users;
using API.Entities.Users.Role;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : IdentityDbContext<AppUser, AppRole, int,
    IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>,
    IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<GameRoom> GameRoom { get; set; }
        public DbSet<GameInfo> GameInfo { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            /*********** User/Role **************/

            builder.Entity<AppUser>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            builder.Entity<AppRole>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            /*********** STEAM **************/

            builder.Entity<AppData>()
                .HasOne(app => app.GameInfo)
                .WithOne(gameinfo => gameinfo.AppData)
                .HasForeignKey<GameInfo>(gameinfo => gameinfo.Id)
                .IsRequired();

            builder.Entity<ReleaseDate>()
                .HasOne(release => release.AppData)
                .WithOne(app => app.Release_date)
                .HasForeignKey<AppData>(app => app.Id)
                .IsRequired();

            builder.Entity<ReleaseDate>()
                .HasKey(release => release.AppDataId);

            builder.Entity<Screenshot>()
                .HasOne(screenshot => screenshot.AppData)
                .WithMany(app => app.Screenshots)
                .HasForeignKey(screenshot => screenshot.AppDataId)
                .IsRequired();

            builder.Entity<Screenshot>()
                .HasKey(screenshot => new { screenshot.AppDataId, screenshot.Id });

            builder.Entity<PcRequirements>()
                .HasOne(pc => pc.AppData)
                .WithOne(appdata => appdata.Pc_requirements)
                .HasForeignKey<AppData>(app => app.Id)
                .IsRequired();

            builder.Entity<PcRequirements>()
                .HasKey(pc => pc.AppDataId);

            builder.Entity<MacRequirements>()
                .HasOne(mac => mac.AppData)
                .WithOne(appdata => appdata.Mac_requirements)
                .HasForeignKey<AppData>(app => app.Id)
                .IsRequired();

            builder.Entity<MacRequirements>()
                .HasKey(mac => mac.AppDataId);

            builder.Entity<LinuxRequirements>()
                .HasOne(linux => linux.AppData)
                .WithOne(appdata => appdata.Linux_requirements)
                .HasForeignKey<AppData>(app => app.Id)
                .IsRequired();

            builder.Entity<LinuxRequirements>()
                .HasKey(linux => linux.AppDataId);

            builder.Entity<SupportInfo>()
                .HasOne(support => support.AppData)
                .WithOne(app => app.Support_info)
                .HasForeignKey<AppData>(app => app.Id)
                .IsRequired();

            builder.Entity<SupportInfo>()
                .HasKey(support => support.AppDataId);

            builder.Entity<Metacritic>()
                .HasOne(meta => meta.AppData)
                .WithOne(app => app.Metacritic)
                .HasForeignKey<AppData>(app => app.Id)
                .IsRequired();

            builder.Entity<Metacritic>()
                .HasKey(meta => meta.AppDataId);

            builder.Entity<Genre>()
                .HasOne(genre => genre.AppData)
                .WithMany(app => app.Genres)
                .HasForeignKey(app => app.AppDataId)
                .IsRequired();

            builder.Entity<Genre>()
                .HasKey(genre => new { genre.AppDataId, genre.Id });

            builder.Entity<ContentDescriptors>()
                .HasOne(contentDescriptor => contentDescriptor.AppData)
                .WithOne(app => app.Content_descriptors)
                .HasForeignKey<AppData>(app => app.Id)
                .IsRequired();

            builder.Entity<ContentDescriptors>()
                .HasKey(contentDescriptor => contentDescriptor.AppDataId);

            builder.Entity<Category>()
                .HasOne(category => category.AppData)
                .WithMany(app => app.Categories)
                .HasForeignKey(app => app.AppDataId)
                .IsRequired();

            builder.Entity<Category>()
                .HasKey(category => new { category.Id, category.AppDataId });

            builder.Entity<Highlighted>()
                .HasOne(high => high.Achievements)
                .WithMany(achievements => achievements.Highlighted)
                .HasForeignKey(achievements => achievements.AchievementsId)
                .IsRequired();

            builder.Entity<Highlighted>()
                .HasKey(highlighted => new { highlighted.AchievementsId, highlighted.Id });

            builder.Entity<Achievements>()
                .HasOne(achievements => achievements.AppData)
                .WithOne(app => app.Achievements)
                .HasForeignKey<AppData>(app => app.Id)
                .IsRequired();

            builder.Entity<Achievements>()
                .HasKey(achievements => achievements.AppDataId);

            builder.Entity<Platforms>()
                .HasOne(platforms => platforms.AppData)
                .WithOne(app => app.Platforms)
                .HasForeignKey<AppData>(app => app.Id)
                .IsRequired();

            builder.Entity<Platforms>()
                .HasKey(platforms => platforms.AppDataId);

            builder.Entity<Mp4>()
                .HasOne(mp4 => mp4.Movy)
                .WithOne(movy => movy.Mp4)
                .HasForeignKey<Movy>(movy => movy.Id)
                .IsRequired();

            builder.Entity<Mp4>()
                .HasKey(mp4 => new { mp4.Id });

            builder.Entity<Webm>()
                .HasOne(webm => webm.Movy)
                .WithOne(movy => movy.Webm)
                .HasForeignKey<Movy>(movy => movy.Id)
                .IsRequired();

            builder.Entity<Webm>()
                .HasKey(webm => new { webm.Id });

            builder.Entity<Movy>()
                .HasOne(movy => movy.AppData)
                .WithMany(app => app.Movies)
                .HasForeignKey(app => app.Id)
                .IsRequired();

            builder.Entity<Movy>()
                .HasKey(movy => new { movy.Id, movy.AppDataId });

            /*

            builder.Entity<Movy>()
                .HasOne(movy => movy.Mp4)
                .WithOne(mp4 => mp4.Movy)
                .IsRequired();

            builder.Entity<Movy>()
                .HasOne(movy => movy.Webm)
                .WithOne(webm => webm.Movy)
                .IsRequired();

            */


            builder.Entity<Dlc>()
                .HasOne(dlc => dlc.AppData)
                .WithMany(app => app.Dlc)
                .IsRequired();

            builder.Entity<Dlc>()
                .HasKey(dlc => new { dlc.AppDataId, dlc.Id });

            builder.Entity<Publisher>()
                .HasOne(dlc => dlc.AppData)
                .WithMany(app => app.Publishers)
                .IsRequired();

            builder.Entity<Publisher>()
                .HasKey(dlc => new { dlc.AppDataId, dlc.Id });

            builder.Entity<PackageGroup>()
                .HasOne(dlc => dlc.AppData)
                .WithMany(app => app.Package_groups)
                .IsRequired();

            builder.Entity<PackageGroup>()
                .HasKey(dlc => new { dlc.AppDataId, dlc.Id });

            builder.Entity<Developer>()
                .HasOne(dlc => dlc.AppData)
                .WithMany(app => app.Developers)
                .IsRequired();

            builder.Entity<Developer>()
                .HasKey(dlc => new { dlc.AppDataId, dlc.Id });

            builder.Entity<ContentDescriptorId>()
                .HasOne(contentId => contentId.ContentDescriptors)
                .WithMany(contentDescriptor => contentDescriptor.Ids)
                .IsRequired();

            builder.Entity<ContentDescriptorId>()
                .HasKey(contentId => new { contentId.Id, contentId.ContentDescriptorsId });
        }
    }
}