using SpotifyClone.CORE;
using SpotifyClone.DTOs;

namespace SpotifyClone.Services.Interfaces;

public interface IUserService
{
    ApiResponse<UserDTO> UpdateUser(Guid id, string changeParametr, string toChange);
    
    ApiResponse<UserDTO> DeleteUser(Guid id);
}