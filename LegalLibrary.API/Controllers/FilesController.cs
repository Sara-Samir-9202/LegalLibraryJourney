using LegalLibrary.Core.Helpers;
using LegalLibrary.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LegalLibrary.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class FilesController : ControllerBase
{
    private readonly IFileService _files;
    public FilesController(IFileService files) => _files = files;

    [HttpPost("upload-pdf")]
    public async Task<ActionResult<ApiResponse<object>>> UploadPdf(IFormFile file)
    {
        try
        {
            var path = await _files.UploadPdfAsync(file);
            var url = _files.GetFileUrl(path, Request);
            return Ok(ApiResponse<object>.Ok(new { path, url }, "تم رفع الملف"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<object>.Fail(ex.Message));
        }
    }

    [HttpPost("upload-cover")]
    public async Task<ActionResult<ApiResponse<object>>> UploadCover(IFormFile file)
    {
        try
        {
            var path = await _files.UploadImageAsync(file);
            var url = _files.GetFileUrl(path, Request);
            return Ok(ApiResponse<object>.Ok(new { path, url }, "تم رفع الصورة"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<object>.Fail(ex.Message));
        }
    }

    [HttpDelete]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteFile([FromQuery] string path)
    {
        var result = await _files.DeleteFileAsync(path);
        return Ok(ApiResponse<bool>.Ok(result, result ? "تم الحذف" : "الملف غير موجود"));
    }
}