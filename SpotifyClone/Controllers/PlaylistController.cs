using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpotifyClone.Requests;
using SpotifyClone.Services.Interfaces;

namespace SpotifyClone.Controllers;

[Route("api/[controller]")]
[ApiController]

public class PlaylistController : ControllerBase
{
    private readonly IPlaylistService _playlistService;

    public PlaylistController(IPlaylistService playlistService)
    {
        _playlistService = playlistService;
    }

    [HttpPost("post-playlist")]
    public ActionResult AddPlaylist(Guid? userId, Guid? artistId, AddPlaylist request)
    {
        var playlist = _playlistService.AddPlaylist(userId, artistId, request);
        return Ok(playlist);
    }
    
    [HttpPost("post-playlist-song")]
    public ActionResult AddPlaylistSong(Guid? userId, Guid? artistId, Guid playlistId, Guid songId)
    {
        var playlist = _playlistService.AddPlaylistSong(userId, artistId, playlistId, songId);
        return Ok(playlist);
    }
    
    [HttpPost("combine-playlists")]
    public ActionResult CombinePlaylist(Guid userId, Guid firstPlaylistId, Guid secondPlaylistId)
    {
        var playlist = _playlistService.CombinePlaylist(userId, firstPlaylistId, secondPlaylistId);
        return Ok(playlist);
    }
    
    [HttpPost("intersected-playlists")]
    public ActionResult IntersecedPlaylist(Guid userId, Guid firstPlaylistId, Guid secondPlaylistId)
    {
        var playlist = _playlistService.IntersecedPlaylist(userId, firstPlaylistId, secondPlaylistId);
        return Ok(playlist);
    }
    
    [HttpGet("get-all-playlist")]
    public ActionResult GetAllPlaylists()
    {
        var playlist = _playlistService.GetAllPlaylists();
        return Ok(playlist);
    }
    
    [HttpGet("get-playlist-finde")]
    public ActionResult GetPlaylistsFinder(string? search, string? sortBy)
    {
        var playlist = _playlistService.GetPlaylistsFinder(search, sortBy);
        return Ok(playlist);
    }
    
    [HttpGet("get-artist-playlist")]
    public ActionResult GetArtistPlaylists(Guid? artistId)
    {
        var playlist = _playlistService.GetArtistPlaylists(artistId);
        return Ok(playlist);
    }
    
    [HttpGet("get-user-playlist")]
    public ActionResult GetUserPlaylists(Guid? userId)
    {
        var playlist = _playlistService.GetUserPlaylists(userId);
        return Ok(playlist);
    }
    
    [HttpGet("play-playlist-song")]
    public ActionResult PlayPlaylistSong(Guid playlistId, Guid? songId)
    {
        var playlist = _playlistService.PlayPlaylistSong(playlistId, songId);
        return Ok(playlist);
    }
    
    [HttpGet("get-playlist-by-id")]
    public ActionResult GetPlaylist(Guid playlistId)
    {
        var playlist = _playlistService.GetPlaylist(playlistId);
        return Ok(playlist);
    }
    
    [HttpPut("update-playlist")]
    public ActionResult ChangePlaylist(Guid? userId, Guid? artistId, Guid playlistId, string changeParametr, string changeTo)
    {
        var playlist = _playlistService.ChangePlaylist(userId, artistId, playlistId, changeParametr, changeTo);
        return Ok(playlist);
    }
    
    [HttpDelete("delete-playlist")]
    public ActionResult DeletePlaylist(Guid? userId, Guid? artistId, Guid playlistId)
    {
        var playlist = _playlistService.DeletePlaylist(userId, artistId, playlistId);
        return Ok(playlist);
    }
    
    [HttpDelete("delete-playlist-song")]
    public ActionResult DeletePlaylistSong(Guid? userId, Guid? artistId, Guid playlistId, Guid songId)
    {
        var playlist = _playlistService.DeletePlaylistSong(userId, artistId, playlistId, songId);
        return Ok(playlist);
    }
}