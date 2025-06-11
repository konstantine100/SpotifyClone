using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SpotifyClone.CORE;
using SpotifyClone.Data;
using SpotifyClone.DTOs;
using SpotifyClone.Models;
using SpotifyClone.Requests;
using SpotifyClone.Services.Interfaces;
using SpotifyClone.Validation;

namespace SpotifyClone.Services.Implenetation;

public class ArtistService : IArtistService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    
    public ArtistService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public ApiResponse<ArtistDTO> PostArtist(Guid userId, AddArtist request)
    {
        var user = _context.Users
            .Include(x => x.Artist)
            .FirstOrDefault(x => x.Id == userId);

        if (user == null)
        {
            var response = new ApiResponse<ArtistDTO>
            {
                Data = null,
                Message = "user not found",
                Status = StatusCodes.Status404NotFound
            };
            return response;
        }
        else
        {
            if (user.Artist != null)
            {
                var response = new ApiResponse<ArtistDTO>
                {
                    Data = null,
                    Message = "artist already exists",
                    Status = StatusCodes.Status403Forbidden
                };
                return response;
            }
            else
            {
                var artist = _mapper.Map<Artist>(request);
                var validator = new ArtistValidator();
                var result = validator.Validate(artist);

                if (!result.IsValid)
                {
                    var response = new ApiResponse<ArtistDTO>
                    {
                        Data = null,
                        Message = "wrong user information",
                        Status = StatusCodes.Status403Forbidden
                    };
                    return response;
                }
                else
                {
                    artist.UserId = user.Id;
                    user.Artist = artist;
                    _context.SaveChanges();
                
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

    public ApiResponse<List<ArtistDTO>> GetArtists()
    {
        var artists = _context.Artists
            .OrderByDescending(x => x.Listens)
            .ToList();
        
        var response = new ApiResponse<List<ArtistDTO>>
        {
            Data = _mapper.Map<List<ArtistDTO>>(artists),
            Message = null,
            Status = StatusCodes.Status200OK
        };
        return response;
    }

    public ApiResponse<List<AlbumDTO>> GetArtistFinder(string? search, string? sortBy, string? filterBy)
    {
        throw new NotImplementedException();
    }

    public ApiResponse<ArtistDTO> GetArtist(Guid artistId)
    {
        throw new NotImplementedException();
    }

    public ApiResponse<ArtistDTO> ChangeArtist(Guid artistId, string changeParametr, string changeTo)
    {
        throw new NotImplementedException();
    }

    public ApiResponse<ArtistDTO> DeleteArtist(Guid artistId)
    {
        throw new NotImplementedException();
    }
}