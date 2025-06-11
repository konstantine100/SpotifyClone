using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SpotifyClone.CORE;
using SpotifyClone.Data;
using SpotifyClone.DTOs;
using SpotifyClone.Enums;
using SpotifyClone.Models;
using SpotifyClone.Requests;
using SpotifyClone.Services.Interfaces;
using SpotifyClone.SMTP;
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

    public ApiResponse<GenreDTO> PostGenre(Guid artistId, GENRE genre)
    {
        var artist = _context.Artists
            .Include(x => x.Genres)
            .FirstOrDefault(x => x.Id == artistId);

        if (artist == null)
        {
            var response = new ApiResponse<GenreDTO>
            {
                Data = null,
                Message = "artist not found",
                Status = StatusCodes.Status404NotFound
            };
            return response;
        }
        else
        {
            var genreToAdd = new Genre
            {
                Genres = genre,
                ArtistId = artist.Id
            };
            artist.Genres.Add(genreToAdd);
            _context.SaveChanges();
            
            var response = new ApiResponse<GenreDTO>
            {
                Data = _mapper.Map<GenreDTO>(genre),
                Message = null,
                Status = StatusCodes.Status200OK
            };
            return response;
        }
    }

    public ApiResponse<ArtistDTO> RequestVerified(Guid artistId)
    {
        var artist = _context.Artists
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
                var users = _context.Users
                    .Where(x => x.Role == ROLES.Admin)
                    .ToList();

                if (users != null && users.Count > 0)
                {
                    foreach (var admin in users)
                    {
                        SMTPService smtpService = new SMTPService();

                        smtpService.SendEmail(admin.Email, "Artist Verification", $"<p>admins please verify artist {artist.Name}, id: {artist.Id}, liseners: {artist.Listens}</p>");
                    }
                
                    var response = new ApiResponse<ArtistDTO>
                    {
                        Data = null,
                        Message = "message sent successfully",
                        Status = StatusCodes.Status200OK
                    };
                    return response;
                }
                else
                {
                    var response = new ApiResponse<ArtistDTO>
                    {
                        Data = null,
                        Message = "no admins found",
                        Status = StatusCodes.Status404NotFound
                    };
                    return response;
                }
            }
        }
    }

    public ApiResponse<List<ArtistDTO>> GetArtists()
    {
        var artists = _context.Artists
            .Include(x => x.Genres)
            .OrderByDescending(x => x.Listens)
            .OrderByDescending(x => x.IsVerified)
            .ToList();
        
        var response = new ApiResponse<List<ArtistDTO>>
        {
            Data = _mapper.Map<List<ArtistDTO>>(artists),
            Message = null,
            Status = StatusCodes.Status200OK
        };
        return response;
    }

    public ApiResponse<List<ArtistDTO>> GetArtistFinder(string? search, string? sortBy, COUNTRY? country, GENRE? genre)
    {
        var artists = _context.Artists
            .Include(x => x.Details)
            .Include(x => x.Genres)
            .ToList();

        if (!string.IsNullOrWhiteSpace(search))
        {
            artists = artists
                .Where(x => x.Name.ToLower().Contains(search.ToLower()) ||
                                 x.Country.ToString().Contains(search.ToLower()) ||
                                 (x.Details != null &&
                                 x.Details.Description.ToLower().Contains(search.ToLower())) ||
                                 (x.Details != null &&
                                 x.Details.Website.ToLower().Contains(search.ToLower())))
                .ToList();
        }

        if (!string.IsNullOrWhiteSpace(sortBy))
        {
            switch (sortBy.ToLower())
            {
                case "name_asc":
                    artists = artists.OrderBy(x => x.Name).ToList();
                    break;
                case "name_desc":
                    artists = artists.OrderByDescending(x => x.Name).ToList();
                    break;
                case "country_asc":
                    artists = artists.OrderBy(x => x.Country.ToString()).ToList();
                    break;
                case "country_desc":
                    artists = artists.OrderByDescending(x => x.Country.ToString()).ToList();
                    break;
                case "listens_asc":
                    artists = artists.OrderBy(x => x.Listens).ToList();
                    break;
                case "listens_desc":
                    artists = artists.OrderByDescending(x => x.Listens).ToList();
                    break;
                case "is_verified_asc":
                    artists = artists.OrderBy(x => x.IsVerified).ToList();
                    break;
                case "is_verified_desc":
                    artists = artists.OrderByDescending(x => x.IsVerified).ToList();
                    break;
                case "formation_year_asc":
                    artists = artists
                        .OrderBy(x => x.Details?.FormationYear ?? DateTime.MaxValue)
                        .ToList();
                    break;
                case "formation_year_desc":
                    artists = artists
                        .OrderByDescending(x => x.Details?.FormationYear ?? DateTime.MaxValue)
                        .ToList();
                    break;
                case "is_active_asc":
                    artists = artists
                        .OrderBy(x => x.Details?.IsActive ?? true) 
                        .ToList();
                    break;
                case "is_active_desc":
                    artists = artists
                        .OrderByDescending(x => x.Details?.IsActive ?? false) 
                        .ToList();
                    break;
                default:
                    artists = artists;
                    break;
            }
        }

        if (!string.IsNullOrWhiteSpace(country.ToString()))
        {
            artists = artists.Where(x => x.Country == country).ToList();
        }

        if (!string.IsNullOrWhiteSpace(genre.ToString()))
        {
            artists = artists.Where(x => x.Genres.Any(x => x.Genres == genre)).ToList();
        }
        
        var response = new ApiResponse<List<ArtistDTO>>
        {
            Data = _mapper.Map<List<ArtistDTO>>(artists),
            Message = null,
            Status = StatusCodes.Status200OK
        };
        return response;
    }

    public ApiResponse<ArtistDTO> GetArtist(Guid artistId)
    {
        var artist = _context.Artists
            .Include(x => x.Details)
            .Include(x => x.Genres)
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
            var response = new ApiResponse<ArtistDTO>
            {
                Data = _mapper.Map<ArtistDTO>(artist),
                Message = null,
                Status = StatusCodes.Status200OK
            };
            return response;
        }
    }

    public ApiResponse<ArtistDTO> ChangeArtist(Guid artistId, string changeParametr, string changeTo, COUNTRY? country)
    {
        var artist = _context.Artists
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
            if (changeParametr.ToLower() == "name")
            {
                artist.Name = changeTo;
                var validator = new ArtistValidator();
                var result = validator.Validate(artist);

                if (!result.IsValid)
                {
                    var response = new ApiResponse<ArtistDTO>
                    {
                        Data = null,
                        Message = "wrong artist information",
                        Status = StatusCodes.Status403Forbidden
                    };
                    return response;
                }
                else
                {
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
            else if (changeParametr.ToLower() == "profilepicture")
            {
                artist.ProfilePicture = changeTo;
                var validator = new ArtistValidator();
                var result = validator.Validate(artist);

                if (!result.IsValid)
                {
                    var response = new ApiResponse<ArtistDTO>
                    {
                        Data = null,
                        Message = "wrong artist information",
                        Status = StatusCodes.Status403Forbidden
                    };
                    return response;
                }
                else
                {
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
            else if (changeParametr.ToLower() == "country" && country != null)
            {
                artist.Country = country.Value;
                var validator = new ArtistValidator();
                var result = validator.Validate(artist);

                if (!result.IsValid)
                {
                    var response = new ApiResponse<ArtistDTO>
                    {
                        Data = null,
                        Message = "wrong artist information",
                        Status = StatusCodes.Status403Forbidden
                    };
                    return response;
                }
                else
                {
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
            else
            {
                var response = new ApiResponse<ArtistDTO>
                {
                    Data = null,
                    Message = "wrong change parametr",
                    Status = StatusCodes.Status403Forbidden
                };
                return response;
            }
        }
    }

    public ApiResponse<ArtistDTO> DeleteArtist(Guid artistId)
    {
        var artist = _context.Artists
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
            _context.Artists.Remove(artist);
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

    public ApiResponse<GenreDTO> DeleteGenre(Guid artistId, Guid genreId)
    {
        var artist = _context.Artists
            .Include(x => x.Genres)
            .FirstOrDefault(x => x.Id == artistId);

        if (artist == null)
        {
            var response = new ApiResponse<GenreDTO>
            {
                Data = null,
                Message = "artist not found",
                Status = StatusCodes.Status404NotFound
            };
            return response;
        }
        else
        {
            var genre = artist.Genres
                .FirstOrDefault(x => x.Id == genreId);

            if (genre == null)
            {
                var response = new ApiResponse<GenreDTO>
                {
                    Data = null,
                    Message = "genre not found",
                    Status = StatusCodes.Status404NotFound
                };
                return response;
            }
            else
            {
                _context.Genres.Remove(genre);
                _context.SaveChanges();
                
                var response = new ApiResponse<GenreDTO>
                {
                    Data = _mapper.Map<GenreDTO>(genre),
                    Message = null,
                    Status = StatusCodes.Status200OK
                };
                return response;
            }
        }
    }
}