using SpotifyClone.CORE;
using SpotifyClone.DTOs;
using SpotifyClone.Requests;

namespace SpotifyClone.Services.Interfaces;

public interface IUserDetailsService
{
    ApiResponse<UserDetailsDTO> PostUserDetails(Guid userId, AddUserDetails request);
    ApiResponse<List<UserDetailsDTO>> SearchUsers(string search);
    ApiResponse<UserDetailsDTO> ChangeUserDetails(Guid userId, string changeParametr, string changeTo);
    ApiResponse<UserDetailsDTO> DeleteUserDetails(Guid userId);
    
}