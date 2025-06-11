using SpotifyClone.CORE;
using SpotifyClone.DTOs;
using SpotifyClone.Requests;

namespace SpotifyClone.Services.Interfaces;

public interface IArtistDetailsServices
{
    ApiResponse<ArtistDetailsDTO> PostArtistDetails(Guid artistId, AddArtistDetails request);
    ApiResponse<ArtistDetailsDTO> GetArtistDetails(Guid artistId);
    ApiResponse<ArtistDetailsDTO> ChangeArtistDetails(Guid artistDetailsId, string changeParametr, string changeTo);
    ApiResponse<ArtistDetailsDTO> DeleteArtistDetails(Guid artistId);
}