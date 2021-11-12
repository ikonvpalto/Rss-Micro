using Microsoft.EntityFrameworkCore;
using Sender.API.Models;

namespace Sender.API.Database
{
    public sealed class SenderDbContext : DbContext
    {
        public DbSet<ReceiverEmailModel> Emails { get; set; }

        public SenderDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReceiverEmailModel>(builder =>
            {
                builder.ToTable("Emails");
                builder.HasKey(s => s.Guid).HasName("PK_Email");
                builder.HasIndex(s => s.GroupGuid).IsUnique(false).HasDatabaseName("IDX_Email_GroupGuid");

                builder.Property(s => s.Guid).HasColumnName("Guid").IsRequired().ValueGeneratedOnAdd();
                builder.Property(s => s.GroupGuid).HasColumnName("GroupGuid").IsRequired().ValueGeneratedNever();
                builder.Property(s => s.Email).HasColumnName("Email").IsRequired().HasColumnType("varchar(1000)").HasMaxLength(1000);
            });
        }
    }
}
