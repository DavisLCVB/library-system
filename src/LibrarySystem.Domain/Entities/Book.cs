namespace LibrarySystem.Domain.Entities;

public class Book
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Isbn { get; set; }
    public int PublishedYear { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
}
