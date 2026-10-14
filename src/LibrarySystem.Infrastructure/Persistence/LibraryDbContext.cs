using LibrarySystem.Application.Common.Persistence;
using LibrarySystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Infrastructure.Persistence;

public class LibraryDbContext(DbContextOptions<LibraryDbContext> options)
    : DbContext(options),
        IUnitOfWork
{
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<BookCopy> BookCopies => Set<BookCopy>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasCollation(
            "case_insensitive",
            locale: "und-u-ks-level2",
            provider: "icu",
            deterministic: false
        );
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LibraryDbContext).Assembly);
    }
}
