using SpotifyClone.Enums;

namespace SpotifyClone.Models;

public class Artist
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    
    public string Name { get; set; }
    public string ProfilePicture { get; set; }
    public COUNTRY Country { get; set; }
    public List<Genre> Genres { get; set; } = new List<Genre>();
    public bool IsVerified { get; set; } = false;

    public ArtistDetails Details { get; set; }
    public int Listens { get; set; } = 0;
    public List<Album> Albums { get; set; } = new List<Album>();
    public List<SongComposers> ArtistComposedSongs { get; set; } = new List<SongComposers>();
    public List<Playlist> Playlists { get; set; } = new List<Playlist>();
}