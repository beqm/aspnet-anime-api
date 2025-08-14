using Domain.Models;

namespace Domain.Interfaces;

public interface IAnimeRepository
{
    Task AddAsync(Anime anime);
    Task<Anime?> GetByIdAsync(int id);
    Task<Anime?> GetByTitleAsync(string title);
    Task<Anime?> GetByAuthorAsync(string author);
    Task<IEnumerable<Anime>> GetRangeAsync(int start, int end);
    Task UpdateAsync(Anime anime);
    Task DeleteByIdAsync(int id);
}