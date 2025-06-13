using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SpotifyClone.CORE;
using SpotifyClone.Data;
using SpotifyClone.DTOs;
using SpotifyClone.Enums;
using SpotifyClone.Services.Interfaces;
using SpotifyClone.SMTP;

namespace SpotifyClone.Services.Implenetation;

public class AdminService : IAdminService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    
    public AdminService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }


    public ApiResponse<UserDTO> ChangeRole(Guid userId, ROLES role)
    {
        var user = _context.Users.FirstOrDefault(x => x.Id == userId);

        if (user == null)
        {
            var response = new ApiResponse<UserDTO>
            {
                Data = null,
                Message = "user not found",
                Status = StatusCodes.Status404NotFound
            };
            return response;
        }
        else
        {
            user.Role = role;
            _context.SaveChanges();

            var response = new ApiResponse<UserDTO>
            {
                Data = _mapper.Map<UserDTO>(user),
                Message = null,
                Status = StatusCodes.Status200OK
            };
            return response;
        }
    }

    public ApiResponse<ArtistDTO> AdminArtistVerify(Guid artistId)
    {
        var artist = _context.Artists
            .Include(x => x.User)
            .FirstOrDefault(x => x.Id == artistId);

        if (artist == null)
        {
            var response = new ApiResponse<ArtistDTO>
            {
                Data = null,
                Message = "artist not found",
                Status = StatusCodes.Status404NotFound
            };
            return response;
        }
        else
        {
            if (artist.IsVerified)
            {
                var response = new ApiResponse<ArtistDTO>
                {
                    Data = null,
                    Message = "artist is already verified",
                    Status = StatusCodes.Status403Forbidden
                };
                return response;
            }
            else
            {
                artist.IsVerified = true;
                _context.SaveChanges();
                
                SMTPService smtpService = new SMTPService();

                smtpService.SendEmail(artist.User.Email, "artist verivied", $"<p>dear {artist.Name} you are know verified!</p>");
                
                var response = new ApiResponse<ArtistDTO>
                {
                    Data = _mapper.Map<ArtistDTO>(artist),
                    Message = null,
                    Status = StatusCodes.Status200OK
                };
                return response;
            }
        }
    }
}