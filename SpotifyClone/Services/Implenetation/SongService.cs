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

public class SongService : ISongService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    
    public SongService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public ApiResponse<SongDTO> PostSong(Guid artistId, Guid albumId, AddSong request)
    {
        var artist = _context.Artists
            .Include(x => x.Albums)
            .ThenInclude(x => x.Songs)
            .FirstOrDefault(a => a.Id == artistId);

        if (artist == null)
        {
            var response = new ApiResponse<SongDTO>
            {
                Data = null,
                Message = "Artist not found",
                Status = StatusCodes.Status404NotFound
            };
            return response;
        }
        else
        {
            var album = artist.Albums.FirstOrDefault(a => a.Id == albumId);

            if (album == null)
            {
                var response = new ApiResponse<SongDTO>
                {
                    Data = null,
                    Message = "album not found",
                    Status = StatusCodes.Status404NotFound
                };
                return response;
            }
            else
            {
                var song = _mapper.Map<Song>(request);
                var validator = new SongValidator();
                var result = validator.Validate(song);

                if (!result.IsValid)
                {
                    var response = new ApiResponse<SongDTO>
                    {
                        Data = null,
                        Message = "wrong artist information",
                        Status = StatusCodes.Status403Forbidden
                    };
                    return response;
                }
                else
                {
                    song.AlbumId = album.Id;
                    album.Songs.Add(song);
                    album.SongCount++;
                    album.AlbumLength = album.AlbumLength + song.Duration;
                    _context.SaveChanges();
                    
                    var response = new ApiResponse<SongDTO>
                    {
                        Data = _mapper.Map<SongDTO>(song),
                        Message = null,
                        Status = StatusCodes.Status200OK
                    };
                    return response;
                }
            }
        }
    }

    public ApiResponse<GenreDTO> PostSongGenre(Guid artistId, Guid albumId, Guid songId, GENRE request)
    {
        var artist = _context.Artists
            .Include(x => x.Albums)
            .ThenInclude(x => x.Songs)
            .ThenInclude(x => x.Genres)
            .FirstOrDefault(a => a.Id == artistId);

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
            var album = artist.Albums.FirstOrDefault(a => a.Id == albumId);

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
                var song = album.Songs.FirstOrDefault(s => s.Id == songId);

                if (song == null)
                {
                    var response = new ApiResponse<GenreDTO>
                    {
                        Data = null,
                        Message = "Song not found",
                        Status = StatusCodes.Status404NotFound
                    };
                    return response;
                }
                else
                {
                    var genre = new Genre
                    {
                        SongId = song.Id,
                        Genres = request
                    };
                    
                    song.Genres.Add(genre);
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

    public ApiResponse<ArtistDTO> PostSongFeatureArtist(Guid artistId, Guid albumId, Guid songId, Guid ftArtistId)
    {
        var artist = _context.Artists
            .Include(x => x.Albums)
            .ThenInclude(x => x.Songs)
            .ThenInclude(x => x.FeaturingArtist)
            .FirstOrDefault(a => a.Id == artistId);

        if (artist == null)
        {
            var response = new ApiResponse<ArtistDTO>
            {
                Data = null,
                Message = "Artist not found",
                Status = StatusCodes.Status404NotFound
            };
            return response;
        }
        else
        {
            var album = artist.Albums.FirstOrDefault(a => a.Id == albumId);

            if (album == null)
            {
                var response = new ApiResponse<ArtistDTO>
                {
                    Data = null,
                    Message = "Album not found",
                    Status = StatusCodes.Status404NotFound
                };
                return response;
            }
            else
            {
                var song = album.Songs.FirstOrDefault(s => s.Id == songId);

                if (song == null)
                {
                    var response = new ApiResponse<ArtistDTO>
                    {
                        Data = null,
                        Message = "Song not found",
                        Status = StatusCodes.Status404NotFound
                    };
                    return response;
                }
                else
                {
                    var ftArtist = _context.Artists
                        .Include(x => x.FeaturingSongs)
                        .FirstOrDefault(a => a.Id == ftArtistId);

                    if (ftArtist == null)
                    {
                        var response = new ApiResponse<ArtistDTO>
                        {
                            Data = null,
                            Message = "feature artist not found",
                            Status = StatusCodes.Status404NotFound
                        };
                        return response;
                    }
                    else
                    {
                        song.FeaturingArtist.Add(ftArtist);
                        ftArtist.FeaturingSongs.Add(song);
                        _context.SaveChanges();
                        
                        var response = new ApiResponse<ArtistDTO>
                        {
                            Data = _mapper.Map<ArtistDTO>(ftArtist),
                            Message = null,
                            Status = StatusCodes.Status200OK
                        };
                        return response;
                    }
                }
            }
        }
    }

    public ApiResponse<SongComposersDTO> PostSongComposers(Guid artistId, Guid albumId, Guid songId, Guid composerId, SONG_CONTRIBUTOR request)
    {
        var artist = _context.Artists
            .Include(x => x.Albums)
            .ThenInclude(x => x.Songs)
            .ThenInclude(x => x.SongComposers)
            .FirstOrDefault(a => a.Id == artistId);

        if (artist == null)
        {
            var response = new ApiResponse<SongComposersDTO>
            {
                Data = null,
                Message = "Artist not found",
                Status = StatusCodes.Status404NotFound
            };
            return response;
        }
        else
        {
            var album = artist.Albums.FirstOrDefault(a => a.Id == albumId);

            if (album == null)
            {
                var response = new ApiResponse<SongComposersDTO>
                {
                    Data = null,
                    Message = "Album not found",
                    Status = StatusCodes.Status404NotFound
                };
                return response;
            }
            else
            {
                var song = album.Songs.FirstOrDefault(s => s.Id == songId);

                if (song == null)
                {
                    var response = new ApiResponse<SongComposersDTO>
                    {
                        Data = null,
                        Message = "Song not found",
                        Status = StatusCodes.Status404NotFound
                    };
                    return response;
                }
                else
                {
                    var composer = _context.Artists
                        .Include(x => x.ArtistComposedSongs)
                        .FirstOrDefault(x => x.Id == composerId);

                    if (composer == null)
                    {
                        var response = new ApiResponse<SongComposersDTO>
                        {
                            Data = null,
                            Message = "composer not found",
                            Status = StatusCodes.Status404NotFound
                        };
                        return response;
                    }
                    else
                    {
                        var songComposer = new SongComposers
                        {
                            Position = request,
                            SongId = song.Id,
                            ArtistId = composerId
                        };
                        
                        song.SongComposers.Add(songComposer);
                        composer.ArtistComposedSongs.Add(songComposer);
                        _context.SaveChanges();
                        
                        var response = new ApiResponse<SongComposersDTO>
                        {
                            Data = _mapper.Map<SongComposersDTO>(songComposer),
                            Message = null,
                            Status = StatusCodes.Status200OK
                        };
                        return response;
                    }
                    
                }
            }
        }
    }

    public ApiResponse<List<SongDTO>> GetAllSongs()
    {
        var songs = _context.Songs
            .OrderByDescending(x => x.TimesPlayed)
            .ToList();

        var response = new ApiResponse<List<SongDTO>>
        {
            Data = _mapper.Map<List<SongDTO>>(songs),
            Message = null,
            Status = StatusCodes.Status200OK
        };
        return response;
    }

    public ApiResponse<List<SongDTO>> GetSongFinder(string? search, string? sortBy, string? filterBy, GENRE? genre)
    {
        var songs = _context.Songs
            .Include(x => x.Genres)
            .Include(x => x.Album)
            .ThenInclude(x => x.Artist)
            .Include(x => x.FeaturingArtist)
            .ToList();

        if (!string.IsNullOrEmpty(search))
        {
            songs = songs
                .Where(x => x.Title.ToLower().Contains(search.ToLower()) ||
                                 x.Lyrics.ToLower().Contains(search.ToLower()) ||
                                 x.Album.Title.ToLower().Contains(search.ToLower()) ||
                                 x.Album.Artist.Name.ToLower().Contains(search.ToLower()) ||
                                 x.FeaturingArtist.Any(x => x.Name.ToLower().Contains(search.ToLower())))
                .ToList();
        }

        if (!string.IsNullOrEmpty(sortBy))
        {
            switch (sortBy.ToLower())
            {
                case "title_asc":
                    songs = songs.OrderBy(x => x.Title).ToList();
                    break;
                case "title_desc":
                    songs = songs.OrderByDescending(x => x.Title).ToList();
                    break;
                case "listens_asc":
                    songs = songs.OrderBy(x => x.TimesPlayed).ToList();
                    break;
                case "listens_desc":
                    songs = songs.OrderByDescending(x => x.TimesPlayed).ToList();
                    break;
                case "duration_asc":
                    songs = songs.OrderBy(x => x.Duration).ToList();
                    break;
                case "duration_desc":
                    songs = songs.OrderByDescending(x => x.Duration).ToList();
                    break;
                case "explecit_asc":
                    songs = songs.OrderBy(x => x.IsExplicit).ToList();
                    break;
                case "explecit_desc":
                    songs = songs.OrderByDescending(x => x.IsExplicit).ToList();
                    break;
                default:
                    songs = songs;
                    break;
            }
        }

        if (!string.IsNullOrEmpty(filterBy))
        {

            if (filterBy.ToLower() == "explicit")
            {
                songs = songs.Where(x => x.IsExplicit).ToList();
            }
            else if (filterBy.ToLower() == "non_explicit")
            {
                songs = songs.Where(x => x.IsExplicit == false).ToList();
            }
            else if (filterBy.ToLower() == "genre" && genre.ToString() != null)
            {
                songs = songs.Where(x => x.Genres.Any(x => x.Genres == genre)).ToList();
            }
            else
            {
                songs = songs;
            }
        }
        
        var response = new ApiResponse<List<SongDTO>>
        {
            Data = _mapper.Map<List<SongDTO>>(songs),
            Message = null,
            Status = StatusCodes.Status200OK
        };
        return response;
    }

    public ApiResponse<List<SongDTO>> GetArtistSongs(Guid artistId)
    {
        var artist = _context.Artists
            .Include(x => x.Albums)
            .ThenInclude(x => x.Songs)
            .ThenInclude(x => x.Genres)
            .FirstOrDefault(x => x.Id == artistId);

        if (artist == null)
        {
            var response = new ApiResponse<List<SongDTO>>
            {
                Data = null,
                Message = "artist not found",
                Status = StatusCodes.Status404NotFound
            };
            return response;
        }
        else
        {
            List<Song> songs = new List<Song>();
            var albums = artist.Albums;

            foreach (var album in albums)
            {
                foreach (var song in album.Songs)
                {
                    songs.Add(song);
                }
            }
            
            var response = new ApiResponse<List<SongDTO>>
            {
                Data = _mapper.Map<List<SongDTO>>(songs),
                Message = null,
                Status = StatusCodes.Status200OK
            };
            return response;
        }
        
    }

    public ApiResponse<List<SongDTO>> GetAlbumSongs(Guid albumId)
    {
        var album = _context.Albums
            .Include(x => x.Songs)
            .ThenInclude(x => x.Genres)
            .Include(x => x.Songs)
            .ThenInclude(x => x.FeaturingArtist)
            .FirstOrDefault(x => x.Id == albumId);

        if (album == null)
        {
            var response = new ApiResponse<List<SongDTO>>
            {
                Data = null,
                Message = "album not found",
                Status = StatusCodes.Status404NotFound
            };
            return response;
        }
        else
        {
            var response = new ApiResponse<List<SongDTO>>
            {
                Data = _mapper.Map<List<SongDTO>>(album.Songs),
                Message = null,
                Status = StatusCodes.Status200OK
            };
            return response;
        }
    }

    public ApiResponse<SongDTO> GetSongById(Guid songId)
    {
        var song = _context.Songs
            .Include(x => x.Genres)
            .Include(x => x.FeaturingArtist)
            .Include(x => x.SongComposers)
            .FirstOrDefault(x => x.Id == songId);

        if (song == null)
        {
            var response = new ApiResponse<SongDTO>
            {
                Data = null,
                Message = "song not found",
                Status = StatusCodes.Status404NotFound
            };
            return response;
        }
        else
        {
            var response = new ApiResponse<SongDTO>
            {
                Data = _mapper.Map<SongDTO>(song),
                Message = null,
                Status = StatusCodes.Status200OK
            };
            return response;
        }
    }

    public ApiResponse<SongDTO> PlaySong(Guid songId)
    {
        var song = _context.Songs
            .Include(x => x.Album)
            .ThenInclude(x => x.Artist)
            .Include(x => x.Genres)
            .Include(x => x.FeaturingArtist)
            .Include(x => x.SongComposers)
            .FirstOrDefault(x => x.Id == songId);

        if (song == null)
        {
            var response = new ApiResponse<SongDTO>
            {
                Data = null,
                Message = "song not found",
                Status = StatusCodes.Status404NotFound
            };
            return response;
        }
        else
        {
            song.TimesPlayed++;
            song.Album.OverallPlayed++;
            song.Album.Artist.Listens++;

            _context.SaveChanges();
                
            var response = new ApiResponse<SongDTO>
            {
                Data = _mapper.Map<SongDTO>(song),
                Message = $"now playing: {song.Album.Artist.Name} - {song.Title} [{song.Duration}] ",
                Status = StatusCodes.Status200OK
            };
            return response;
        }
    }

    public ApiResponse<List<ArtistDTO>> GetFeaturingArtists(Guid songId)
    {
        var song = _context.Songs
            .Include(x => x.FeaturingArtist)
            .FirstOrDefault(x => x.Id == songId);

        if (song == null)
        {
            var response = new ApiResponse<List<ArtistDTO>>
            {
                Data = null,
                Message = "song not found",
                Status = StatusCodes.Status404NotFound
            };
            return response;
        }
        else
        {
            var response = new ApiResponse<List<ArtistDTO>>
            {
                Data = _mapper.Map<List<ArtistDTO>>(song.FeaturingArtist),
                Message = null,
                Status = StatusCodes.Status200OK
            };
            return response;
        }
    }

    public ApiResponse<List<SongComposersDTO>> GetSongComposers(Guid songId)
    {
        var song = _context.Songs
            .Include(x => x.SongComposers)
            .ThenInclude(x => x.Artist)
            .FirstOrDefault(x => x.Id == songId);

        if (song == null)
        {
            var response = new ApiResponse<List<SongComposersDTO>>
            {
                Data = null,
                Message = "song not found",
                Status = StatusCodes.Status404NotFound
            };
            return response;
        }
        else
        {
            var response = new ApiResponse<List<SongComposersDTO>>
            {
                Data = _mapper.Map<List<SongComposersDTO>>(song.SongComposers),
                Message = null,
                Status = StatusCodes.Status200OK
            };
            return response;
        }
    }

    public ApiResponse<SongDTO> ChangeSong(Guid artistId, Guid albumId, Guid songId, string changeParametr, string changeTo)
    {
        var artist = _context.Artists
            .Include(x => x.Albums)
            .ThenInclude(x => x.Songs)
            .FirstOrDefault(a => a.Id == artistId);

        if (artist == null)
        {
            var response = new ApiResponse<SongDTO>
            {
                Data = null,
                Message = "Artist not found",
                Status = StatusCodes.Status404NotFound
            };
            return response;
        }
        else
        {
            var album = artist.Albums.FirstOrDefault(a => a.Id == albumId);

            if (album == null)
            {
                var response = new ApiResponse<SongDTO>
                {
                    Data = null,
                    Message = "Album not found",
                    Status = StatusCodes.Status404NotFound
                };
                return response;
            }
            else
            {
                var song = album.Songs.FirstOrDefault(s => s.Id == songId);

                if (song == null)
                {
                    var response = new ApiResponse<SongDTO>
                    {
                        Data = null,
                        Message = "Song not found",
                        Status = StatusCodes.Status404NotFound
                    };
                    return response;
                }
                else
                {
                    if (changeParametr.ToLower() == "title")
                    {
                        song.Title = changeTo;
                        var validator = new SongValidator();
                        var result = validator.Validate(song);

                        if (!result.IsValid)
                        {
                            var response = new ApiResponse<SongDTO>
                            {
                                Data = null,
                                Message = "wrong song information",
                                Status = StatusCodes.Status403Forbidden
                            };
                            return response;
                        }
                        else
                        {
                            _context.SaveChanges();
                            
                            var response = new ApiResponse<SongDTO>
                            {
                                Data = _mapper.Map<SongDTO>(song),
                                Message = null,
                                Status = StatusCodes.Status200OK
                            };
                            return response;
                        }
                    }
                    else if (changeParametr.ToLower() == "duration")
                    {
                        song.Duration = TimeSpan.Parse(changeTo);
                        var validator = new SongValidator();
                        var result = validator.Validate(song);

                        if (!result.IsValid)
                        {
                            var response = new ApiResponse<SongDTO>
                            {
                                Data = null,
                                Message = "wrong song information",
                                Status = StatusCodes.Status403Forbidden
                            };
                            return response;
                        }
                        else
                        {
                            _context.SaveChanges();
                            
                            var response = new ApiResponse<SongDTO>
                            {
                                Data = _mapper.Map<SongDTO>(song),
                                Message = null,
                                Status = StatusCodes.Status200OK
                            };
                            return response;
                        }
                    }
                    else if (changeParametr.ToLower() == "songurl")
                    {
                        song.SongUrl = changeTo;
                        var validator = new SongValidator();
                        var result = validator.Validate(song);

                        if (!result.IsValid)
                        {
                            var response = new ApiResponse<SongDTO>
                            {
                                Data = null,
                                Message = "wrong song information",
                                Status = StatusCodes.Status403Forbidden
                            };
                            return response;
                        }
                        else
                        {
                            _context.SaveChanges();
                            
                            var response = new ApiResponse<SongDTO>
                            {
                                Data = _mapper.Map<SongDTO>(song),
                                Message = null,
                                Status = StatusCodes.Status200OK
                            };
                            return response;
                        }
                    }
                    else if (changeParametr.ToLower() == "lyrics")
                    {
                        song.Lyrics = changeTo;
                        var validator = new SongValidator();
                        var result = validator.Validate(song);

                        if (!result.IsValid)
                        {
                            var response = new ApiResponse<SongDTO>
                            {
                                Data = null,
                                Message = "wrong song information",
                                Status = StatusCodes.Status403Forbidden
                            };
                            return response;
                        }
                        else
                        {
                            _context.SaveChanges();
                            
                            var response = new ApiResponse<SongDTO>
                            {
                                Data = _mapper.Map<SongDTO>(song),
                                Message = null,
                                Status = StatusCodes.Status200OK
                            };
                            return response;
                        }
                    }
                    else if (changeParametr.ToLower() == "tracknumber")
                    {
                        song.TrackNumber = int.Parse(changeTo);
                        var validator = new SongValidator();
                        var result = validator.Validate(song);

                        if (!result.IsValid)
                        {
                            var response = new ApiResponse<SongDTO>
                            {
                                Data = null,
                                Message = "wrong song information",
                                Status = StatusCodes.Status403Forbidden
                            };
                            return response;
                        }
                        else
                        {
                            _context.SaveChanges();
                            
                            var response = new ApiResponse<SongDTO>
                            {
                                Data = _mapper.Map<SongDTO>(song),
                                Message = null,
                                Status = StatusCodes.Status200OK
                            };
                            return response;
                        }
                    }
                    else if (changeParametr.ToLower() == "explicit")
                    {
                        if (song.IsExplicit)
                        {
                            song.IsExplicit = false;
                            _context.SaveChanges();
                            
                            var response = new ApiResponse<SongDTO>
                            {
                                Data = _mapper.Map<SongDTO>(song),
                                Message = null,
                                Status = StatusCodes.Status200OK
                            };
                            return response;
                        }
                        else
                        {
                            song.IsExplicit = true;
                            _context.SaveChanges();
                            
                            var response = new ApiResponse<SongDTO>
                            {
                                Data = _mapper.Map<SongDTO>(song),
                                Message = null,
                                Status = StatusCodes.Status200OK
                            };
                            return response;
                        }
                    }
                    else
                    {
                        var response = new ApiResponse<SongDTO>
                        {
                            Data = null,
                            Message = "wrong change parameter",
                            Status = StatusCodes.Status403Forbidden
                        };
                        return response;
                    }
                }
            }
        }
    }

    public ApiResponse<SongDTO> DeleteSong(Guid artistId, Guid albumId, Guid songId)
    {
        var artist = _context.Artists
            .Include(x => x.Albums)
            .ThenInclude(x => x.Songs)
            .FirstOrDefault(a => a.Id == artistId);

        if (artist == null)
        {
            var response = new ApiResponse<SongDTO>
            {
                Data = null,
                Message = "Artist not found",
                Status = StatusCodes.Status404NotFound
            };
            return response;
        }
        else
        {
            var album = artist.Albums.FirstOrDefault(a => a.Id == albumId);

            if (album == null)
            {
                var response = new ApiResponse<SongDTO>
                {
                    Data = null,
                    Message = "Album not found",
                    Status = StatusCodes.Status404NotFound
                };
                return response;
            }
            else
            {
                var song = album.Songs.FirstOrDefault(s => s.Id == songId);

                if (song == null)
                {
                    var response = new ApiResponse<SongDTO>
                    {
                        Data = null,
                        Message = "Song not found",
                        Status = StatusCodes.Status404NotFound
                    };
                    return response;
                }
                else
                {
                    _context.Songs.Remove(song);
                    album.OverallPlayed = album.OverallPlayed - song.TimesPlayed;
                    artist.Listens = artist.Listens - song.TimesPlayed;
                    album.SongCount--;
                    album.AlbumLength = album.AlbumLength - song.Duration;
                    _context.SaveChanges();
                    
                    var response = new ApiResponse<SongDTO>
                    {
                        Data = _mapper.Map<SongDTO>(song),
                        Message = null,
                        Status = StatusCodes.Status200OK
                    };
                    return response;
                }
            }
        }
    }

    public ApiResponse<ArtistDTO> DeleteSongFeatureArtist(Guid artistId, Guid albumId, Guid songId, Guid ftArtistId)
    {
        var artist = _context.Artists
            .Include(x => x.Albums)
            .ThenInclude(x => x.Songs)
            .ThenInclude(x => x.FeaturingArtist)
            .FirstOrDefault(a => a.Id == artistId);

        if (artist == null)
        {
            var response = new ApiResponse<ArtistDTO>
            {
                Data = null,
                Message = "Artist not found",
                Status = StatusCodes.Status404NotFound
            };
            return response;
        }
        else
        {
            var album = artist.Albums.FirstOrDefault(a => a.Id == albumId);

            if (album == null)
            {
                var response = new ApiResponse<ArtistDTO>
                {
                    Data = null,
                    Message = "Album not found",
                    Status = StatusCodes.Status404NotFound
                };
                return response;
            }
            else
            {
                var song = album.Songs.FirstOrDefault(s => s.Id == songId);

                if (song == null)
                {
                    var response = new ApiResponse<ArtistDTO>
                    {
                        Data = null,
                        Message = "Song not found",
                        Status = StatusCodes.Status404NotFound
                    };
                    return response;
                }
                else
                {
                    var ftArtist = song.FeaturingArtist.FirstOrDefault(x => x.Id == ftArtistId);

                    if (ftArtist == null)
                    {
                        var response = new ApiResponse<ArtistDTO>
                        {
                            Data = null,
                            Message = "featuring artist not found",
                            Status = StatusCodes.Status404NotFound
                        };
                        return response;
                    }
                    else
                    {
                        song.FeaturingArtist.Remove(ftArtist);
                        _context.SaveChanges();
                        
                        var response = new ApiResponse<ArtistDTO>
                        {
                            Data = _mapper.Map<ArtistDTO>(ftArtist),
                            Message = null,
                            Status = StatusCodes.Status200OK
                        };
                        return response;
                    }
                }
            }
        }
    }

    public ApiResponse<ArtistDTO> DeletYourselfAsFeatureArtist(Guid ftArtistId, Guid songId)
    {
        var artist = _context.Artists
            .Include(x => x.FeaturingSongs)
            .FirstOrDefault(x => x.Id == ftArtistId);

        if (artist == null)
        {
            var response = new ApiResponse<ArtistDTO>
            {
                Data = null,
                Message = "Artist not found",
                Status = StatusCodes.Status404NotFound
            };
            return response;
        }
        else
        {
            var song = artist.FeaturingSongs.FirstOrDefault(s => s.Id == songId);

            if (song == null)
            {
                var response = new ApiResponse<ArtistDTO>
                {
                    Data = null,
                    Message = "song not found",
                    Status = StatusCodes.Status404NotFound
                };
                return response;
            }
            else
            {
                artist.FeaturingSongs.Remove(song);
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

    public ApiResponse<GenreDTO> DeleteSongGenre(Guid artistId, Guid albumId, Guid songId, Guid genreId)
    {
        var artist = _context.Artists
            .Include(x => x.Albums)
            .ThenInclude(x => x.Songs)
            .ThenInclude(x => x.Genres)
            .FirstOrDefault(a => a.Id == artistId);

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
            var album = artist.Albums.FirstOrDefault(a => a.Id == albumId);

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
                var song = album.Songs.FirstOrDefault(s => s.Id == songId);

                if (song == null)
                {
                    var response = new ApiResponse<GenreDTO>
                    {
                        Data = null,
                        Message = "Song not found",
                        Status = StatusCodes.Status404NotFound
                    };
                    return response;
                }
                else
                {
                    var genre = song.Genres.FirstOrDefault(x => x.Id == genreId);

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
                        song.Genres.Remove(genre);
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

    public ApiResponse<ArtistDTO> DeleteSongComposer(Guid artistId, Guid albumId, Guid songId, Guid composerId)
    {
        var artist = _context.Artists
            .Include(x => x.Albums)
            .ThenInclude(x => x.Songs)
            .ThenInclude(x => x.SongComposers)
            .ThenInclude(x => x.Artist)
            .FirstOrDefault(a => a.Id == artistId);

        if (artist == null)
        {
            var response = new ApiResponse<ArtistDTO>
            {
                Data = null,
                Message = "Artist not found",
                Status = StatusCodes.Status404NotFound
            };
            return response;
        }
        else
        {
            var album = artist.Albums.FirstOrDefault(a => a.Id == albumId);

            if (album == null)
            {
                var response = new ApiResponse<ArtistDTO>
                {
                    Data = null,
                    Message = "Album not found",
                    Status = StatusCodes.Status404NotFound
                };
                return response;
            }
            else
            {
                var song = album.Songs.FirstOrDefault(s => s.Id == songId);

                if (song == null)
                {
                    var response = new ApiResponse<ArtistDTO>
                    {
                        Data = null,
                        Message = "Song not found",
                        Status = StatusCodes.Status404NotFound
                    };
                    return response;
                }
                else
                {
                    var composer = song.SongComposers.FirstOrDefault(x => x.Id == composerId);

                    if (composer == null)
                    {
                        var response = new ApiResponse<ArtistDTO>
                        {
                            Data = null,
                            Message = "featuring artist not found",
                            Status = StatusCodes.Status404NotFound
                        };
                        return response;
                    }
                    else
                    {
                        _context.SongComposers.Remove(composer);
                        _context.SaveChanges();
                        
                        var response = new ApiResponse<ArtistDTO>
                        {
                            Data = _mapper.Map<ArtistDTO>(composer.Artist),
                            Message = null,
                            Status = StatusCodes.Status200OK
                        };
                        return response;
                    }
                }
            }
        }
    }

    public ApiResponse<ArtistDTO> DeleteYourselfAsComposer(Guid artistId, Guid composerId, Guid songId)
    {
        var composer = _context.Artists
            .Include(x => x.ArtistComposedSongs)
            .FirstOrDefault(x => x.Id == artistId);

        if (composer == null)
        {
            var response = new ApiResponse<ArtistDTO>
            {
                Data = null,
                Message = "Artist not found",
                Status = StatusCodes.Status404NotFound
            };
            return response;
        }
        else
        {
            var composing = composer.ArtistComposedSongs.FirstOrDefault(x => x.Id == composerId);

            if (composing == null)
            {
                var response = new ApiResponse<ArtistDTO>
                {
                    Data = null,
                    Message = "composing not found",
                    Status = StatusCodes.Status404NotFound
                };
                return response;
            }
            else
            {
                if (composing.SongId != songId)
                {
                    var response = new ApiResponse<ArtistDTO>
                    {
                        Data = null,
                        Message = "song not found",
                        Status = StatusCodes.Status404NotFound
                    };
                    return response;
                }
                else if (composing.SongId == songId)
                {
                    _context.SongComposers.Remove(composing);
                    _context.SaveChanges();
                    
                    var response = new ApiResponse<ArtistDTO>
                    {
                        Data = _mapper.Map<ArtistDTO>(composer),
                        Message = null,
                        Status = StatusCodes.Status200OK
                    };
                    return response;
                }
                else
                {
                    var response = new ApiResponse<ArtistDTO>
                    {
                        Data = null,
                        Message = "something went wrong",
                        Status = StatusCodes.Status404NotFound
                    };
                    return response;
                }
            }
        }
    }
}