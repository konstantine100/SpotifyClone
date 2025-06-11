using SpotifyClone.CORE;
using SpotifyClone.DTOs;
using SpotifyClone.Requests;

namespace SpotifyClone.Services.Interfaces;

public interface IArtistService
{
    ApiResponse<ArtistDTO> PostArtist(Guid userId, AddArtist request);
    ApiResponse<List<ArtistDTO>> GetArtists();
    ApiResponse<List<AlbumDTO>> GetArtistFinder(string? search, string? sortBy, string? filterBy);
    ApiResponse<ArtistDTO> GetArtist(Guid artistId);
    ApiResponse<ArtistDTO> ChangeArtist(Guid artistId, string changeParametr, string changeTo);
    ApiResponse<ArtistDTO> DeleteArtist(Guid artistId);
    
    
}