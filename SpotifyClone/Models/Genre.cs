using SpotifyClone.Enums;

namespace SpotifyClone.Models;

public class Genre
{
    public Guid Id { get; set; }
    public Guid? ArtistId { get; set; }
    public Artist? Artist { get; set; }
    public Guid? AlbumId { get; set; }
    public Album? Album { get; set; }
    public Guid? SongId { get; set; }
    public Song? Song { get; set; }
    
    public GENRE Genres { get; set; }
}