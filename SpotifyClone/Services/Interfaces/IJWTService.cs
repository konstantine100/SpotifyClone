using SpotifyClone.CORE;
using SpotifyClone.Models;

namespace SpotifyClone.Services.Interfaces;

public interface IJWTService
{
    UserToken GetUserToken(User user);
    
}