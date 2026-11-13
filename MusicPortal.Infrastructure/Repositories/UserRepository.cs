using Microsoft.EntityFrameworkCore;
using MusicPortal.Domain.Entities;
using MusicPortal.Infrastructure.Persistence;
using MusicPortal.Application.Interfaces;

namespace MusicPortal.Infrastructure.Repositories;

public class UserRepository(MusicPortalContext db) : IRepository<User>
{
    public async Task<IEnumerable<User>> GetAll()
    {
        return await db.Users.ToListAsync();
    }

    public async Task<User?> Get(int id)
    {
        return await db.Users
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> Get(string name)
    {
        return await db.Users
            .FirstOrDefaultAsync(u => u.Username == name);
    }

    public async Task Create(User user) =>
        await db.Users.AddAsync(user);

    public void Update(User user) =>
        db.Entry(user).State = EntityState.Modified;

    public async Task Delete(int id)
    {
        User? user = await db.Users.FindAsync(id);

        if (user != null)
        {
            db.Users.Remove(user);
        }
    }
}