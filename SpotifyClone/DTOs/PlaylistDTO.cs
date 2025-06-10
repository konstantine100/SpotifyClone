namespace SpotifyClone.DTOs;

public class PlaylistDTO
{
    public string Title { get; set; }
    public string PlaylistPicture { get; set; }
    public TimeSpan AlbumLength { get; set; } 
    public int SongCount { get; set; } 
    public int OverallPlayed { get; set; } 
    public List<SongDTO> Songs { get; set; }
}