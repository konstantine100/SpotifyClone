using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpotifyClone.Enums;
using SpotifyClone.Requests;
using SpotifyClone.Services.Interfaces;

namespace SpotifyClone.Controllers;

[Route("api/[controller]")]
[ApiController]

public class AlbumController : ControllerBase
{
    private readonly IAlbumService _albumService;

    public AlbumController(IAlbumService albumService)
    {
        _albumService = albumService;
    }

    [HttpPost("post-album")]
    public ActionResult AddAlbum(Guid artistId, AddAlbum request)
    {
        var album = _albumService.AddAlbum(artistId, request);
        return Ok(album);
    }
    
    [HttpPost("post-album-genre")]
    public ActionResult AddAlbumGenre(Guid artistId, Guid albumId, GENRE request)
    {
        var album = _albumService.AddAlbumGenre(artistId, albumId, request);
        return Ok(album);
    }
    
    [HttpGet("get-all-albums")]
    public ActionResult GetAllAlbums()
    {
        var album = _albumService.GetAllAlbums();
        return Ok(album);
    }
    
    [HttpGet("get-album-finder")]
    public ActionResult GetAlbumsFinder(string? search, string? sortBy, GENRE? genre)
    {
        var album = _albumService.GetAlbumsFinder(search, sortBy, genre);
        return Ok(album);
    }
    
    [HttpGet("get-artist-albums")]
    public ActionResult GetArtistAlbums(Guid artistId)
    {
        var album = _albumService.GetArtistAlbums(artistId);
        return Ok(album);
    }
    
    [HttpGet("get-album-by-id")]
    public ActionResult GetAlbumById(Guid albumId)
    {
        var album = _albumService.GetAlbumById(albumId);
        return Ok(album);
    }
    
    [HttpPut("update-album")]
    public ActionResult ChangeAlbum(Guid artistId, Guid albumId, string changeParametr, string changeTo)
    {
        var album = _albumService.ChangeAlbum(artistId, albumId, changeParametr, changeTo);
        return Ok(album);
    }
    
    [HttpDelete("delete-album")]
    public ActionResult DeleteAlbum(Guid artistId, Guid albumId)
    {
        var album = _albumService.DeleteAlbum(artistId, albumId);
        return Ok(album);
    }
    
    [HttpDelete("delete-album-genre")]
    public ActionResult DeleteAlbumGenre(Guid artistId, Guid albumId, Guid genreId)
    {
        var album = _albumService.DeleteAlbumGenre(artistId, albumId, genreId);
        return Ok(album);
    }
}