using MusicPortal.Application.DTO;

namespace MusicPortal.Application.Interfaces;

public interface IPlaylistService
{
    Task<IEnumerable<PlaylistDTO>> GetAllPlaylists();

    Task<PlaylistDTO?> GetPlaylist(int id);

    Task CreatePlaylist(PlaylistDTO playlist);

    Task UpdatePlaylist(PlaylistDTO playlist);

    Task DeletePlaylist(int id);
}
