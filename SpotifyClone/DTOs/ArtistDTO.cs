using SpotifyClone.Enums;

namespace SpotifyClone.DTOs;

public class ArtistDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string ProfilePicture { get; set; }
    public COUNTRY Country { get; set; }
    public List<GenreDTO> Genres { get; set; } 
    public bool IsVerified { get; set; }
    
    public List<AlbumDTO> Albums { get; set; }
    public List<PlaylistDTO> Playlists { get; set; }
}