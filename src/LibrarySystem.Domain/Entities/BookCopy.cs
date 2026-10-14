using LibrarySystem.Domain.Enums;

namespace LibrarySystem.Domain.Entities;

public class BookCopy
{
    public int Id { get; set; }
    public required string InventoryCode { get; set; }
    public CopyStatus Status { get; set; } = CopyStatus.Available;
    public int BookId { get; set; }
    public Book Book { get; set; } = null!;
}
