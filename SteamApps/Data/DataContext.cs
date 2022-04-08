using System.Text.Json;
using SteamApps.Entities.SteamApp;
using SteamApps.Entities.SteamApp.Information;
using SteamApps.Entities.SteamApps;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace SteamApps.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<AppList> AppList { get; set; }

        public DbSet<AppInfo> AppInfo { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            /*********** Steam API **************/
            builder.Entity<AppListInfo>()
                .HasOne(app => app.AppList)
                .WithMany(list => list.Apps)
                .HasForeignKey(app => app.AppListId)
                .IsRequired();

            builder.Entity<AppListInfo>()
                .HasKey(app => app.AppListInfoId);

            /*********** Steam Store **************/

            builder.Entity<AppData>()
                .HasOne(app => app.AppInfo)
                .WithOne(gameinfo => gameinfo.Data)
                .HasForeignKey<AppData>(app => app.AppInfoId)
                .IsRequired();

            builder.Entity<AppData>()
                .HasKey(app => app.Id);

            builder.Entity<AppData>()
                .Property(app => app.Dlc)
                .HasConversion(
                    dlc => JsonSerializer.Serialize(dlc, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }),
                    dlc => JsonSerializer.Deserialize<List<int>>(dlc, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }),
                    new ValueComparer<ICollection<int>>(
                        (c1, c2) => c1.SequenceEqual(c2),
                        c => c.Aggregate(0, (a, dlc) => HashCode.Combine(a, dlc.GetHashCode())),
                        c => (ICollection<int>)c.ToList()));

            builder.Entity<AppData>()
                .Property(app => app.Developers)
                .HasConversion(
                    developers => JsonSerializer.Serialize(developers, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }),
                    developers => JsonSerializer.Deserialize<List<string>>(developers, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }),
                    new ValueComparer<ICollection<string>>(
                        (c1, c2) => c1.SequenceEqual(c2),
                        c => c.Aggregate(0, (a, developers) => HashCode.Combine(a, developers.GetHashCode())),
                        c => (ICollection<string>)c.ToList()));

            builder.Entity<AppData>()
                .Property(app => app.Publishers)
                .HasConversion(
                    publishers => JsonSerializer.Serialize(publishers, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }),
                    publishers => JsonSerializer.Deserialize<List<string>>(publishers, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }),
                    new ValueComparer<ICollection<string>>(
                        (c1, c2) => c1.SequenceEqual(c2),
                        c => c.Aggregate(0, (a, publishers) => HashCode.Combine(a, publishers.GetHashCode())),
                        c => (ICollection<string>)c.ToList()));

            builder.Entity<ReleaseDate>()
                .HasOne(release => release.AppData)
                .WithOne(app => app.ReleaseDate)
                .HasForeignKey<ReleaseDate>(release => release.AppDataId)
                .IsRequired();

            builder.Entity<ReleaseDate>()
                .HasKey(release => release.AppDataId);

            builder.Entity<Screenshot>()
                .HasOne(screenshot => screenshot.AppData)
                .WithMany(app => app.Screenshots)
                .HasForeignKey(screenshot => screenshot.AppDataId)
                .IsRequired();

            builder.Entity<Screenshot>()
                .HasKey(screenshot => screenshot.ScreenshotId);

            builder.Entity<PcRequirements>()
                .HasOne(pc => pc.AppData)
                .WithOne(appdata => appdata.PcRequirements)
                .HasForeignKey<PcRequirements>(pc => pc.AppDataId)
                .IsRequired();

            builder.Entity<PcRequirements>()
                .HasKey(pc => pc.AppDataId);

            builder.Entity<MacRequirements>()
                .HasOne(mac => mac.AppData)
                .WithOne(appdata => appdata.MacRequirements)
                .HasForeignKey<MacRequirements>(mac => mac.AppDataId)
                .IsRequired();

            builder.Entity<MacRequirements>()
                .HasKey(mac => mac.AppDataId);

            builder.Entity<LinuxRequirements>()
                .HasOne(linux => linux.AppData)
                .WithOne(appdata => appdata.LinuxRequirements)
                .HasForeignKey<LinuxRequirements>(linux => linux.AppDataId)
                .IsRequired();

            builder.Entity<LinuxRequirements>()
                .HasKey(linux => linux.AppDataId);

            builder.Entity<SupportInfo>()
                .HasOne(support => support.AppData)
                .WithOne(app => app.SupportInfo)
                .HasForeignKey<SupportInfo>(support => support.AppDataId)
                .IsRequired();

            builder.Entity<SupportInfo>()
                .HasKey(support => support.AppDataId);

            builder.Entity<Metacritic>()
                .HasOne(meta => meta.AppData)
                .WithOne(app => app.Metacritic)
                .HasForeignKey<Metacritic>(meta => meta.AppDataId)
                .IsRequired();

            builder.Entity<Metacritic>()
                .HasKey(meta => meta.AppDataId);

            builder.Entity<Genre>()
                .HasOne(genre => genre.AppData)
                .WithMany(app => app.Genres)
                .HasForeignKey(app => app.AppDataId)
                .IsRequired();

            builder.Entity<Genre>()
                .HasKey(genre => genre.GenreId);

            builder.Entity<ContentDescriptors>()
                .HasOne(contentDescriptor => contentDescriptor.AppData)
                .WithOne(app => app.ContentDescriptors)
                .HasForeignKey<ContentDescriptors>(contentDescriptor => contentDescriptor.AppDataId)
                .IsRequired();

            builder.Entity<ContentDescriptors>()
                .Property(content => content.Ids)
                .HasConversion(
                    content => JsonSerializer.Serialize(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }),
                    content => JsonSerializer.Deserialize<List<string>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }),
                    new ValueComparer<ICollection<string>>(
                        (c1, c2) => c1.SequenceEqual(c2),
                        c => c.Aggregate(0, (a, content) => HashCode.Combine(a, content.GetHashCode())),
                        c => (ICollection<string>)c.ToList()));

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
                .HasKey(highlighted => highlighted.Id);

            builder.Entity<Achievements>()
                .HasOne(achievements => achievements.AppData)
                .WithOne(app => app.Achievements)
                .HasForeignKey<Achievements>(achievements => achievements.AppDataId)
                .IsRequired();

            builder.Entity<Achievements>()
                .HasKey(achievements => achievements.AppDataId);

            builder.Entity<Platforms>()
                .HasOne(platforms => platforms.AppData)
                .WithOne(app => app.Platforms)
                .HasForeignKey<Platforms>(platforms => platforms.AppDataId)
                .IsRequired();

            builder.Entity<Platforms>()
                .HasKey(platforms => platforms.AppDataId);

            builder.Entity<Mp4>()
                .HasOne(mp4 => mp4.Movy)
                .WithOne(movy => movy.Mp4)
                .HasForeignKey<Mp4>(mp4 => mp4.MovyId)
                .IsRequired();

            builder.Entity<Mp4>()
                .HasKey(mp4 => mp4.MovyId);

            builder.Entity<Webm>()
                .HasOne(webm => webm.Movy)
                .WithOne(movy => movy.Webm)
                .HasForeignKey<Webm>(webm => webm.MovyId)
                .IsRequired();

            builder.Entity<Webm>()
                .HasKey(webm => webm.MovyId);

            builder.Entity<Movy>()
                .HasOne(movy => movy.AppData)
                .WithMany(app => app.Movies)
                .HasForeignKey(app => app.Id)
                .IsRequired();

            builder.Entity<Movy>()
                .HasKey(movy => movy.MovyId);

            builder.Entity<PackageGroup>()
                .HasOne(package => package.AppData)
                .WithMany(app => app.PackageGroups)
                .HasForeignKey(package => package.AppDataId)
                .IsRequired();

            builder.Entity<PackageGroup>()
                .HasKey(package => package.PackageGroupId);

            builder.Entity<Recommendations>()
                .HasOne(rec => rec.AppData)
                .WithOne(app => app.Recommendations)
                .HasForeignKey<Recommendations>(rec => rec.AppDataId)
                .IsRequired();

            builder.Entity<Recommendations>()
                .HasKey(rec => rec.AppDataId);

            builder.Entity<Price>()
                .HasOne(price => price.AppData)
                .WithOne(app => app.PriceOverview)
                .HasForeignKey<Price>(price => price.AppDataId)
                .IsRequired();

            builder.Entity<Price>()
                .HasKey(price => price.AppDataId);

            builder.Entity<Sub>()
                .HasOne(sub => sub.PackageGroup)
                .WithMany(package => package.Subs)
                .HasForeignKey(sub => sub.PackageGroupId)
                .IsRequired();

            builder.Entity<Sub>()
                .HasKey(sub => sub.SubId);
        }
    }
}