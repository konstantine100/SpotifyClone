using SpotifyClone.Enums;

namespace SpotifyClone.Models;

public class SongComposers
{
    public Guid Id { get; set; }
    public SONG_CONTRIBUTOR Position { get; set; }
    
    public Guid ArtistId { get; set; }
    public Artist Artist { get; set; }
    public Guid SongId { get; set; }
    public Song Song { get; set; }
}