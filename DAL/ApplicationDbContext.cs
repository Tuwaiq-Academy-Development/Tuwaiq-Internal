using DAL.Converters;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<CheckList> CheckList { get; set; } = null!;
    public DbSet<CheckRequest> CheckRequests { get; set; } = null!;
    public DbSet<CheckLog> CheckLogs { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CheckList>().ToTable("CheckList");
        // modelBuilder.Entity<CheckList>().Property(s => s.CheckTypes).HasConversion<CheckTypesConverter>();
        modelBuilder.Entity<CheckList>().HasKey(x => x.Id);

        modelBuilder.Entity<CheckRequest>().ToTable("CheckRequests");
        modelBuilder.Entity<CheckRequest>().HasKey(x => x.Id);
        modelBuilder.Entity<CheckRequest>().Property(s => s.IdentitiesList).HasConversion<ArrayConverter>();
        
        modelBuilder.Entity<CheckLog>().ToTable("CheckLogs");
        modelBuilder.Entity<CheckLog>().HasKey(x => x.Id);
        base.OnModelCreating(modelBuilder);
    }
}