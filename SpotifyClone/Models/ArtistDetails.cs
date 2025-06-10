namespace SpotifyClone.Models;

public class ArtistDetails
{
    public Guid Id { get; set; }
    public Guid ArtistId { get; set; }
    public Artist Artist { get; set; }
    
    public string Description { get; set; }
    public string? Website { get; set; }
    public DateTime FormationYear { get; set; }
    public bool IsActive { get; set; }
    public int TotalAlbums { get; set; } = 0;

}