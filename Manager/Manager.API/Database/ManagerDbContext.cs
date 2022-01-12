using Manager.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Manager.API.Database
{
    public sealed class ManagerDbContext : DbContext
    {
        public ManagerDbContext(DbContextOptions options) : base(options) { }

        public DbSet<JobPeriodicityModel> JobPeriodicities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<JobPeriodicityModel>(builder =>
            {
                builder.ToTable("JobPeriodicities");
                builder.HasKey(s => s.Guid).HasName("PK_JobPeriodicity");

                builder.Property(s => s.Guid).HasColumnName("Guid").IsRequired().ValueGeneratedNever();
                builder.Property(s => s.Periodicity).HasColumnName("Periodicity").HasColumnType("varchar(40)").HasMaxLength(40).IsRequired();
                builder.Property(s => s.IsJobEnabled).HasColumnName("IsJobEnabled").HasColumnType("boolean").IsRequired();
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
