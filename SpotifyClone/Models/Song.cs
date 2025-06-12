namespace SpotifyClone.Models;

public class Song
{
    public Guid Id { get; set; }
    public Guid AlbumId { get; set; }
    public Album Album { get; set; }
    public string Title { get; set; }
    public TimeSpan Duration { get; set; }
    public string SongUrl { get; set; }
    public string Lyrics { get; set; }
    public int TrackNumber { get; set; }
    public int TimesPlayed { get; set; } = 0;
    public bool IsExplicit { get; set; }

    public List<Artist> FeaturingArtist { get; set; } = new List<Artist>();
    public List<SongComposers> SongComposers { get; set; } = new List<SongComposers>();
    public List<Genre> Genres { get; set; } = new List<Genre>();
    public List<Playlist> Playlists { get; set; } = new List<Playlist>();
}