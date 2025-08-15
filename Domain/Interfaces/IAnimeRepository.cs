using Domain.Models;

namespace Domain.Interfaces;

public interface IAnimeRepository
{
    Task AddAsync(Anime anime);
    Task<Anime?> GetByIdAsync(int id);
    Task<List<Anime>> GetListAsync(string? title = null, string? description = null);
    Task<int> UpdateAsync(Anime anime);
    Task<int> DeleteByIdAsync(int id);
}