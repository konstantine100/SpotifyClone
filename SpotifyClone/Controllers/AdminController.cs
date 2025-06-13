using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpotifyClone.Enums;
using SpotifyClone.Services.Interfaces;

namespace SpotifyClone.Controllers;

[Route("api/[controller]")]
[ApiController]

public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpPut("change-role")]
    public ActionResult ChangeRole(Guid userId, ROLES role)
    {
        var admin = _adminService.ChangeRole(userId, role);
        return Ok(admin);
    }
    
    [HttpPut("admin-verify-artist")]
    public ActionResult AdminArtistVerify(Guid artistId)
    {
        var admin = _adminService.AdminArtistVerify(artistId);
        return Ok(admin);
    }
}