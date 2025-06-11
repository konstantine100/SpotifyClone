using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpotifyClone.Requests;
using SpotifyClone.Services.Interfaces;

namespace SpotifyClone.Controllers;

[Route("api/[controller]")]
[ApiController]

public class ArtistDetailsController : ControllerBase
{
    private readonly IArtistDetailsServices _artistDetailsServices;

    public ArtistDetailsController(IArtistDetailsServices artistDetailsServices)
    {
        _artistDetailsServices = artistDetailsServices;
    }

    [HttpPost("post-artist-details")]
    public ActionResult PostArtistDetails(Guid artistId, AddArtistDetails request)
    {
        var artistDetails = _artistDetailsServices.PostArtistDetails(artistId, request);
        return Ok(artistDetails);
    }
    
    [HttpGet("get-artist-details")]
    public ActionResult GetArtistDetails(Guid artistId)
    {
        var artistDetails = _artistDetailsServices.GetArtistDetails(artistId);
        return Ok(artistDetails);
    }
    
    [HttpPut("update-artist-details")]
    public ActionResult ChangeArtistDetails(Guid artistDetailsId, string changeParametr, string changeTo)
    {
        var artistDetails = _artistDetailsServices.ChangeArtistDetails(artistDetailsId, changeParametr, changeTo);
        return Ok(artistDetails);
    }
    
    [HttpDelete("delete-artist-details")]
    public ActionResult DeleteArtistDetails(Guid artistId)
    {
        var artistDetails = _artistDetailsServices.DeleteArtistDetails(artistId);
        return Ok(artistDetails);
    }
}