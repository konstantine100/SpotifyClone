namespace SpotifyClone.DTOs;

public class SongDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public TimeSpan Duration { get; set; }
    public string Lyrics { get; set; }
    public string SongUrl { get; set; }
    public int TrackNumber { get; set; }
    public int TimesPlayed { get; set; } = 0;
    public bool IsExplicit { get; set; }
    public List<ArtistDTO> FeaturingArtist { get; set; }
    public List<SongComposersDTO> SongComposers { get; set; }
    public List<GenreDTO> Genres { get; set; }
}