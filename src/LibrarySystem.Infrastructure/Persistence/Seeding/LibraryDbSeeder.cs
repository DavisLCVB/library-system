using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Infrastructure.Persistence.Seeding;

public static class LibraryDbSeeder
{
    public static void Seed(DbContext context, bool _)
    {
        if (context.Set<Category>().Any())
        {
            return;
        }
        context.Set<Book>().AddRange(CreateSampleBooks());
        context.SaveChanges();
    }

    public static async Task SeedAsync(
        DbContext context,
        bool _,
        CancellationToken cancellationToken
    )
    {
        if (await context.Set<Category>().AnyAsync(cancellationToken))
        {
            return;
        }
        context.Set<Book>().AddRange(CreateSampleBooks());
        await context.SaveChangesAsync(cancellationToken);
    }

    private static List<Book> CreateSampleBooks()
    {
        var software = new Category { Name = "Software Engineering" };
        var math = new Category { Name = "Mathematics" };

        var martin = new Author { FullName = "Robert C. Martin", Nationality = "American" };
        var fowler = new Author { FullName = "Martin Fowler", Nationality = "British" };
        var beck = new Author { FullName = "Kent Beck", Nationality = "American" };
        var knuth = new Author { FullName = "Donald E. Knuth", Nationality = "American" };

        return
        [
            new Book
            {
                Isbn = "9780132350884",
                Title = "Clean Code",
                PublishedYear = 2008,
                Category = software,
                Authors = [martin],
                Copies =
                [
                    new BookCopy { InventoryCode = "CC-001" },
                    new BookCopy { InventoryCode = "CC-002" },
                ],
            },
            new Book
            {
                Isbn = "9780134757599",
                Title = "Refactoring",
                PublishedYear = 2018,
                Category = software,
                Authors = [fowler, beck],
                Copies = [new BookCopy { InventoryCode = "RF-001" }],
            },
            new Book
            {
                Isbn = "9780201896831",
                Title = "The Art of Computer Programming, Vol. 1",
                PublishedYear = 1997,
                Category = math,
                Authors = [knuth],
                Copies =
                [
                    new BookCopy { InventoryCode = "TAOCP1-001" },
                    new BookCopy { InventoryCode = "TAOCP1-002", Status = CopyStatus.Maintenance },
                ],
            },
        ];
    }
}
