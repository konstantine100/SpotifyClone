using SpotifyClone.CORE;
using SpotifyClone.DTOs;
using SpotifyClone.Requests;

namespace SpotifyClone.Services.Interfaces;

public interface IPlaylistService
{
    ApiResponse<PlaylistDTO> AddPlaylist(Guid? userId, Guid? artistId, AddPlaylist request);
    ApiResponse<SongDTO> AddPlaylistSong(Guid? userId, Guid? artistId, Guid playlistId, Guid songId);
    ApiResponse<PlaylistDTO> CombinePlaylist(Guid userId, Guid firstPlaylistId, Guid secondPlaylistId);
    ApiResponse<PlaylistDTO> IntersecedPlaylist(Guid userId, Guid firstPlaylistId, Guid secondPlaylistId);
    ApiResponse<List<PlaylistDTO>> GetAllPlaylists();
    ApiResponse<List<PlaylistDTO>> GetPlaylistsFinder(string? search, string? sortBy);
    ApiResponse<List<PlaylistDTO>> GetArtistPlaylists(Guid? artistId);
    ApiResponse<List<PlaylistDTO>> GetUserPlaylists(Guid? userId);
    ApiResponse<SongDTO> PlayPlaylistSong(Guid playlistId, Guid? songId);
    ApiResponse<PlaylistDTO> GetPlaylist(Guid playlistId);
    ApiResponse<PlaylistDTO> ChangePlaylist(Guid? userId, Guid? artistId, Guid playlistId, string changeParametr, string changeTo);
    ApiResponse<PlaylistDTO> DeletePlaylist(Guid? userId, Guid? artistId, Guid playlistId);
    ApiResponse<SongDTO> DeletePlaylistSong(Guid? userId, Guid? artistId, Guid playlistId, Guid songId);
    
    
}