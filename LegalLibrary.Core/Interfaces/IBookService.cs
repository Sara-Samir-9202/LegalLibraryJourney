using LegalLibrary.Core.DTOs.Books;

namespace LegalLibrary.Core.Interfaces;

public interface IBookService
{
    Task<PagedResult<BookDto>> GetAllAsync(BookFilterDto filter);
    Task<BookDto?> GetByIdAsync(int id);
    Task<BookDto> CreateAsync(CreateBookDto dto);
    Task<BookDto?> UpdateAsync(int id, UpdateBookDto dto);
    Task<bool> DeleteAsync(int id);
    Task<bool> IncrementDownloadAsync(int id, string userId, string ip);
    Task<bool> IncrementOnlineReadAsync(int id);
    Task<DashboardStatsDto> GetDashboardStatsAsync();
}