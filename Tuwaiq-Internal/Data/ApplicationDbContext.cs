using Microsoft.EntityFrameworkCore;
using TuwaiqInternal.Data.Converters;

namespace TuwaiqInternal.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<ToBeChecked> ToBeCheckeds { get; set; } = null!;
    public DbSet<ChecksHistory> ChecksHistories { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ToBeChecked>().ToTable("ToBeChecked");
         modelBuilder.Entity<ChecksHistory>().ToTable("ChecksHistory");
        modelBuilder.Entity<ToBeChecked>().HasKey(x => x.NationalId);
        modelBuilder.Entity<ChecksHistory>().HasKey(x => x.Id);
        modelBuilder.Entity<ChecksHistory>().Property(s => s.IdentitiesList).HasConversion<ArrayConverter>();
        base.OnModelCreating(modelBuilder);
    }
}