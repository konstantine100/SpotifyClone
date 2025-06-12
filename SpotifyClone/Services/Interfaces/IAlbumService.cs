using SpotifyClone.CORE;
using SpotifyClone.DTOs;
using SpotifyClone.Enums;
using SpotifyClone.Requests;

namespace SpotifyClone.Services.Interfaces;

public interface IAlbumService
{
    ApiResponse<AlbumDTO> AddAlbum(Guid artistId, AddAlbum request);
    ApiResponse<GenreDTO> AddAlbumGenre(Guid artistId, Guid albumId, GENRE request);
    ApiResponse<List<AlbumDTO>> GetAllAlbums();
    ApiResponse<List<AlbumDTO>> GetAlbumsFinder(string? search, string? sortBy, GENRE? genre);
    ApiResponse<List<AlbumDTO>> GetArtistAlbums(Guid artistId);
    ApiResponse<AlbumDTO> GetAlbumById(Guid albumId);
    ApiResponse<AlbumDTO> ChangeAlbum(Guid artistId, Guid albumId, string changeParametr, string changeTo);
    ApiResponse<AlbumDTO> DeleteAlbum(Guid artistId, Guid albumId);
    ApiResponse<GenreDTO> DeleteAlbumGenre(Guid artistId, Guid albumId, Guid genreId);
    
}