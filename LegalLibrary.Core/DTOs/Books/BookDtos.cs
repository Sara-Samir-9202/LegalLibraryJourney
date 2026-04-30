using System.ComponentModel.DataAnnotations;

namespace LegalLibrary.Core.DTOs.Books;

public class PagedResult<T>
{
    public List<T> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}

public class BookDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? TitleAr { get; set; }
    public string? Description { get; set; }
    public string? Author { get; set; }
    public string? Professor { get; set; }
    public string AcademicLevel { get; set; } = string.Empty;
    public string? CoverImageUrl { get; set; }
    public string PdfUrl { get; set; } = string.Empty;
    public int DownloadCount { get; set; }
    public int OnlineReadCount { get; set; }
    public bool IsPublished { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string LawField { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CreateBookDto
{
    [Required] public string Title { get; set; } = string.Empty;
    public string? TitleAr { get; set; }
    public string? Description { get; set; }
    public string? Author { get; set; }
    public string? Professor { get; set; }
    [Required] public string AcademicLevel { get; set; } = string.Empty;
    [Required] public int CategoryId { get; set; }
    public bool IsPublished { get; set; } = false;
    [Required] public string PdfPath { get; set; } = string.Empty;
    public string? CoverImagePath { get; set; }
}

public class UpdateBookDto
{
    public string? Title { get; set; }
    public string? TitleAr { get; set; }
    public string? Description { get; set; }
    public string? Author { get; set; }
    public string? Professor { get; set; }
    public string? AcademicLevel { get; set; }
    public int? CategoryId { get; set; }
    public bool? IsPublished { get; set; }
    public bool? IsActive { get; set; }
    public string? PdfPath { get; set; }
    public string? CoverImagePath { get; set; }
}

public class BookFilterDto
{
    public string? Search { get; set; }
    public int? CategoryId { get; set; }
    public string? LawField { get; set; }
    public string? AcademicLevel { get; set; }
    public bool? IsPublished { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 12;
}

public class DashboardStatsDto
{
    public int TotalBooks { get; set; }
    public int TotalCategories { get; set; }
    public int TotalDownloads { get; set; }
    public int TotalOnlineReads { get; set; }
    public int RecentUploads { get; set; }
    public int TotalUsers { get; set; }
}