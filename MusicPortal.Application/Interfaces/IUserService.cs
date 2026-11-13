using MusicPortal.Application.DTO;

namespace MusicPortal.Application.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDTO>> GetAllUsers();
    Task<UserDTO?> GetUser(int id);
    Task<UserDTO?> Authenticate(string username, string password);
    Task CreateUser(RegisterDTO user);
    Task UpdateUser(UserDTO user);
    Task DeleteUser(int id);
}
