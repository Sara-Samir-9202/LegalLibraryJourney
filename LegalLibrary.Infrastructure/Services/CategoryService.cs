using LegalLibrary.Core.DTOs.Categories;
using LegalLibrary.Core.Entities;
using LegalLibrary.Core.Interfaces;
using LegalLibrary.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LegalLibrary.Infrastructure.Services;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _db;
    public CategoryService(AppDbContext db) => _db = db;

    public async Task<List<CategoryDto>> GetAllAsync() =>
        await _db.Categories
            .Where(c => c.IsActive)
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                NameAr = c.NameAr,
                NameEn = c.NameEn,
                Description = c.Description,
                LawField = c.LawField,
                BooksCount = c.Books.Count(b => b.IsActive && b.IsPublished),
                IsActive = c.IsActive
            }).ToListAsync();

    public async Task<CategoryDto?> GetByIdAsync(int id)
    {
        var c = await _db.Categories.Include(x => x.Books)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (c == null) return null;
        return new CategoryDto
        {
            Id = c.Id,
            NameAr = c.NameAr,
            NameEn = c.NameEn,
            Description = c.Description,
            LawField = c.LawField,
            BooksCount = c.Books.Count(b => b.IsActive && b.IsPublished),
            IsActive = c.IsActive
        };
    }

    public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto)
    {
        var cat = new Category
        {
            NameAr = dto.NameAr,
            NameEn = dto.NameEn,
            Description = dto.Description,
            LawField = dto.LawField
        };
        _db.Categories.Add(cat);
        await _db.SaveChangesAsync();
        return (await GetByIdAsync(cat.Id))!;
    }

    public async Task<CategoryDto?> UpdateAsync(int id, UpdateCategoryDto dto)
    {
        var cat = await _db.Categories.FindAsync(id);
        if (cat == null) return null;

        if (dto.NameAr != null) cat.NameAr = dto.NameAr;
        if (dto.NameEn != null) cat.NameEn = dto.NameEn;
        if (dto.Description != null) cat.Description = dto.Description;
        if (dto.LawField != null) cat.LawField = dto.LawField;
        if (dto.IsActive.HasValue) cat.IsActive = dto.IsActive.Value;

        await _db.SaveChangesAsync();
        return await GetByIdAsync(id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var cat = await _db.Categories.FindAsync(id);
        if (cat == null) return false;
        cat.IsActive = false;
        await _db.SaveChangesAsync();
        return true;
    }
}