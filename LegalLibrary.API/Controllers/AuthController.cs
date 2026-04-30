using LegalLibrary.Core.DTOs.Auth;
using LegalLibrary.Core.Helpers;
using LegalLibrary.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LegalLibrary.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;
    public AuthController(IAuthService auth) => _auth = auth;

    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Register(RegisterDto dto)
    {
        try
        {
            var result = await _auth.RegisterAsync(dto);
            return Ok(ApiResponse<AuthResponseDto>.Ok(result, "تم التسجيل بنجاح"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<AuthResponseDto>.Fail(ex.Message));
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Login(LoginDto dto)
    {
        try
        {
            var result = await _auth.LoginAsync(dto);
            return Ok(ApiResponse<AuthResponseDto>.Ok(result, "تم تسجيل الدخول بنجاح"));
        }
        catch (InvalidOperationException ex)
        {
            return Unauthorized(ApiResponse<AuthResponseDto>.Fail(ex.Message));
        }
    }
}