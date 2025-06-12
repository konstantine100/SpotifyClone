using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SpotifyClone.CORE;
using SpotifyClone.Data;
using SpotifyClone.DTOs;
using SpotifyClone.Enums;
using SpotifyClone.Models;
using SpotifyClone.Requests;
using SpotifyClone.Services.Interfaces;
using SpotifyClone.Validation;

namespace SpotifyClone.Services.Implenetation;

public class AlbumService : IAlbumService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    
    public AlbumService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public ApiResponse<AlbumDTO> AddAlbum(Guid artistId, AddAlbum request)
    {
        var artist = _context.Artists
            .Include(x => x.Albums)
            .Include(x => x.Details)
            .FirstOrDefault(x => x.Id == artistId);

        if (artist == null)
        {
            var response = new ApiResponse<AlbumDTO>
            {
                Data = null,
                Message = "Artist not found",
                Status = StatusCodes.Status404NotFound
            };
            return response;
        }
        else
        {
            var album = _mapper.Map<Album>(request);
            var validator = new AlbumValidator();
            var result = validator.Validate(album);

            if (!result.IsValid)
            {
                var response = new ApiResponse<AlbumDTO>
                {
                    Data = null,
                    Message = "wrong album information",
                    Status = StatusCodes.Status403Forbidden
                };
                return response;
            }
            else if (request.ReleaseYear < artist.Details.FormationYear.Year)
            {
                var response = new ApiResponse<AlbumDTO>
                {
                    Data = null,
                    Message = "wrong album year, cant be older than artist",
                    Status = StatusCodes.Status403Forbidden
                };
                return response;
            }
            else
            {
                artist.Albums.Add(album);
                _context.SaveChanges();
                
                var response = new ApiResponse<AlbumDTO>
                {
                    Data = _mapper.Map<AlbumDTO>(album),
                    Message = null,
                    Status = StatusCodes.Status200OK
                };
                return response;
            }
        }
    }

    public ApiResponse<GenreDTO> AddAlbumGenre(Guid artistId, Guid albumId, GENRE request)
    {
        var artist = _context.Artists
            .Include(x => x.Albums)
            .ThenInclude(x => x.Genres)
            .FirstOrDefault(x => x.Id == artistId);

        if (artist == null)
        {
            var response = new ApiResponse<GenreDTO>
            {
                Data = null,
                Message = "Artist not found",
                Status = StatusCodes.Status404NotFound
            };
            return response;
        }
        else
        {
            var album = artist.Albums
                .FirstOrDefault(x => x.Id == albumId);

            if (album == null)
            {
                var response = new ApiResponse<GenreDTO>
                {
                    Data = null,
                    Message = "Album not found",
                    Status = StatusCodes.Status404NotFound
                };
                return response;
            }
            else
            {
                var genre = new Genre
                {
                    Genres = request,
                    AlbumId = album.Id
                };
                
                album.Genres.Add(genre);
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

    public ApiResponse<List<AlbumDTO>> GetAllAlbums()
    {
        var album = _context.Albums
            .Include(x => x.Genres)
            .OrderByDescending(x => x.OverallPlayed)
            .ToList();
        
        var response = new ApiResponse<List<AlbumDTO>>
        {
            Data = _mapper.Map<List<AlbumDTO>>(album),
            Message = null,
            Status = StatusCodes.Status200OK
        };
        return response;
    }

    public ApiResponse<List<AlbumDTO>> GetAlbumsFinder(string? search, string? sortBy, GENRE? genre)
    {
        var album = _context.Albums
            .Include(x => x.Genres)
            .ToList();

        if (!string.IsNullOrEmpty(search))
        {
            album = album
                .Where(x => x.Title.ToLower().Contains(search.ToLower()))
                .ToList();
        }

        if (!string.IsNullOrEmpty(sortBy))
        {
            switch (sortBy.ToLower())
            {
                case "title_asc":
                    album = album.OrderBy(x => x.Title).ToList();
                    break;
                case "title_desc":
                    album = album.OrderByDescending(x => x.Title).ToList();
                    break;
                case "release_year_asc":
                    album = album.OrderBy(x => x.ReleaseYear).ToList();
                    break;
                case "release_year_desc":
                    album = album.OrderByDescending(x => x.ReleaseYear).ToList();
                    break;
                case "length_asc":
                    album = album.OrderBy(x => x.AlbumLength).ToList();
                    break;
                case "length_desc":
                    album = album.OrderByDescending(x => x.AlbumLength).ToList();
                    break;
                case "song_count_asc":
                    album = album.OrderBy(x => x.SongCount).ToList();
                    break;
                case "song_count_desc":
                    album = album.OrderByDescending(x => x.SongCount).ToList();
                    break;
                case "overall_played_asc":
                    album = album.OrderBy(x => x.OverallPlayed).ToList();
                    break;
                case "overall_played_desc":
                    album = album.OrderByDescending(x => x.OverallPlayed).ToList();
                    break;
            }
        }

        if (!string.IsNullOrEmpty(genre.ToString()))
        {
            album = album.Where(x => x.Genres.Any(y => y.Genres == genre)).ToList();
        }
        
        
        var response = new ApiResponse<List<AlbumDTO>>
        {
            Data = _mapper.Map<List<AlbumDTO>>(album),
            Message = null,
            Status = StatusCodes.Status200OK
        };
        return response;
    }

    public ApiResponse<List<AlbumDTO>> GetArtistAlbums(Guid artistId)
    {
        var artist = _context.Artists
            .Include(x => x.Albums)
            .FirstOrDefault(x => x.Id == artistId);

        if (artist == null)
        {
            var response = new ApiResponse<List<AlbumDTO>>
            {
                Data = null,
                Message = "Artist not found",
                Status = StatusCodes.Status404NotFound
            };
            return response;
        }
        else
        {
            var response = new ApiResponse<List<AlbumDTO>>
            {
                Data = _mapper.Map<List<AlbumDTO>>(artist.Albums),
                Message = null,
                Status = StatusCodes.Status200OK
            };
            return response;
        }
    }

    public ApiResponse<AlbumDTO> GetAlbumById(Guid albumId)
    {
        var album = _context.Albums
            .FirstOrDefault(x => x.Id == albumId);

        if (album == null)
        {
            var response = new ApiResponse<AlbumDTO>
            {
                Data = null,
                Message = "album not found",
                Status = StatusCodes.Status404NotFound
            };
            return response;
        }
        else
        {
            var response = new ApiResponse<AlbumDTO>
            {
                Data = _mapper.Map<AlbumDTO>(album),
                Message = null,
                Status = StatusCodes.Status200OK
            };
            return response;
        }
    }

    public ApiResponse<AlbumDTO> ChangeAlbum(Guid artistId, Guid albumId, string changeParametr, string changeTo)
    {
        var artist = _context.Artists
            .Include(x => x.Details)
            .Include(x => x.Albums)
            .ThenInclude(x => x.Genres)
            .FirstOrDefault(x => x.Id == artistId);

        if (artist == null)
        {
            var response = new ApiResponse<AlbumDTO>
            {
                Data = null,
                Message = "Artist not found",
                Status = StatusCodes.Status404NotFound
            };
            return response;
        }
        else
        {
            var album = artist.Albums
                .FirstOrDefault(x => x.Id == albumId);

            if (album == null)
            {
                var response = new ApiResponse<AlbumDTO>
                {
                    Data = null,
                    Message = "Album not found",
                    Status = StatusCodes.Status404NotFound
                };
                return response;
            }
            else
            {
                if (changeParametr.ToLower() == "title")
                {
                    album.Title = changeTo;
                    var validator = new AlbumValidator();
                    var result = validator.Validate(album);

                    if (!result.IsValid)
                    {
                        var response = new ApiResponse<AlbumDTO>
                        {
                            Data = null,
                            Message = "wrong album information",
                            Status = StatusCodes.Status403Forbidden
                        };
                        return response;
                    }
                    else
                    {
                        _context.SaveChanges();
                        
                        var response = new ApiResponse<AlbumDTO>
                        {
                            Data = _mapper.Map<AlbumDTO>(album),
                            Message = null,
                            Status = StatusCodes.Status200OK
                        };
                        return response;
                    }
                }
                else if (changeParametr.ToLower() == "albumcover")
                {
                    album.AlbumCover = changeTo;
                    var validator = new AlbumValidator();
                    var result = validator.Validate(album);

                    if (!result.IsValid)
                    {
                        var response = new ApiResponse<AlbumDTO>
                        {
                            Data = null,
                            Message = "wrong album information",
                            Status = StatusCodes.Status403Forbidden
                        };
                        return response;
                    }
                    else
                    {
                        _context.SaveChanges();
                        
                        var response = new ApiResponse<AlbumDTO>
                        {
                            Data = _mapper.Map<AlbumDTO>(album),
                            Message = null,
                            Status = StatusCodes.Status200OK
                        };
                        return response;
                    }
                }
                else if (changeParametr.ToLower() == "releaseyear")
                {
                    album.ReleaseYear = int.Parse(changeTo);
                    var validator = new AlbumValidator();
                    var result = validator.Validate(album);

                    if (!result.IsValid)
                    {
                        var response = new ApiResponse<AlbumDTO>
                        {
                            Data = null,
                            Message = "wrong album information",
                            Status = StatusCodes.Status403Forbidden
                        };
                        return response;
                    }
                    else if (album.ReleaseYear < artist.Details.FormationYear.Year)
                    {
                        var response = new ApiResponse<AlbumDTO>
                        {
                            Data = null,
                            Message = "wrong album year, cant be older than artist",
                            Status = StatusCodes.Status403Forbidden
                        };
                        return response;
                    }
                    else
                    {
                        _context.SaveChanges();
                        
                        var response = new ApiResponse<AlbumDTO>
                        {
                            Data = _mapper.Map<AlbumDTO>(album),
                            Message = null,
                            Status = StatusCodes.Status200OK
                        };
                        return response;
                    }
                }
                else
                {
                    var response = new ApiResponse<AlbumDTO>
                    {
                        Data = null,
                        Message = "wrong change parametr",
                        Status = StatusCodes.Status403Forbidden
                    };
                    return response;
                }
            }
        }
    }

    public ApiResponse<AlbumDTO> DeleteAlbum(Guid artistId, Guid albumId)
    {
        var artist = _context.Artists
            .Include(x => x.Albums)
            .FirstOrDefault(x => x.Id == artistId);

        if (artist == null)
        {
            var response = new ApiResponse<AlbumDTO>
            {
                Data = null,
                Message = "Artist not found",
                Status = StatusCodes.Status404NotFound
            };
            return response;
        }
        else
        {
            var album = artist.Albums
                .FirstOrDefault(x => x.Id == albumId);

            if (album == null)
            {
                var response = new ApiResponse<AlbumDTO>
                {
                    Data = null,
                    Message = "Album not found",
                    Status = StatusCodes.Status404NotFound
                };
                return response;
            }
            else
            {
                _context.Albums.Remove(album);
                _context.SaveChanges();
                
                var response = new ApiResponse<AlbumDTO>
                {
                    Data = _mapper.Map<AlbumDTO>(album),
                    Message = null,
                    Status = StatusCodes.Status200OK
                };
                return response;
            }
        }
    }

    public ApiResponse<GenreDTO> DeleteAlbumGenre(Guid artistId, Guid albumId, Guid genreId)
    {
        var artist = _context.Artists
            .Include(x => x.Albums)
            .ThenInclude(x => x.Genres)
            .FirstOrDefault(x => x.Id == artistId);

        if (artist == null)
        {
            var response = new ApiResponse<GenreDTO>
            {
                Data = null,
                Message = "Artist not found",
                Status = StatusCodes.Status404NotFound
            };
            return response;
        }
        else
        {
            var album = artist.Albums
                .FirstOrDefault(x => x.Id == albumId);

            if (album == null)
            {
                var response = new ApiResponse<GenreDTO>
                {
                    Data = null,
                    Message = "Album not found",
                    Status = StatusCodes.Status404NotFound
                };
                return response;
            }
            else
            {
                var genre = album.Genres
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
}