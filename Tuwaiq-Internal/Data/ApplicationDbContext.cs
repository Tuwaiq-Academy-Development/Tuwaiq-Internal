using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace TuwaiqRecruitment.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Company> Companies { get; set; } = null!;
    public DbSet<Candidate> Candidates { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Company>().ToTable("Company");
        modelBuilder.Entity<Company>().HasMany(c => c.Candidates).WithOne(c => c.Company)
            .HasForeignKey(c => c.CompanyId);
        modelBuilder.Entity<Company>().Property(p => p.Id).ValueGeneratedOnAdd();
        modelBuilder.Entity<Company>().Property(p => p.Name).IsRequired();
        modelBuilder.Entity<Company>().Property(p => p.CreatedAt).IsRequired();
        modelBuilder.Entity<Company>().Property(p => p.Logo).IsRequired();
        modelBuilder.Entity<Company>().Property(p => p.Users).IsRequired(false)
            .HasConversion(
                v => string.Join(",", v), // Convert List<string> to string
                v => v.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(Guid.Parse).ToList() // Convert string to List<string>
            );

        //     .HasConversion(
        //     new ValueConverter<List<string>, string>(
        //         v => string.Join(",", v),
        //         v => v.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList())
        // ).Metadata.SetValueComparer(new ValueComparer<List<string>>(
        //     (c1, c2) => c1.SequenceEqual(c2),
        //     c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
        //     c => c.ToList()
        // ));

        modelBuilder.Entity<Candidate>().ToTable("Candidate");
        modelBuilder.Entity<Candidate>().Property(p => p.Id).ValueGeneratedOnAdd();
        modelBuilder.Entity<Candidate>().Property(p => p.NationalId).IsRequired();
        modelBuilder.Entity<Candidate>().Property(p => p.Notes).IsRequired(false);
        modelBuilder.Entity<Candidate>().Property(p => p.Tags).IsRequired(false).HasConversion(
            new ValueConverter<List<string>, string>(
                v => string.Join(",", v),
                v => v.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList())
        ).Metadata.SetValueComparer(new ValueComparer<List<string>>(
            (c1, c2) => c1.SequenceEqual(c2),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c.ToList()
        ));
        modelBuilder.Entity<Candidate>().Property(p => p.IsSelected).IsRequired();
        modelBuilder.Entity<Candidate>().Property(p => p.CreatedAt).IsRequired();
        modelBuilder.Entity<Candidate>().Property(p => p.InterviewedDate).IsRequired(false);

        modelBuilder.Entity<Category>().ToTable("Category");
        modelBuilder.Entity<Category>().Property(p => p.Id).ValueGeneratedOnAdd();
        modelBuilder.Entity<Category>().Property(p => p.Name).IsRequired();
        modelBuilder.Entity<Category>().Property(p => p.CreatedAt).IsRequired();

        base.OnModelCreating(modelBuilder);
    }
}