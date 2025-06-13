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

public class PlaylistService : IPlaylistService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    
    public PlaylistService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public ApiResponse<PlaylistDTO> AddPlaylist(Guid? userId, Guid? artistId, AddPlaylist request)
    {
        if (userId == null && artistId == null)
        {
            var response = new ApiResponse<PlaylistDTO>
            {
                Data = null,
                Message = "can't find user/artist",
                Status = StatusCodes.Status400BadRequest
            };
            return response;
        }
        else if (!string.IsNullOrEmpty(userId.ToString()) && !string.IsNullOrEmpty(artistId.ToString()))
        {
            var response = new ApiResponse<PlaylistDTO>
            {
                Data = null,
                Message = "can't access both user and artist",
                Status = StatusCodes.Status400BadRequest
            };
            return response;
        }
        else if (!string.IsNullOrEmpty(userId.ToString()) && string.IsNullOrEmpty(artistId.ToString()))
        {
            var user = _context.UserDetails
                .Include(x => x.Playlists)
                .FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                var response = new ApiResponse<PlaylistDTO>
                {
                    Data = null,
                    Message = "user not found",
                    Status = StatusCodes.Status404NotFound
                };
                return response;
            }
            else
            {
                var playlist = _mapper.Map<Playlist>(request);
                var validator = new PlaylistValidator();
                var result = validator.Validate(playlist);

                if (!result.IsValid)
                {
                    var response = new ApiResponse<PlaylistDTO>
                    {
                        Data = null,
                        Message = "wrong playlist information",
                        Status = StatusCodes.Status403Forbidden
                    };
                    return response;
                }
                else
                {
                    playlist.UserDetailsId = user.Id;
                    user.Playlists.Add(playlist);
                    _context.SaveChanges();
                    
                    var response = new ApiResponse<PlaylistDTO>
                    {
                        Data = _mapper.Map<PlaylistDTO>(playlist),
                        Message = null,
                        Status = StatusCodes.Status200OK
                    };
                    return response;
                }
            }
        }
        else if (string.IsNullOrEmpty(userId.ToString()) && !string.IsNullOrEmpty(artistId.ToString()))
        {
            var artist = _context.Artists
                .Include(x => x.Playlists)
                .FirstOrDefault(u => u.Id == artistId);

            if (artist == null)
            {
                var response = new ApiResponse<PlaylistDTO>
                {
                    Data = null,
                    Message = "artist not found",
                    Status = StatusCodes.Status404NotFound
                };
                return response;
            }
            else
            {
                var playlist = _mapper.Map<Playlist>(request);
                var validator = new PlaylistValidator();
                var result = validator.Validate(playlist);

                if (!result.IsValid)
                {
                    var response = new ApiResponse<PlaylistDTO>
                    {
                        Data = null,
                        Message = "wrong playlist information",
                        Status = StatusCodes.Status403Forbidden
                    };
                    return response;
                }
                else
                {
                    playlist.ArtistId = artist.Id;
                    artist.Playlists.Add(playlist);
                    _context.SaveChanges();
                    
                    var response = new ApiResponse<PlaylistDTO>
                    {
                        Data = _mapper.Map<PlaylistDTO>(playlist),
                        Message = null,
                        Status = StatusCodes.Status200OK
                    };
                    return response;
                }
            }
        }
        else
        {
            var response = new ApiResponse<PlaylistDTO>
            {
                Data = null,
                Message = "unexpected error",
                Status = StatusCodes.Status400BadRequest
            };
            return response;
        }
    }

    public ApiResponse<SongDTO> AddPlaylistSong(Guid? userId, Guid? artistId, Guid playlistId, Guid songId)
    {
        if (userId == null && artistId == null)
        {
            var response = new ApiResponse<SongDTO>
            {
                Data = null,
                Message = "can't find user/artist",
                Status = StatusCodes.Status400BadRequest
            };
            return response;
        }
        else if (!string.IsNullOrEmpty(userId.ToString()) && !string.IsNullOrEmpty(artistId.ToString()))
        {
            var response = new ApiResponse<SongDTO>
            {
                Data = null,
                Message = "can't access both user and artist",
                Status = StatusCodes.Status400BadRequest
            };
            return response;
        }
        else if (!string.IsNullOrEmpty(userId.ToString()) && string.IsNullOrEmpty(artistId.ToString()))
        {
            var user = _context.UserDetails
                .Include(x => x.Playlists)
                .ThenInclude(x => x.Songs)
                .FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                var response = new ApiResponse<SongDTO>
                {
                    Data = null,
                    Message = "user not found",
                    Status = StatusCodes.Status404NotFound
                };
                return response;
            }
            else
            {
                var playlist = user.Playlists.FirstOrDefault(x => x.Id == playlistId);

                if (playlist == null)
                {
                    var response = new ApiResponse<SongDTO>
                    {
                        Data = null,
                        Message = "playlist not found",
                        Status = StatusCodes.Status404NotFound
                    };
                    return response;
                }
                else
                {
                    var song = _context.Songs.FirstOrDefault(x => x.Id == songId);

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
                        playlist.Songs.Add(song);
                        playlist.SongCount++;
                        playlist.AlbumLength = playlist.AlbumLength + song.Duration;
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
        else if (string.IsNullOrEmpty(userId.ToString()) && !string.IsNullOrEmpty(artistId.ToString()))
        {
            var artist = _context.Artists
                .Include(x => x.Playlists)
                .ThenInclude(x => x.Songs)
                .FirstOrDefault(u => u.Id == artistId);

            if (artist == null)
            {
                var response = new ApiResponse<SongDTO>
                {
                    Data = null,
                    Message = "artist not found",
                    Status = StatusCodes.Status404NotFound
                };
                return response;
            }
            else
            {
                var playlist = artist.Playlists.FirstOrDefault(x => x.Id == playlistId);

                if (playlist == null)
                {
                    var response = new ApiResponse<SongDTO>
                    {
                        Data = null,
                        Message = "playlist not found",
                        Status = StatusCodes.Status404NotFound
                    };
                    return response;
                }
                else
                {
                    var song = _context.Songs.FirstOrDefault(x => x.Id == songId);

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
                        playlist.Songs.Add(song);
                        playlist.SongCount++;
                        playlist.AlbumLength = playlist.AlbumLength + song.Duration;
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
        else
        {
            var response = new ApiResponse<SongDTO>
            {
                Data = null,
                Message = "unexpected error",
                Status = StatusCodes.Status400BadRequest
            };
            return response;
        }
    }

    public ApiResponse<PlaylistDTO> CombinePlaylist(Guid userId, Guid firstPlaylistId, Guid secondPlaylistId)
    {
        var user = _context.UserDetails
            .Include(x => x.Playlists)
            .ThenInclude(x => x.Songs)
            .FirstOrDefault(x => x.Id == userId);

        if (user == null)
        {
            var response = new ApiResponse<PlaylistDTO>
            {
                Data = null,
                Message = "user not found",
                Status = StatusCodes.Status404NotFound
            };
            return response;
        }
        else
        {
            var firstPlaylist = _context.Playlists
                .Include(x => x.Songs)
                .FirstOrDefault(x => x.Id == firstPlaylistId);

            if (firstPlaylist == null)
            {
                var response = new ApiResponse<PlaylistDTO>
                {
                    Data = null,
                    Message = "playlist not found",
                    Status = StatusCodes.Status404NotFound
                };
                return response;
            }
            else
            {
                var secondPlaylist = _context.Playlists
                    .Include(x => x.Songs)
                    .FirstOrDefault(x => x.Id == secondPlaylistId);

                if (secondPlaylist == null)
                {
                    var response = new ApiResponse<PlaylistDTO>
                    {
                        Data = null,
                        Message = "playlist not found",
                        Status = StatusCodes.Status404NotFound
                    };
                    return response;
                }
                else
                {
                    TimeSpan allTime = TimeSpan.Zero;
                    int songCount = 0;
                    var songs = new List<Song>();
                    songs.AddRange(firstPlaylist.Songs);
                    songs.AddRange(secondPlaylist.Songs);

                    foreach (var song in songs)
                    {
                        songCount++;
                        allTime += song.Duration;
                    }


                    var newPlaylist = new Playlist
                    {
                        Title = $"{firstPlaylist.Title} {secondPlaylist.Title} combined",
                        PlaylistPicture = "kitxvisNishani.jpeg",
                        AlbumLength = allTime,
                        SongCount = songCount,
                        UserDetailsId = userId,
                        Songs = songs
                    };
                    
                    user.Playlists.Add(newPlaylist);
                    _context.SaveChanges();

                    var response = new ApiResponse<PlaylistDTO>
                    {
                        Data = _mapper.Map<PlaylistDTO>(newPlaylist),
                        Message = null,
                        Status = StatusCodes.Status200OK,
                    };
                    return response;
                }
            }
        }
    }

    public ApiResponse<PlaylistDTO> IntersecedPlaylist(Guid userId, Guid firstPlaylistId, Guid secondPlaylistId)
    {
        var user = _context.UserDetails
            .Include(x => x.Playlists)
            .ThenInclude(x => x.Songs)
            .FirstOrDefault(x => x.Id == userId);

        if (user == null)
        {
            var response = new ApiResponse<PlaylistDTO>
            {
                Data = null,
                Message = "user not found",
                Status = StatusCodes.Status404NotFound
            };
            return response;
        }
        else
        {
            var firstPlaylist = _context.Playlists
                .Include(x => x.Songs)
                .FirstOrDefault(x => x.Id == firstPlaylistId);

            if (firstPlaylist == null)
            {
                var response = new ApiResponse<PlaylistDTO>
                {
                    Data = null,
                    Message = "playlist not found",
                    Status = StatusCodes.Status404NotFound
                };
                return response;
            }
            else
            {
                var secondPlaylist = _context.Playlists
                    .Include(x => x.Songs)
                    .FirstOrDefault(x => x.Id == secondPlaylistId);

                if (secondPlaylist == null)
                {
                    var response = new ApiResponse<PlaylistDTO>
                    {
                        Data = null,
                        Message = "playlist not found",
                        Status = StatusCodes.Status404NotFound
                    };
                    return response;
                }
                else
                {
                    TimeSpan allTime = TimeSpan.Zero;
                    int songCount = 0;
                    var songs = firstPlaylist.Songs.Intersect(secondPlaylist.Songs).ToList();

                    foreach (var song in songs)
                    {
                        songCount++;
                        allTime += song.Duration;
                    }


                    var newPlaylist = new Playlist
                    {
                        Title = $"{firstPlaylist.Title} {secondPlaylist.Title} intersected",
                        PlaylistPicture = "kitxvisNishani.jpeg",
                        AlbumLength = allTime,
                        SongCount = songCount,
                        Songs = songs,
                        UserDetailsId = userId
                    };
                    
                    user.Playlists.Add(newPlaylist);
                    _context.SaveChanges();

                    var response = new ApiResponse<PlaylistDTO>
                    {
                        Data = _mapper.Map<PlaylistDTO>(newPlaylist),
                        Message = null,
                        Status = StatusCodes.Status200OK,
                    };
                    return response;
                }
            }
        }
    }

    public ApiResponse<List<PlaylistDTO>> GetAllPlaylists()
    {
        var playlists = _context.Playlists
            .Include(x => x.Songs)
            .ToList();

        var response = new ApiResponse<List<PlaylistDTO>>
        {
            Data = _mapper.Map<List<PlaylistDTO>>(playlists),
            Message = null,
            Status = StatusCodes.Status200OK,
        };
        return response;
    }

    public ApiResponse<List<PlaylistDTO>> GetPlaylistsFinder(string? search, string? sortBy)
    {
        var playlist = _context.Playlists
            .Include(x => x.Songs)
            .ToList();

        if (!string.IsNullOrWhiteSpace(search))
        {
            playlist = playlist
                .Where(x => x.Title.ToLower().Contains(search.ToLower()) ||
                                  x.Songs.Any(x => x.Title.ToLower().Contains(search.ToLower())) ||
                                  x.Songs.Any(x => x.Lyrics.ToLower().Contains(search.ToLower())))
                .ToList();
        }
        
        if(!string.IsNullOrWhiteSpace(sortBy))
        {
            switch (sortBy.ToLower())
            {
                case "name_asc":
                    playlist = playlist.OrderBy(x => x.Title).ToList();
                    break;
                case "name_desc":
                    playlist = playlist.OrderByDescending(x => x.Title).ToList();
                    break;
                case "song_count_asc":
                    playlist = playlist.OrderBy(x => x.SongCount).ToList();
                    break;
                case "song_count_desc":
                    playlist = playlist.OrderByDescending(x => x.SongCount).ToList();
                    break;
                case "length_asc":
                    playlist = playlist.OrderBy(x => x.AlbumLength).ToList();
                    break;
                case "length_desc":
                    playlist = playlist.OrderByDescending(x => x.AlbumLength).ToList();
                    break;
            }
        }

        var response = new ApiResponse<List<PlaylistDTO>>
        {
            Data = _mapper.Map<List<PlaylistDTO>>(playlist),
            Message = null,
            Status = StatusCodes.Status200OK,
        };
        return response;
    }

    public ApiResponse<List<PlaylistDTO>> GetArtistPlaylists(Guid? artistId)
    {
        var artist = _context.Artists
            .Include(x => x.Playlists)
            .ThenInclude(x => x.Songs)
            .FirstOrDefault(x => x.Id == artistId);

        if (artist == null)
        {
            var response = new ApiResponse<List<PlaylistDTO>>
            {
                Data = null,
                Message = "artist not found",
                Status = StatusCodes.Status404NotFound
            };
            return response;
        }
        else
        {
            var response = new ApiResponse<List<PlaylistDTO>>
            {
                Data = _mapper.Map<List<PlaylistDTO>>(artist.Playlists),
                Message = null,
                Status = StatusCodes.Status200OK
            };
            return response;
        }
    }

    public ApiResponse<List<PlaylistDTO>> GetUserPlaylists(Guid? userId)
    {
        var user = _context.UserDetails
            .Include(x => x.Playlists)
            .ThenInclude(x => x.Songs)
            .FirstOrDefault(x => x.Id == userId);

        if (user == null)
        {
            var response = new ApiResponse<List<PlaylistDTO>>
            {
                Data = null,
                Message = "user not found",
                Status = StatusCodes.Status404NotFound
            };
            return response;
        }
        else
        {
            var response = new ApiResponse<List<PlaylistDTO>>
            {
                Data = _mapper.Map<List<PlaylistDTO>>(user.Playlists),
                Message = null,
                Status = StatusCodes.Status200OK
            };
            return response;
        }
    }

    public ApiResponse<SongDTO> PlayPlaylistSong(Guid playlistId, Guid? songId)
    {
        var playlist = _context.Playlists
            .Include(x => x.Songs)
            .ThenInclude(x => x.Album)
            .ThenInclude(x => x.Artist)
            .FirstOrDefault(x => x.Id == playlistId);

        if (playlist == null)
        {
            var response = new ApiResponse<SongDTO>
            {
                Data = null,
                Message = "playlist not found",
                Status = StatusCodes.Status404NotFound
            };
            return response;
        }
        else
        {
            var song = playlist.Songs.FirstOrDefault(x => x.Id == songId);

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
                playlist.OverallPlayed++;
                
                var response = new ApiResponse<SongDTO>
                {
                    Data = _mapper.Map<SongDTO>(song),
                    Message = $"now playing: {song.Album.Artist.Name} - {song.Title} [{song.Duration}] from playlist: {playlist.Title}",
                    Status = StatusCodes.Status200OK
                };
                return response;
            }
        }
    }

    public ApiResponse<PlaylistDTO> GetPlaylist(Guid playlistId)
    {
        var playlist = _context.Playlists
            .Include(x => x.Songs)
            .FirstOrDefault(x => x.Id == playlistId);

        if (playlist == null)
        {
            var response = new ApiResponse<PlaylistDTO>
            {
                Data = null,
                Message = "playlist not found",
                Status = StatusCodes.Status404NotFound
            };
            return response;
        }
        else
        {
            var response = new ApiResponse<PlaylistDTO>
            {
                Data = _mapper.Map<PlaylistDTO>(playlist),
                Message = null,
                Status = StatusCodes.Status200OK
            };
            return response;
        }
    }

    public ApiResponse<PlaylistDTO> ChangePlaylist(Guid? userId, Guid? artistId, Guid playlistId, string changeParametr, string changeTo)
    {
        if (userId == null && artistId == null)
        {
            var response = new ApiResponse<PlaylistDTO>
            {
                Data = null,
                Message = "can't find user/artist",
                Status = StatusCodes.Status400BadRequest
            };
            return response;
        }
        else if (!string.IsNullOrEmpty(userId.ToString()) && !string.IsNullOrEmpty(artistId.ToString()))
        {
            var response = new ApiResponse<PlaylistDTO>
            {
                Data = null,
                Message = "can't access both user and artist",
                Status = StatusCodes.Status400BadRequest
            };
            return response;
        }
        else if (!string.IsNullOrEmpty(userId.ToString()) && string.IsNullOrEmpty(artistId.ToString()))
        {
            var user = _context.UserDetails
                .Include(x => x.Playlists)
                .FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                var response = new ApiResponse<PlaylistDTO>
                {
                    Data = null,
                    Message = "user not found",
                    Status = StatusCodes.Status404NotFound
                };
                return response;
            }
            else
            {
                var playlist = user.Playlists.FirstOrDefault(x => x.Id == playlistId);

                if (playlist == null)
                {
                    var response = new ApiResponse<PlaylistDTO>
                    {
                        Data = null,
                        Message = "playlist not found",
                        Status = StatusCodes.Status404NotFound
                    };
                    return response;
                }
                else
                {
                    if (changeParametr.ToLower() == "title")
                    {
                        playlist.Title = changeTo;
                        var validator = new PlaylistValidator();
                        var result = validator.Validate(playlist);

                        if (!result.IsValid)
                        {
                            var response = new ApiResponse<PlaylistDTO>
                            {
                                Data = null,
                                Message = "wrong playlist information",
                                Status = StatusCodes.Status403Forbidden
                            };
                            return response;
                        }
                        else
                        {
                            _context.SaveChanges();
                            
                            var response = new ApiResponse<PlaylistDTO>
                            {
                                Data = _mapper.Map<PlaylistDTO>(playlist),
                                Message = null,
                                Status = StatusCodes.Status200OK
                            };
                            return response;
                        }
                    }
                    else if (changeParametr.ToLower() == "picture")
                    {
                        playlist.PlaylistPicture = changeTo;
                        var validator = new PlaylistValidator();
                        var result = validator.Validate(playlist);

                        if (!result.IsValid)
                        {
                            var response = new ApiResponse<PlaylistDTO>
                            {
                                Data = null,
                                Message = "wrong playlist information",
                                Status = StatusCodes.Status403Forbidden
                            };
                            return response;
                        }
                        else
                        {
                            _context.SaveChanges();
                            
                            var response = new ApiResponse<PlaylistDTO>
                            {
                                Data = _mapper.Map<PlaylistDTO>(playlist),
                                Message = null,
                                Status = StatusCodes.Status200OK
                            };
                            return response;
                        }
                    }
                    else
                    {
                        var response = new ApiResponse<PlaylistDTO>
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
        else if (string.IsNullOrEmpty(userId.ToString()) && !string.IsNullOrEmpty(artistId.ToString()))
        {
            var artist = _context.Artists
                .Include(x => x.Playlists)
                .FirstOrDefault(u => u.Id == artistId);

            if (artist == null)
            {
                var response = new ApiResponse<PlaylistDTO>
                {
                    Data = null,
                    Message = "artist not found",
                    Status = StatusCodes.Status404NotFound
                };
                return response;
            }
            else
            {
                var playlist = artist.Playlists.FirstOrDefault(x => x.Id == playlistId);

                if (playlist == null)
                {
                    var response = new ApiResponse<PlaylistDTO>
                    {
                        Data = null,
                        Message = "playlist not found",
                        Status = StatusCodes.Status404NotFound
                    };
                    return response;
                }
                else
                {
                    if (changeParametr.ToLower() == "title")
                    {
                        playlist.Title = changeTo;
                        var validator = new PlaylistValidator();
                        var result = validator.Validate(playlist);

                        if (!result.IsValid)
                        {
                            var response = new ApiResponse<PlaylistDTO>
                            {
                                Data = null,
                                Message = "wrong playlist information",
                                Status = StatusCodes.Status403Forbidden
                            };
                            return response;
                        }
                        else
                        {
                            _context.SaveChanges();
                            
                            var response = new ApiResponse<PlaylistDTO>
                            {
                                Data = _mapper.Map<PlaylistDTO>(playlist),
                                Message = null,
                                Status = StatusCodes.Status200OK
                            };
                            return response;
                        }
                    }
                    else if (changeParametr.ToLower() == "picture")
                    {
                        playlist.PlaylistPicture = changeTo;
                        var validator = new PlaylistValidator();
                        var result = validator.Validate(playlist);

                        if (!result.IsValid)
                        {
                            var response = new ApiResponse<PlaylistDTO>
                            {
                                Data = null,
                                Message = "wrong playlist information",
                                Status = StatusCodes.Status403Forbidden
                            };
                            return response;
                        }
                        else
                        {
                            _context.SaveChanges();
                            
                            var response = new ApiResponse<PlaylistDTO>
                            {
                                Data = _mapper.Map<PlaylistDTO>(playlist),
                                Message = null,
                                Status = StatusCodes.Status200OK
                            };
                            return response;
                        }
                    }
                    else
                    {
                        var response = new ApiResponse<PlaylistDTO>
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
        else
        {
            var response = new ApiResponse<PlaylistDTO>
            {
                Data = null,
                Message = "unexpected error",
                Status = StatusCodes.Status400BadRequest
            };
            return response;
        }
    }

    public ApiResponse<PlaylistDTO> DeletePlaylist(Guid? userId, Guid? artistId, Guid playlistId)
    {
        if (userId == null && artistId == null)
        {
            var response = new ApiResponse<PlaylistDTO>
            {
                Data = null,
                Message = "can't find user/artist",
                Status = StatusCodes.Status400BadRequest
            };
            return response;
        }
        else if (!string.IsNullOrEmpty(userId.ToString()) && !string.IsNullOrEmpty(artistId.ToString()))
        {
            var response = new ApiResponse<PlaylistDTO>
            {
                Data = null,
                Message = "can't access both user and artist",
                Status = StatusCodes.Status400BadRequest
            };
            return response;
        }
        else if (!string.IsNullOrEmpty(userId.ToString()) && string.IsNullOrEmpty(artistId.ToString()))
        {
            var user = _context.UserDetails
                .Include(x => x.Playlists)
                .FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                var response = new ApiResponse<PlaylistDTO>
                {
                    Data = null,
                    Message = "user not found",
                    Status = StatusCodes.Status404NotFound
                };
                return response;
            }
            else
            {
                var playlist = user.Playlists.FirstOrDefault(x => x.Id == playlistId);

                if (playlist == null)
                {
                    var response = new ApiResponse<PlaylistDTO>
                    {
                        Data = null,
                        Message = "playlist not found",
                        Status = StatusCodes.Status404NotFound
                    };
                    return response;
                }
                else
                {
                    _context.Playlists.Remove(playlist);
                    _context.SaveChanges();
                    
                    var response = new ApiResponse<PlaylistDTO>
                    {
                        Data = _mapper.Map<PlaylistDTO>(playlist),
                        Message = null,
                        Status = StatusCodes.Status200OK
                    };
                    return response;
                }
            }
        }
        else if (string.IsNullOrEmpty(userId.ToString()) && !string.IsNullOrEmpty(artistId.ToString()))
        {
            var artist = _context.Artists
                .Include(x => x.Playlists)
                .FirstOrDefault(u => u.Id == artistId);

            if (artist == null)
            {
                var response = new ApiResponse<PlaylistDTO>
                {
                    Data = null,
                    Message = "artist not found",
                    Status = StatusCodes.Status404NotFound
                };
                return response;
            }
            else
            {
                var playlist = artist.Playlists.FirstOrDefault(x => x.Id == playlistId);

                if (playlist == null)
                {
                    var response = new ApiResponse<PlaylistDTO>
                    {
                        Data = null,
                        Message = "playlist not found",
                        Status = StatusCodes.Status404NotFound
                    };
                    return response;
                }
                else
                {
                    _context.Playlists.Remove(playlist);
                    _context.SaveChanges();
                    
                    var response = new ApiResponse<PlaylistDTO>
                    {
                        Data = _mapper.Map<PlaylistDTO>(playlist),
                        Message = null,
                        Status = StatusCodes.Status200OK
                    };
                    return response;
                }
            }
        }
        else
        {
            var response = new ApiResponse<PlaylistDTO>
            {
                Data = null,
                Message = "unexpected error",
                Status = StatusCodes.Status400BadRequest
            };
            return response;
        }
    }

    public ApiResponse<SongDTO> DeletePlaylistSong(Guid? userId, Guid? artistId, Guid playlistId, Guid songId)
    {
        if (userId == null && artistId == null)
        {
            var response = new ApiResponse<SongDTO>
            {
                Data = null,
                Message = "can't find user/artist",
                Status = StatusCodes.Status400BadRequest
            };
            return response;
        }
        else if (!string.IsNullOrEmpty(userId.ToString()) && !string.IsNullOrEmpty(artistId.ToString()))
        {
            var response = new ApiResponse<SongDTO>
            {
                Data = null,
                Message = "can't access both user and artist",
                Status = StatusCodes.Status400BadRequest
            };
            return response;
        }
        else if (!string.IsNullOrEmpty(userId.ToString()) && string.IsNullOrEmpty(artistId.ToString()))
        {
            var user = _context.UserDetails
                .Include(x => x.Playlists)
                .ThenInclude(x => x.Songs)
                .FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                var response = new ApiResponse<SongDTO>
                {
                    Data = null,
                    Message = "user not found",
                    Status = StatusCodes.Status404NotFound
                };
                return response;
            }
            else
            {
                var playlist = user.Playlists.FirstOrDefault(x => x.Id == playlistId);

                if (playlist == null)
                {
                    var response = new ApiResponse<SongDTO>
                    {
                        Data = null,
                        Message = "playlist not found",
                        Status = StatusCodes.Status404NotFound
                    };
                    return response;
                }
                else
                {
                    var song = playlist.Songs.FirstOrDefault(x => x.Id == songId);

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
                        playlist.Songs.Remove(song);
                        playlist.SongCount = playlist.SongCount - 1;
                        playlist.AlbumLength = playlist.AlbumLength - song.Duration;
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
        else if (string.IsNullOrEmpty(userId.ToString()) && !string.IsNullOrEmpty(artistId.ToString()))
        {
            var artist = _context.Artists
                .Include(x => x.Playlists)
                .ThenInclude(x => x.Songs)
                .FirstOrDefault(u => u.Id == artistId);

            if (artist == null)
            {
                var response = new ApiResponse<SongDTO>
                {
                    Data = null,
                    Message = "artist not found",
                    Status = StatusCodes.Status404NotFound
                };
                return response;
            }
            else
            {
                var playlist = artist.Playlists.FirstOrDefault(x => x.Id == playlistId);

                if (playlist == null)
                {
                    var response = new ApiResponse<SongDTO>
                    {
                        Data = null,
                        Message = "playlist not found",
                        Status = StatusCodes.Status404NotFound
                    };
                    return response;
                }
                else
                {
                    var song = playlist.Songs.FirstOrDefault(x => x.Id == songId);

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
                        playlist.Songs.Remove(song);
                        playlist.SongCount = playlist.SongCount - 1;
                        playlist.AlbumLength = playlist.AlbumLength - song.Duration;
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
        else
        {
            var response = new ApiResponse<SongDTO>
            {
                Data = null,
                Message = "unexpected error",
                Status = StatusCodes.Status400BadRequest
            };
            return response;
        }
    }
}