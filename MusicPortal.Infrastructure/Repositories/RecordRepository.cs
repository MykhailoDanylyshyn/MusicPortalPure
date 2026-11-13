using Microsoft.EntityFrameworkCore;
using MusicPortal.Infrastructure.Persistence;
using MusicPortal.Domain.Entities;
using MusicPortal.Application.Interfaces;

namespace MusicPortal.Infrastructure.Repositories;

public class RecordRepository(MusicPortalContext db) : IRepository<Record>
{
    public async Task<IEnumerable<Record>> GetAll()
    {
        return await db.Records
            .Include(r => r.Artist)
            .Include(r => r.Genre)
            .ToListAsync();
    }

    public async Task<Record?> Get(int id)
    {
        return await db.Records
            .Include(r => r.Artist)
            .Include(r => r.Genre)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Record?> Get(string name)
    {
        return await db.Records
            .Include(r => r.Artist)
            .Include(r => r.Genre)
            .FirstOrDefaultAsync(r => r.Title == name);
    }

    public async Task Create(Record record)
    {
        await db.Records.AddAsync(record);
    }

    public void Update(Record record)
    {
        db.Entry(record).State = EntityState.Modified;
    }

    public async Task Delete(int id)
    {
        Record? record = await db.Records
            .Include(r => r.PlaylistRecords)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (record != null)
        {
            db.PlaylistRecords.RemoveRange(record.PlaylistRecords);
            db.Records.Remove(record);
        }
    }
}