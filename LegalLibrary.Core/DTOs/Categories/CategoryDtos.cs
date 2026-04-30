using System.ComponentModel.DataAnnotations;

namespace LegalLibrary.Core.DTOs.Categories;

public class CategoryDto
{
    public int Id { get; set; }
    public string NameAr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string LawField { get; set; } = string.Empty;
    public int BooksCount { get; set; }
    public bool IsActive { get; set; }
}

public class CreateCategoryDto
{
    [Required] public string NameAr { get; set; } = string.Empty;
    [Required] public string NameEn { get; set; } = string.Empty;
    public string? Description { get; set; }
    [Required] public string LawField { get; set; } = string.Empty;
}

public class UpdateCategoryDto
{
    public string? NameAr { get; set; }
    public string? NameEn { get; set; }
    public string? Description { get; set; }
    public string? LawField { get; set; }
    public bool? IsActive { get; set; }
}