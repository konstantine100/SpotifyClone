using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpotifyClone.Enums;
using SpotifyClone.Requests;
using SpotifyClone.Services.Interfaces;

namespace SpotifyClone.Controllers;

[Route("api/[controller]")]
[ApiController]

public class ArtistController : ControllerBase
{
    private readonly IArtistService _artistService;

    public ArtistController(IArtistService artistService)
    {
        _artistService = artistService;
    }

    [HttpPost("post-artist")]
    public ActionResult PostArtist(Guid userId, AddArtist request)
    {
        var artist = _artistService.PostArtist(userId, request);
        return Ok(artist);
    }
    
    [HttpPost("post-artist-genre")]
    public ActionResult PostGenre(Guid artistId, GENRE genre)
    {
        var artist = _artistService.PostGenre(artistId, genre);
        return Ok(artist);
    }
    
    [HttpPost("request-artist-verification")]
    public ActionResult RequestVerified(Guid artistId)
    {
        var artist = _artistService.RequestVerified(artistId);
        return Ok(artist);
    }
    
    [HttpGet("get-artists")]
    public ActionResult GetArtists()
    {
        var artist = _artistService.GetArtists();
        return Ok(artist);
    }
    
    [HttpGet("get-artists-finder")]
    public ActionResult GetArtistFinder(string? search, string? sortBy, COUNTRY? country, GENRE? genre)
    {
        var artist = _artistService.GetArtistFinder(search, sortBy, country, genre);
        return Ok(artist);
    }
    
    [HttpGet("get-artist")]
    public ActionResult GetArtist(Guid artistId)
    {
        var artist = _artistService.GetArtist(artistId);
        return Ok(artist);
    }
    
    [HttpPut("update-artist")]
    public ActionResult ChangeArtist(Guid artistId, string changeParametr, string changeTo, COUNTRY? country)
    {
        var artist = _artistService.ChangeArtist(artistId, changeParametr, changeTo, country);
        return Ok(artist);
    }
    
    [HttpDelete("delete-artist")]
    public ActionResult DeleteArtist(Guid artistId)
    {
        var artist = _artistService.DeleteArtist(artistId);
        return Ok(artist);
    }
    
    [HttpDelete("delete-artist-genre")]
    public ActionResult DeleteGenre(Guid artistId, Guid genreId)
    {
        var artist = _artistService.DeleteGenre(artistId, genreId);
        return Ok(artist);
    }
}