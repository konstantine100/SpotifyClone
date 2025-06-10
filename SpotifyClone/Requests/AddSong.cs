namespace SpotifyClone.Requests;

public class AddSong
{
    public string Title { get; set; }
    public TimeSpan Duration { get; set; }
    public string Lyrics { get; set; }
    public int TrackNumber { get; set; }
    public bool IsExplicit { get; set; }
}