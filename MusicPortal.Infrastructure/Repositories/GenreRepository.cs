using Microsoft.EntityFrameworkCore;
using MusicPortal.Infrastructure.Persistence;
using MusicPortal.Domain.Entities;
using MusicPortal.Application.Interfaces;

namespace MusicPortal.Infrastructure.Repositories;

public class GenreRepository(MusicPortalContext db) : IRepository<Genre>
{
    public async Task<IEnumerable<Genre>> GetAll()
    {
        return await db.Genres
            .Include(g => g.Records)
            .ToListAsync();
    }

    public async Task<Genre?> Get(int id)
    {
        return await db.Genres
            .Include(g => g.Records)
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<Genre?> Get(string name)
    {
        return await db.Genres
            .FirstOrDefaultAsync(g => g.Name == name);
    }

    public async Task Create(Genre genre)
    {
        await db.Genres.AddAsync(genre);
    }

    public void Update(Genre genre)
    {
        db.Entry(genre).State = EntityState.Modified;
    }

    public async Task Delete(int id)
    {
        Genre? genre = await db.Genres.FindAsync(id);

        if (genre != null)
        {
            db.Genres.Remove(genre);
        }
    }
}