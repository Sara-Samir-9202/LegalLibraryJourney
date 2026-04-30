using LegalLibrary.Core.DTOs.Categories;
using LegalLibrary.Core.Helpers;
using LegalLibrary.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LegalLibrary.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _service;
    public CategoriesController(ICategoryService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<CategoryDto>>>> GetAll()
        => Ok(ApiResponse<List<CategoryDto>>.Ok(await _service.GetAllAsync()));

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<CategoryDto>>> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result == null) return NotFound(ApiResponse<CategoryDto>.Fail("غير موجود"));
        return Ok(ApiResponse<CategoryDto>.Ok(result));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<CategoryDto>>> Create(CreateCategoryDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id },
            ApiResponse<CategoryDto>.Ok(result, "تم الإنشاء"));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<CategoryDto>>> Update(int id, UpdateCategoryDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        if (result == null) return NotFound(ApiResponse<CategoryDto>.Fail("غير موجود"));
        return Ok(ApiResponse<CategoryDto>.Ok(result, "تم التحديث"));
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        if (!result) return NotFound(ApiResponse<bool>.Fail("غير موجود"));
        return Ok(ApiResponse<bool>.Ok(true, "تم الحذف"));
    }
}