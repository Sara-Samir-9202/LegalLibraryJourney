using LegalLibrary.Core.DTOs.Books;
using LegalLibrary.Core.Entities;
using LegalLibrary.Core.Interfaces;
using LegalLibrary.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LegalLibrary.Infrastructure.Services;

public class BookService : IBookService
{
    private readonly AppDbContext _db;
    public BookService(AppDbContext db) => _db = db;

    public async Task<PagedResult<BookDto>> GetAllAsync(BookFilterDto filter)
    {
        var query = _db.Books.Include(b => b.Category)
            .Where(b => b.IsActive).AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Search))
            query = query.Where(b =>
                b.Title.Contains(filter.Search) ||
                (b.TitleAr != null && b.TitleAr.Contains(filter.Search)) ||
                (b.Author != null && b.Author.Contains(filter.Search)));

        if (filter.CategoryId.HasValue)
            query = query.Where(b => b.CategoryId == filter.CategoryId.Value);
        if (!string.IsNullOrWhiteSpace(filter.LawField))
            query = query.Where(b => b.Category.LawField == filter.LawField);
        if (!string.IsNullOrWhiteSpace(filter.AcademicLevel))
            query = query.Where(b => b.AcademicLevel == filter.AcademicLevel);
        if (filter.IsPublished.HasValue)
            query = query.Where(b => b.IsPublished == filter.IsPublished.Value);

        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(b => b.CreatedAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                TitleAr = b.TitleAr,
                Description = b.Description,
                Author = b.Author,
                Professor = b.Professor,
                AcademicLevel = b.AcademicLevel,
                PdfUrl = b.PdfPath,
                CoverImageUrl = b.CoverImagePath,
                DownloadCount = b.DownloadCount,
                OnlineReadCount = b.OnlineReadCount,
                IsPublished = b.IsPublished,
                CategoryId = b.CategoryId,
                CategoryName = b.Category.NameAr,
                LawField = b.Category.LawField,
                CreatedAt = b.CreatedAt
            }).ToListAsync();

        return new PagedResult<BookDto>
        {
            Items = items,
            TotalCount = total,
            Page = filter.Page,
            PageSize = filter.PageSize
        };
    }

    public async Task<BookDto?> GetByIdAsync(int id)
    {
        var b = await _db.Books.Include(x => x.Category)
            .FirstOrDefaultAsync(x => x.Id == id && x.IsActive);
        return b == null ? null : MapToDto(b);
    }

    public async Task<BookDto> CreateAsync(CreateBookDto dto)
    {
        var book = new Book
        {
            Title = dto.Title,
            TitleAr = dto.TitleAr,
            Description = dto.Description,
            Author = dto.Author,
            Professor = dto.Professor,
            AcademicLevel = dto.AcademicLevel,
            CategoryId = dto.CategoryId,
            PdfPath = dto.PdfPath,
            CoverImagePath = dto.CoverImagePath,
            IsPublished = dto.IsPublished
        };
        _db.Books.Add(book);
        await _db.SaveChangesAsync();
        return (await GetByIdAsync(book.Id))!;
    }

    public async Task<BookDto?> UpdateAsync(int id, UpdateBookDto dto)
    {
        var book = await _db.Books.FindAsync(id);
        if (book == null) return null;

        if (dto.Title != null) book.Title = dto.Title;
        if (dto.TitleAr != null) book.TitleAr = dto.TitleAr;
        if (dto.Description != null) book.Description = dto.Description;
        if (dto.Author != null) book.Author = dto.Author;
        if (dto.Professor != null) book.Professor = dto.Professor;
        if (dto.AcademicLevel != null) book.AcademicLevel = dto.AcademicLevel;
        if (dto.CategoryId.HasValue) book.CategoryId = dto.CategoryId.Value;
        if (dto.IsPublished.HasValue) book.IsPublished = dto.IsPublished.Value;
        if (dto.IsActive.HasValue) book.IsActive = dto.IsActive.Value;
        if (dto.PdfPath != null) book.PdfPath = dto.PdfPath;
        if (dto.CoverImagePath != null) book.CoverImagePath = dto.CoverImagePath;
        book.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return await GetByIdAsync(id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var book = await _db.Books.FindAsync(id);
        if (book == null) return false;
        book.IsActive = false;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> IncrementDownloadAsync(int id, string userId, string ip)
    {
        var book = await _db.Books.FindAsync(id);
        if (book == null) return false;
        book.DownloadCount++;
        _db.BookDownloads.Add(new BookDownload
        { BookId = id, UserId = userId, IpAddress = ip });
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> IncrementOnlineReadAsync(int id)
    {
        var book = await _db.Books.FindAsync(id);
        if (book == null) return false;
        book.OnlineReadCount++;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<DashboardStatsDto> GetDashboardStatsAsync() =>
        new()
        {
            TotalBooks = await _db.Books.CountAsync(b => b.IsActive),
            TotalCategories = await _db.Categories.CountAsync(c => c.IsActive),
            TotalDownloads = await _db.Books.SumAsync(b => b.DownloadCount),
            TotalOnlineReads = await _db.Books.SumAsync(b => b.OnlineReadCount),
            RecentUploads = await _db.Books.CountAsync(b =>
                b.IsActive && b.CreatedAt >= DateTime.UtcNow.AddDays(-7)),
            TotalUsers = await _db.Users.CountAsync()
        };

    private static BookDto MapToDto(Book b) => new()
    {
        Id = b.Id,
        Title = b.Title,
        TitleAr = b.TitleAr,
        Description = b.Description,
        Author = b.Author,
        Professor = b.Professor,
        AcademicLevel = b.AcademicLevel,
        PdfUrl = b.PdfPath,
        CoverImageUrl = b.CoverImagePath,
        DownloadCount = b.DownloadCount,
        OnlineReadCount = b.OnlineReadCount,
        IsPublished = b.IsPublished,
        CategoryId = b.CategoryId,
        CategoryName = b.Category?.NameAr ?? "",
        LawField = b.Category?.LawField ?? "",
        CreatedAt = b.CreatedAt
    };
}