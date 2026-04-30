using LegalLibrary.Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace LegalLibrary.Infrastructure.Services;

public class FileService : IFileService
{
    private readonly string _uploadsRoot;
    private readonly string[] _allowedPdfTypes = ["application/pdf"];
    private readonly string[] _allowedImageTypes = ["image/jpeg", "image/png", "image/webp"];
    private const long MaxPdfSize = 50 * 1024 * 1024;
    private const long MaxImageSize = 5 * 1024 * 1024;

    public FileService()
    {
        _uploadsRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
        Directory.CreateDirectory(Path.Combine(_uploadsRoot, "pdfs"));
        Directory.CreateDirectory(Path.Combine(_uploadsRoot, "covers"));
    }

    public async Task<string> UploadPdfAsync(IFormFile file)
    {
        if (file.Length > MaxPdfSize)
            throw new InvalidOperationException("حجم الملف يتجاوز 50 MB");
        if (!_allowedPdfTypes.Contains(file.ContentType))
            throw new InvalidOperationException("يجب أن يكون الملف PDF");

        var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
        var relativePath = Path.Combine("uploads", "pdfs", fileName);
        var fullPath = Path.Combine(_uploadsRoot, "pdfs", fileName);

        using var stream = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(stream);
        return relativePath.Replace("\\", "/");
    }

    public async Task<string> UploadImageAsync(IFormFile file)
    {
        if (file.Length > MaxImageSize)
            throw new InvalidOperationException("حجم الصورة يتجاوز 5 MB");
        if (!_allowedImageTypes.Contains(file.ContentType))
            throw new InvalidOperationException("يجب أن تكون الصورة JPG أو PNG أو WEBP");

        var ext = Path.GetExtension(file.FileName);
        var fileName = $"{Guid.NewGuid()}{ext}";
        var relativePath = Path.Combine("uploads", "covers", fileName);
        var fullPath = Path.Combine(_uploadsRoot, "covers", fileName);

        using var stream = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(stream);
        return relativePath.Replace("\\", "/");
    }

    public Task<bool> DeleteFileAsync(string filePath)
    {
        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filePath);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }

    public string GetFileUrl(string filePath, HttpRequest request)
        => $"{request.Scheme}://{request.Host}/{filePath}";
}