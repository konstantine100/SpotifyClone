using SpotifyClone.CORE;
using SpotifyClone.DTOs;
using SpotifyClone.Enums;
using SpotifyClone.Models;
using SpotifyClone.Requests;

namespace SpotifyClone.Services.Interfaces;

public interface IArtistService
{
    ApiResponse<ArtistDTO> PostArtist(Guid userId, AddArtist request, COUNTRY country);
    ApiResponse<GenreDTO> PostGenre(Guid artistId, GENRE genre); 
    ApiResponse<ArtistDTO> RequestVerified(Guid artistId); 
    ApiResponse<List<ArtistDTO>> GetArtists();
    ApiResponse<List<ArtistDTO>> GetArtistFinder(string? search, string? sortBy, COUNTRY? country, GENRE? genre);
    ApiResponse<ArtistDTO> GetArtist(Guid artistId);
    ApiResponse<ArtistDTO> ChangeArtist(Guid artistId, string changeParametr, string? changeTo, COUNTRY? country);
    ApiResponse<ArtistDTO> DeleteArtist(Guid artistId);
    ApiResponse<GenreDTO> DeleteGenre(Guid artistId, Guid genreId);
    
    
}