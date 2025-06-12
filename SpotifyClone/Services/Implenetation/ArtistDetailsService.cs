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

public class ArtistDetailsService : IArtistDetailsServices
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    
    public ArtistDetailsService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public ApiResponse<ArtistDetailsDTO> PostArtistDetails(Guid artistId, AddArtistDetails request)
    {
        var artist = _context.Artists
            .Include(x => x.Details)
            .FirstOrDefault(x => x.Id == artistId);

        if (artist == null)
        {
            var response = new ApiResponse<ArtistDetailsDTO>
            {
                Data = null,
                Message = "Artist not found",
                Status = StatusCodes.Status404NotFound,
            };
            return response;
        }
        else
        {
            if (artist.Details != null)
            {
                var response = new ApiResponse<ArtistDetailsDTO>
                {
                    Data = null,
                    Message = "Artist details already exists",
                    Status = StatusCodes.Status403Forbidden,
                };
                return response;
            }
            else
            {
                var artistDetails = _mapper.Map<ArtistDetails>(request);
                var validator = new ArtistDetailsValidator();
                var result = validator.Validate(artistDetails);

                if (!result.IsValid)
                {
                    var response = new ApiResponse<ArtistDetailsDTO>
                    {
                        Data = null,
                        Message = "wrong artist details info",
                        Status = StatusCodes.Status403Forbidden,
                    };
                    return response;
                }
                else
                {
                    artistDetails.ArtistId = artist.Id;
                    artist.Details = artistDetails;
                    _context.SaveChanges();
                    
                    var response = new ApiResponse<ArtistDetailsDTO>
                    {
                        Data = _mapper.Map<ArtistDetailsDTO>(artistDetails),
                        Message = null,
                        Status = StatusCodes.Status200OK,
                    };
                    return response;
                }
            }
        }
    }

    public ApiResponse<ArtistDetailsDTO> GetArtistDetails(Guid artistId)
    {
        var artist = _context.Artists
            .Include(x => x.Details)
            .FirstOrDefault(x => x.Id == artistId);
        
        if (artist == null)
        {
            var response = new ApiResponse<ArtistDetailsDTO>
            {
                Data = null,
                Message = "Artist not found",
                Status = StatusCodes.Status404NotFound,
            };
            return response;
        }
        else
        {
            if (artist.Details == null)
            {
                var response = new ApiResponse<ArtistDetailsDTO>
                {
                    Data = null,
                    Message = "artist details not found",
                    Status = StatusCodes.Status404NotFound,
                };
                return response;
            }
            else
            {
                var response = new ApiResponse<ArtistDetailsDTO>
                {
                    Data = _mapper.Map<ArtistDetailsDTO>(artist.Details),
                    Message = null,
                    Status = StatusCodes.Status200OK,
                };
                return response;
            }
        }
    }

    public ApiResponse<ArtistDetailsDTO> ChangeArtistDetails(Guid artistDetailsId, string changeParametr, string changeTo)
    {
        var artist = _context.ArtistDetails
            .FirstOrDefault(x => x.Id == artistDetailsId);

        if (artist == null)
        {
            var response = new ApiResponse<ArtistDetailsDTO>
            {
                Data = null,
                Message = "Artist not found",
                Status = StatusCodes.Status404NotFound,
            };
            return response;
        }
        else
        {
            if (changeParametr.ToLower() == "description")
            {
                artist.Description = changeTo;
                var validator = new ArtistDetailsValidator();
                var result = validator.Validate(artist);

                if (!result.IsValid)
                {
                    var response = new ApiResponse<ArtistDetailsDTO>
                    {
                        Data = null,
                        Message = "wrong artist information",
                        Status = StatusCodes.Status403Forbidden,
                    };
                    return response;
                }
                else
                {
                    _context.SaveChanges();
                    
                    var response = new ApiResponse<ArtistDetailsDTO>
                    {
                        Data = _mapper.Map<ArtistDetailsDTO>(artist),
                        Message = null,
                        Status = StatusCodes.Status200OK,
                    };
                    return response;
                }
            }
            else if (changeParametr.ToLower() == "website")
            {
                artist.Website = changeTo;
                var validator = new ArtistDetailsValidator();
                var result = validator.Validate(artist);

                if (!result.IsValid)
                {
                    var response = new ApiResponse<ArtistDetailsDTO>
                    {
                        Data = null,
                        Message = "wrong artist information",
                        Status = StatusCodes.Status403Forbidden,
                    };
                    return response;
                }
                else
                {
                    _context.SaveChanges();
                    
                    var response = new ApiResponse<ArtistDetailsDTO>
                    {
                        Data = _mapper.Map<ArtistDetailsDTO>(artist),
                        Message = null,
                        Status = StatusCodes.Status200OK,
                    };
                    return response;
                }
            }
            else if (changeParametr.ToLower() == "formationyear")
            {
                artist.FormationYear = DateTime.Parse(changeTo);
                var validator = new ArtistDetailsValidator();
                var result = validator.Validate(artist);

                if (!result.IsValid)
                {
                    var response = new ApiResponse<ArtistDetailsDTO>
                    {
                        Data = null,
                        Message = "wrong artist information",
                        Status = StatusCodes.Status403Forbidden,
                    };
                    return response;
                }
                else
                {
                    _context.SaveChanges();
                    
                    var response = new ApiResponse<ArtistDetailsDTO>
                    {
                        Data = _mapper.Map<ArtistDetailsDTO>(artist),
                        Message = null,
                        Status = StatusCodes.Status200OK,
                    };
                    return response;
                }
            }
            else if (changeParametr.ToLower() == "isactive")
            {
                if (artist.IsActive == true)
                {
                    artist.IsActive = false;
                    _context.SaveChanges();
                    
                    var response = new ApiResponse<ArtistDetailsDTO>
                    {
                        Data = _mapper.Map<ArtistDetailsDTO>(artist),
                        Message = null,
                        Status = StatusCodes.Status200OK,
                    };
                    return response;
                }
                else
                {
                    artist.IsActive = true;
                    _context.SaveChanges();
                    
                    var response = new ApiResponse<ArtistDetailsDTO>
                    {
                        Data = _mapper.Map<ArtistDetailsDTO>(artist),
                        Message = null,
                        Status = StatusCodes.Status200OK,
                    };
                    return response;
                }
            }
            else
            {
                var response = new ApiResponse<ArtistDetailsDTO>
                {
                    Data = null,
                    Message = "wrong change parameter",
                    Status = StatusCodes.Status403Forbidden,
                };
                return response;
            }
        }
    }

    public ApiResponse<ArtistDetailsDTO> DeleteArtistDetails(Guid artistId)
    {
        var artist = _context.Artists
            .Include(x => x.Details)
            .FirstOrDefault(x => x.Id == artistId);

        if (artist == null)
        {
            var response = new ApiResponse<ArtistDetailsDTO>
            {
                Data = null,
                Message = "Artist not found",
                Status = StatusCodes.Status404NotFound,
            };
            return response;
        }
        else
        {
            if (artist.Details != null)
            {
                var response = new ApiResponse<ArtistDetailsDTO>
                {
                    Data = null,
                    Message = "Artist details already exists",
                    Status = StatusCodes.Status403Forbidden,
                };
                return response;
            }
            else
            {
                _context.ArtistDetails.Remove(artist.Details);
                _context.SaveChanges();
                
                var response = new ApiResponse<ArtistDetailsDTO>
                {
                    Data = _mapper.Map<ArtistDetailsDTO>(artist),
                    Message = null,
                    Status = StatusCodes.Status200OK,
                };
                return response;
            }
        }
    }
}