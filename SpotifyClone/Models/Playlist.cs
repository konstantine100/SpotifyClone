namespace SpotifyClone.Models;

public class Playlist
{
    public Guid Id { get; set; }
    public Guid? UserDetailsId { get; set; }
    public UserDetails? UserDetails { get; set; }
    public Guid? ArtistId { get; set; }
    public Artist? Artist { get; set; }
    
    public string Title { get; set; }
    public string? PlaylistPicture { get; set; }
    public TimeSpan AlbumLength { get; set; } = TimeSpan.Zero;
    public int SongCount { get; set; } = 0;
    public int OverallPlayed { get; set; } = 0;
    public List<Song> Songs { get; set; } = new List<Song>();
    
    
}