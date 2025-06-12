using SpotifyClone.CORE;
using SpotifyClone.DTOs;
using SpotifyClone.Enums;
using SpotifyClone.Requests;

namespace SpotifyClone.Services.Interfaces;

public interface ISongService
{
    ApiResponse<SongDTO> PostSong(Guid artistId, Guid albumId, AddSong request);
    ApiResponse<GenreDTO> PostSongGenre(Guid artistId, Guid albumId, Guid songId, GENRE request);
    ApiResponse<ArtistDTO> PostSongFeatureArtist(Guid artistId, Guid albumId, Guid songId, Guid ftArtistId);
    ApiResponse<SongComposersDTO> PostSongComposers(Guid artistId, Guid albumId, Guid songId, Guid composerId, SONG_CONTRIBUTOR request);
    ApiResponse<List<SongDTO>> GetAllSongs();
    ApiResponse<List<SongDTO>> GetSongFinder(string? search, string? sortBy, string? filterBy, GENRE? genre);
    ApiResponse<List<SongDTO>> GetArtistSongs(Guid artistId);
    ApiResponse<List<SongDTO>> GetAlbumSongs(Guid albumId);
    ApiResponse<SongDTO> GetSongById(Guid songId);
    ApiResponse<SongDTO> PlaySong(Guid songId);
    
    ApiResponse<List<ArtistDTO>> GetFeaturingArtists(Guid songId);
    ApiResponse<List<SongComposersDTO>> GetSongComposers(Guid songId);
    ApiResponse<SongDTO> ChangeSong(Guid artistId, Guid albumId, Guid songId, string changeParametr, string changeTo);
    ApiResponse<SongDTO> DeleteSong(Guid artistId, Guid albumId, Guid songId);
    ApiResponse<ArtistDTO> DeleteSongFeatureArtist(Guid artistId, Guid albumId, Guid songId, Guid ftArtistId);
    ApiResponse<ArtistDTO> DeletYourselfAsFeatureArtist(Guid ftArtistId, Guid songId);
    ApiResponse<GenreDTO> DeleteSongGenre(Guid artistId, Guid albumId, Guid songId, Guid genreId);  
    ApiResponse<ArtistDTO> DeleteSongComposer(Guid artistId, Guid albumId, Guid songId, Guid composerId);  
    ApiResponse<ArtistDTO> DeleteYourselfAsComposer(Guid artistId, Guid composerId, Guid songId);  

}