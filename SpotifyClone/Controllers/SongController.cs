using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpotifyClone.Enums;
using SpotifyClone.Requests;
using SpotifyClone.Services.Interfaces;

namespace SpotifyClone.Controllers;

[Route("api/[controller]")]
[ApiController]

public class SongController : ControllerBase
{
    private readonly ISongService _songService;

    public SongController(ISongService songService)
    {
        _songService = songService;
    }

    [HttpPost("post-song")]
    public ActionResult PostSong(Guid artistId, Guid albumId, AddSong request)
    {
        var song = _songService.PostSong(artistId, albumId, request);
        return Ok(song);
    }
    
    [HttpPost("post-song-genre")]
    public ActionResult PostSongGenre(Guid artistId, Guid albumId, Guid songId, GENRE request)
    {
        var song = _songService.PostSongGenre(artistId, albumId, songId, request);
        return Ok(song);
    }
    
    [HttpPost("post-song-featuring-artist")]
    public ActionResult PostSongFeatureArtist(Guid artistId, Guid albumId, Guid songId, Guid ftArtistId)
    {
        var song = _songService.PostSongFeatureArtist(artistId, albumId, songId, ftArtistId);
        return Ok(song);
    }
    
    [HttpPost("post-song-composer")]
    public ActionResult PostSongComposers(Guid artistId, Guid albumId, Guid songId, Guid composerId, SONG_CONTRIBUTOR request)
    {
        var song = _songService.PostSongComposers(artistId, albumId, songId, composerId, request);
        return Ok(song);
    }
    
    [HttpGet("get-all-songs")]
    public ActionResult GetAllSongs()
    {
        var song = _songService.GetAllSongs();
        return Ok(song);
    }
    
    [HttpGet("get-songs-finder")]
    public ActionResult GetSongFinder(string? search, string? sortBy, string? filterBy, GENRE? genre)
    {
        var song = _songService.GetSongFinder(search, sortBy, filterBy, genre);
        return Ok(song);
    }
    
    [HttpGet("get-artist-songs")]
    public ActionResult GetArtistSongs(Guid artistId)
    {
        var song = _songService.GetArtistSongs(artistId);
        return Ok(song);
    }
    
    [HttpGet("get-album-songs")]
    public ActionResult GetAlbumSongs(Guid albumId)
    {
        var song = _songService.GetAlbumSongs(albumId);
        return Ok(song);
    }
    
    [HttpGet("get-song-by-id")]
    public ActionResult GetSongById(Guid songId)
    {
        var song = _songService.GetSongById(songId);
        return Ok(song);
    }
    
    [HttpGet("play-song")]
    public ActionResult PlaySong(Guid songId)
    {
        var song = _songService.PlaySong(songId);
        return Ok(song);
    }
    
    [HttpGet("get-song-featuring-artists")]
    public ActionResult GetFeaturingArtists(Guid songId)
    {
        var song = _songService.GetFeaturingArtists(songId);
        return Ok(song);
    }
    
    [HttpGet("get-song-composers")]
    public ActionResult GetSongComposers(Guid songId)
    {
        var song = _songService.GetSongComposers(songId);
        return Ok(song);
    }
    
    [HttpPut("update-song")]
    public ActionResult ChangeSong(Guid artistId, Guid albumId, Guid songId, string changeParametr, string changeTo)
    {
        var song = _songService.ChangeSong(artistId, albumId, songId, changeParametr, changeTo);
        return Ok(song);
    }
    
    [HttpDelete("delete-song")]
    public ActionResult DeleteSong(Guid artistId, Guid albumId, Guid songId)
    {
        var song = _songService.DeleteSong(artistId, albumId, songId);
        return Ok(song);
    }
    
    [HttpDelete("delete-song-featuring-artist")]
    public ActionResult DeleteSongFeatureArtist(Guid artistId, Guid albumId, Guid songId, Guid ftArtistId)
    {
        var song = _songService.DeleteSongFeatureArtist(artistId, albumId, songId, ftArtistId);
        return Ok(song);
    }
    
    [HttpDelete("delete-yourself-as-featuring-artist")]
    public ActionResult DeletYourselfAsFeatureArtist(Guid ftArtistId, Guid songId)
    {
        var song = _songService.DeletYourselfAsFeatureArtist(ftArtistId, songId);
        return Ok(song);
    }
    
    [HttpDelete("delete-song-genre")]
    public ActionResult DeleteSongGenre(Guid artistId, Guid albumId, Guid songId, Guid genreId)
    {
        var song = _songService.DeleteSongGenre(artistId, albumId, songId, genreId);
        return Ok(song);
    }
    
    [HttpDelete("delete-song-composer")]
    public ActionResult DeleteSongComposer(Guid artistId, Guid albumId, Guid songId, Guid composerId)
    {
        var song = _songService.DeleteSongComposer(artistId, albumId, songId, composerId);
        return Ok(song);
    }
    
    [HttpDelete("delete-yourself-as-composer")]
    public ActionResult DeleteYourselfAsComposer(Guid artistId, Guid composerId, Guid songId)
    {
        var song = _songService.DeleteYourselfAsComposer(artistId, composerId, songId);
        return Ok(song);
    }
}