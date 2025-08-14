using Domain.Models;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Application.Common.Exceptions;

namespace Infrastructure.Persistence;

public class AnimeRepository : IAnimeRepository
{
    private readonly DatabaseContext _dbContext;

    public AnimeRepository(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Anime anime)
    {
        bool exists = await _dbContext.Animes
                            .AnyAsync(a => a.Title == anime.Title);

        if (exists)
            throw new ConflictException($"Anime with title '{anime.Title}' already exists.");

        _dbContext.Animes.Add(anime);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Anime?> GetByIdAsync(int id)
    {
        return await _dbContext.Animes.AsNoTracking().FirstOrDefaultAsync(t => t.ID == id);
    }

    public async Task<IEnumerable<Anime>> GetRangeAsync(int start, int end)
    {
        return await _dbContext.Animes
            .AsNoTracking()
            .Skip((start - 1) * end)
            .Take(end)
            .ToListAsync();
    }

    public async Task UpdateAsync(Anime anime)
    {
        _dbContext.Animes.Update(anime);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteByIdAsync(int id)
    {
        var result = await _dbContext
            .Animes
            .Where(t => t.ID == id)
            .ExecuteDeleteAsync();
        if (result == 0)
        {
            throw new KeyNotFoundException($"Task with ID {id} not found.");
        }
    }
}