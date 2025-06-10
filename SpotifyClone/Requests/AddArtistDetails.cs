namespace SpotifyClone.Requests;

public class AddArtistDetails
{
    public string Description { get; set; }
    public string? Website { get; set; }
    public DateTime FormationYear { get; set; }
    public bool IsActive { get; set; }
}