namespace LegalLibrary.Core.Entities;

public class BookDownload
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int BookId { get; set; }
    public DateTime DownloadedAt { get; set; } = DateTime.UtcNow;
    public string? IpAddress { get; set; }
    public ApplicationUser User { get; set; } = null!;
    public Book Book { get; set; } = null!;
}