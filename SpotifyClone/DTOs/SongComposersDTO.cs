using SpotifyClone.Enums;

namespace SpotifyClone.DTOs;

public class SongComposersDTO
{
    public SONG_CONTRIBUTOR Position { get; set; }
    public ArtistDTO Artist { get; set; }
}