using System.Security.Claims;
using LegalLibrary.Core.DTOs.Books;
using LegalLibrary.Core.Helpers;
using LegalLibrary.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LegalLibrary.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _service;
    public BooksController(IBookService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<BookDto>>>> GetAll(
        [FromQuery] BookFilterDto filter)
    {
        if (!User.IsInRole("Admin")) filter.IsPublished = true;
        var result = await _service.GetAllAsync(filter);
        return Ok(ApiResponse<PagedResult<BookDto>>.Ok(result));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<BookDto>>> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result == null) return NotFound(ApiResponse<BookDto>.Fail("غير موجود"));
        return Ok(ApiResponse<BookDto>.Ok(result));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<BookDto>>> Create(CreateBookDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id },
            ApiResponse<BookDto>.Ok(result, "تم الإضافة"));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<BookDto>>> Update(int id, UpdateBookDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        if (result == null) return NotFound(ApiResponse<BookDto>.Fail("غير موجود"));
        return Ok(ApiResponse<BookDto>.Ok(result, "تم التحديث"));
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        if (!result) return NotFound(ApiResponse<bool>.Fail("غير موجود"));
        return Ok(ApiResponse<bool>.Ok(true, "تم الحذف"));
    }

    [HttpPost("{id}/download")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<bool>>> RecordDownload(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "";
        var result = await _service.IncrementDownloadAsync(id, userId, ip);
        return Ok(ApiResponse<bool>.Ok(result));
    }

    [HttpPost("{id}/read")]
    public async Task<ActionResult<ApiResponse<bool>>> RecordRead(int id)
        => Ok(ApiResponse<bool>.Ok(await _service.IncrementOnlineReadAsync(id)));

    [HttpGet("dashboard/stats")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<DashboardStatsDto>>> GetStats()
        => Ok(ApiResponse<DashboardStatsDto>.Ok(await _service.GetDashboardStatsAsync()));
}