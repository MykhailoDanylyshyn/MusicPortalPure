using MusicPortal.Domain.Entities;

namespace MusicPortal.Application.Interfaces;

public interface IUnitOfWork
{
    IRepository<Record> Records { get; }

    IRepository<Artist> Artists { get; }

    IRepository<Genre> Genres { get; }

    IRepository<User> Users { get; }

    IRepository<Playlist> Playlists { get; }

    Task Save();
}
