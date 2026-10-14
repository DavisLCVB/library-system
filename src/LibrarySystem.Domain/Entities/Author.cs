namespace LibrarySystem.Domain.Entities;

public class Author
{
    public int Id { get; set; }
    public required string FullName { get; set; }
    public string? Nationality { get; set; }
    public ICollection<Book> Books { get; set; } = [];
}
