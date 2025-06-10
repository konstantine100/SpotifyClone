namespace SpotifyClone.DTOs;

public class AlbumDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string AlbumCover { get; set; }
    public int ReleaseYear { get; set; }
    public List<GenreDTO> Genres { get; set; } 
    public TimeSpan AlbumLength { get; set; } 
    public int SongCount { get; set; }
    public int OverallPlayed { get; set; }
    public List<SongDTO> Songs { get; set; }
}