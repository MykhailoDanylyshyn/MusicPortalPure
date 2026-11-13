using MusicPortal.Application.DTO;

namespace MusicPortal.Application.Interfaces;

public interface IGenreService
{
    Task<IEnumerable<GenreDTO>> GetAllGenres();

    Task<GenreDTO?> GetGenre(int id);

    Task CreateGenre(GenreDTO genre);

    Task UpdateGenre(GenreDTO genre);

    Task DeleteGenre(int id);
}
