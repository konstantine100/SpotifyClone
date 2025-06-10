namespace SpotifyClone.Models;

public class Album
{
    public Guid Id { get; set; }
    public Guid ArtistId { get; set; }
    public Artist Artist { get; set; }
    
    public string Title { get; set; }
    public string AlbumCover { get; set; }
    public int ReleaseYear { get; set; }
    public List<Genre> Genres { get; set; } =  new List<Genre>();
    public TimeSpan AlbumLength { get; set; } = TimeSpan.Zero;
    public int SongCount { get; set; } = 0;
    public int OverallPlayed { get; set; } = 0;

    public List<Song> Songs { get; set; } = new List<Song>();
}