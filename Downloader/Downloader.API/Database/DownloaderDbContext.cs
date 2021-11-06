using Downloader.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Downloader.API.Database
{
    public class DownloaderDbContext : DbContext
    {
        public DbSet<RssSource> RssSources { get; set; }

        public DownloaderDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RssSource>(builder =>
            {
                builder.ToTable("RssSources");
                builder.HasKey(s => s.Guid).HasName("PK_RssSource");

                builder.Property(s => s.Guid).HasColumnName("Guid").IsRequired().ValueGeneratedNever();
                builder.Property(s => s.Url).HasColumnName("Url").HasColumnType("varchar(1000)").HasMaxLength(1000).IsRequired();
                builder.Property(s => s.LastSuccessDownloading).HasColumnName("LastSuccessDownloading").HasColumnType("timestamp").IsRequired();
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
