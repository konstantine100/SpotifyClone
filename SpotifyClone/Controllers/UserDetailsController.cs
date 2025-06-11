using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpotifyClone.Requests;
using SpotifyClone.Services.Interfaces;

namespace SpotifyClone.Controllers;

[Route("api/[controller]")]
[ApiController]

public class UserDetailsController : ControllerBase
{
    private readonly IUserDetailsService _userDetailsService;

    public UserDetailsController(IUserDetailsService userDetailsService)
    {
        _userDetailsService = userDetailsService;
    }

    [HttpPost("post-user-details")]
    public ActionResult PostUserDetails(Guid userId, AddUserDetails request)
    {
        var user = _userDetailsService.PostUserDetails(userId, request);
        return Ok(user);
    }
    
    [HttpGet("search-user")]
    public ActionResult SearchUsers(string search)
    {
        var user = _userDetailsService.SearchUsers(search);
        return Ok(user);
    }
    
    [HttpPut("update-user-details")]
    public ActionResult ChangeUserDetails(Guid userId, string changeParametr, string changeTo)
    {
        var user = _userDetailsService.ChangeUserDetails(userId, changeParametr, changeTo);
        return Ok(user);
    }
    
    [HttpDelete("Delete-user-details")]
    public ActionResult DeleteUserDetails(Guid userId)
    {
        var user = _userDetailsService.DeleteUserDetails(userId);
        return Ok(user);
    }
}