using SpotifyClone.Enums;

namespace SpotifyClone.DTOs;

public class UserDTO
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public ACCOUNT_STATUS Status { get; set; } 
    public ROLES Role { get; set; } 
    public string Email { get; set; }
    public DateTime BirthDate { get; set; }
}