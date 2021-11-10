using Filter.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Filter.API.Database
{
    public sealed class FilterDbContext : DbContext
    {
        public DbSet<FilterModel> Filters { get; set; }

        public FilterDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FilterModel>(builder =>
            {
                builder.ToTable("Filters");
                builder.HasKey(s => s.Guid).HasName("PK_Filter");
                builder.HasIndex(s => s.GroupGuid).IsUnique(false).HasDatabaseName("IDX_Filter_GroupGuid");

                builder.Property(s => s.Guid).HasColumnName("Guid").IsRequired().ValueGeneratedOnAdd();
                builder.Property(s => s.GroupGuid).HasColumnName("GroupGuid").IsRequired().ValueGeneratedNever();
                builder.Property(s => s.Filter).HasColumnName("Filter").IsRequired().HasColumnType("varchar(1000)").HasMaxLength(1000);
            });
        }
    }
}
