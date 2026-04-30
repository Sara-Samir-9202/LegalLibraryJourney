namespace LegalLibrary.Core.Entities;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? TitleAr { get; set; }
    public string? Description { get; set; }
    public string? Author { get; set; }
    public string? Professor { get; set; }
    public string AcademicLevel { get; set; } = string.Empty;
    public string PdfPath { get; set; } = string.Empty;
    public string? CoverImagePath { get; set; }
    public int DownloadCount { get; set; } = 0;
    public int OnlineReadCount { get; set; } = 0;
    public bool IsPublished { get; set; } = false;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public ICollection<BookDownload> Downloads { get; set; } = new List<BookDownload>();
}