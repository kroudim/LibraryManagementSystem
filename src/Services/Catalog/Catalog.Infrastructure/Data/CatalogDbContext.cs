using Microsoft.EntityFrameworkCore;
using Catalog.Domain.Entities;

namespace Catalog.Infrastructure.Data;

public class CatalogDbContext : DbContext
{
    public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.ISBN).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.ISBN).IsUnique();
            
            entity.HasOne(e => e.Category)
                .WithMany(c => c.Books)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
        });

        // Seed data
        var fictionCategoryId = Guid.Parse("66666666-6666-6666-6666-666666666666");
        var mysteryCategoryId = Guid.Parse("77777777-7777-7777-7777-777777777777");

        modelBuilder.Entity<Category>().HasData(
            new Category { Id = fictionCategoryId, Name = "Fiction" },
            new Category { Id = mysteryCategoryId, Name = "Mystery" }
        );

        var authorId = Guid.Parse("33333333-3333-3333-3333-333333333333");

        modelBuilder.Entity<Book>().HasData(
            new Book
            {
                Id = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                Title = "The Great Adventure",
                ISBN = "978-1234567890",
                AuthorPartyId = authorId,
                CategoryId = fictionCategoryId,
                TotalCopies = 5,
                AvailableCopies = 5,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Book
            {
                Id = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                Title = "Mystery of the Lost City",
                ISBN = "978-0987654321",
                AuthorPartyId = authorId,
                CategoryId = mysteryCategoryId,
                TotalCopies = 3,
                AvailableCopies = 3,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Book
            {
                Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                Title = "Science Fiction Tales",
                ISBN = "978-1122334455",
                AuthorPartyId = authorId,
                CategoryId = fictionCategoryId,
                TotalCopies = 10,
                AvailableCopies = 10,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        );
    }
}
