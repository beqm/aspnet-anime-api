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

    public async Task<Anime?> GetByTitleAsync(string title)
    {
        return await _dbContext.Animes.AsNoTracking().FirstOrDefaultAsync(t => t.Title == title);
    }

    public async Task<List<Anime>> GetListAsync(string? title = null, string? description = null)
    {
        var query = _dbContext.Animes.AsNoTracking().AsQueryable();

        if (!string.IsNullOrEmpty(title))
            query = query.Where(a => a.Title.Contains(title));

        if (!string.IsNullOrEmpty(description))
            query = query.Where(a => a.Description.Contains(description));

        return await query.ToListAsync();
    }

    public async Task<int> UpdateAsync(Anime anime)
    {
        _dbContext.Animes.Update(anime);
        var result = await _dbContext.SaveChangesAsync();
        return result;
    }

    public async Task<int> DeleteByIdAsync(int id)
    {
        var result = await _dbContext
            .Animes
            .Where(t => t.ID == id)
            .ExecuteDeleteAsync();
        return result;
    }
}