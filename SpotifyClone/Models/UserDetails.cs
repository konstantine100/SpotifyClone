namespace SpotifyClone.Models;

public class UserDetails
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    
    public string Username { get; set; }
    public string? ProfilePicture { get; set; }
    public bool SeeExplecitContent { get; set; } = false;
    public List<Playlist> Playlists { get; set; } = new List<Playlist>();
}