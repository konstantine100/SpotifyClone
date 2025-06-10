using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SpotifyClone.CORE;
using SpotifyClone.Models;
using SpotifyClone.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace SpotifyClone.Services.Implenetation;

public class JWTService : IJWTService
{
    public UserToken GetUserToken(User user)
    {
        var JwtKey = "d1c3a98008b83d67cdc372f02a3cd82d52c8f8035a3c1747de848f05690e4e8833fee65e699c56e289bf21ed4488961c9b422b367b1a1e72cbbb88a0299f85ff487909f15a92c7ae7be8e4827322a5041fa124fd710c168b294fbe0bbf3a22660eb58841c1a3a178e6d4ee0b5405aed5ff6881aeedffd97730c976d62d442c59ed4d5bc7227200d9770e0f047d7dbadcc83ab19a1ddfa396e2b239fcde9df77f6a3aca3ea2ecb66abd1b03e40c5c030556c7793c2d746da7a26441c4eb8e1d904b6fac87f095bfec285f4f3bdf8e6d1e63e9514d7129132f910a1481bafa55beec7a1d51269a0d7fd9065686ead9616a2351445c48b9ced5868b8c1d902d15f1";
        var JwtIssuer = "Spotify";
        var JwtAudience = "SpotifyUser";
        var JwtDuration = 300; // wutebshi anu 5 saati jamshi
        
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtKey));
        
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email.ToString()),
            new Claim( ClaimTypes.Role, user.Role.ToString())

        };

        var token = new JwtSecurityToken(
            issuer: JwtIssuer,
            audience: JwtAudience,
            expires: DateTime.Now.AddMinutes(JwtDuration),
            claims: claims,
            signingCredentials: credentials
        );

        return new UserToken
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token)
        };

    }
}