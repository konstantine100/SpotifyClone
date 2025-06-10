namespace SpotifyClone.DTOs;

public class UserDetailsDTO
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string ProfilePicture { get; set; }
    public bool SeeExplecitContent { get; set; }
}