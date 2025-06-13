using SpotifyClone.CORE;
using SpotifyClone.DTOs;
using SpotifyClone.Enums;

namespace SpotifyClone.Services.Interfaces;

public interface IAdminService
{
    ApiResponse<UserDTO> ChangeRole(Guid userId, ROLES role);
    ApiResponse<ArtistDTO> AdminArtistVerify(Guid artistId);
}