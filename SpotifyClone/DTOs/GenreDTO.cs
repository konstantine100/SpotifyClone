using SpotifyClone.Enums;

namespace SpotifyClone.DTOs;

public class GenreDTO
{
    public Guid Id { get; set; }
    public GENRE Genres { get; set; }
}