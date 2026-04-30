using Microsoft.AspNetCore.Http;

namespace LegalLibrary.Core.Interfaces;

public interface IFileService
{
    Task<string> UploadPdfAsync(IFormFile file);
    Task<string> UploadImageAsync(IFormFile file);
    Task<bool> DeleteFileAsync(string filePath);
    string GetFileUrl(string filePath, HttpRequest request);
}