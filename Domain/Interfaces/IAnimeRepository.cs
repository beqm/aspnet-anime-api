using Domain.Models;

namespace Domain.Interfaces;

public interface IAnimeRepository
{
    Task AddAsync(Anime anime);
    Task<Anime?> GetByIdAsync(int id);
    Task<IEnumerable<Anime>> GetRangeAsync(int start, int end);
    Task UpdateAsync(Anime anime);
    Task DeleteByIdAsync(int id);
}