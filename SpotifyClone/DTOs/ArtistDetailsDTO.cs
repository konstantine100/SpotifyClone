namespace SpotifyClone.DTOs;

public class ArtistDetailsDTO
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public string? Website { get; set; }
    public DateTime FormationYear { get; set; }
    public bool IsActive { get; set; }
    public int TotalAlbums { get; set; }
}