using LegalLibrary.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LegalLibrary.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<BookDownload> BookDownloads => Set<BookDownload>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Category>(e =>
        {
            e.Property(x => x.NameAr).HasMaxLength(200).IsRequired();
            e.Property(x => x.NameEn).HasMaxLength(200).IsRequired();
            e.Property(x => x.LawField).HasMaxLength(100);
        });

        builder.Entity<Book>(e =>
        {
            e.Property(x => x.Title).HasMaxLength(500).IsRequired();
            e.Property(x => x.PdfPath).HasMaxLength(1000).IsRequired();
            e.Property(x => x.AcademicLevel).HasMaxLength(50);
            e.HasOne(x => x.Category)
             .WithMany(c => c.Books)
             .HasForeignKey(x => x.CategoryId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<BookDownload>(e =>
        {
            e.HasOne(x => x.Book)
             .WithMany(b => b.Downloads)
             .HasForeignKey(x => x.BookId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.User)
             .WithMany(u => u.Downloads)
             .HasForeignKey(x => x.UserId)
             .OnDelete(DeleteBehavior.Cascade);
        });
    }
}