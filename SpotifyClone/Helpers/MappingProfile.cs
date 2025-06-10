using System.Diagnostics.Contracts;
using SpotifyClone.Models;
using SpotifyClone.Requests;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SpotifyClone.DTOs;

namespace SpotifyClone.Helpers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AddUser, User>().ReverseMap();
        CreateMap<User, UserDTO>().ReverseMap();
        CreateMap<AddUser, UserDTO>().ReverseMap();
        
        CreateMap<AddUserDetails, UserDetails>().ReverseMap();
        CreateMap<UserDetails, UserDetailsDTO>().ReverseMap();
        CreateMap<AddUserDetails, UserDetailsDTO>().ReverseMap();
        
        CreateMap<AddArtist, Artist>().ReverseMap();
        CreateMap<Artist, ArtistDTO>().ReverseMap();
        CreateMap<AddArtist, ArtistDTO>().ReverseMap();
        
        CreateMap<AddArtistDetails, ArtistDetails>().ReverseMap();
        CreateMap<ArtistDetails, ArtistDetailsDTO>().ReverseMap();
        CreateMap<AddArtistDetails, ArtistDetailsDTO>().ReverseMap();
        
        CreateMap<AddAlbum, Album>().ReverseMap();
        CreateMap<Album, AlbumDTO>().ReverseMap();
        CreateMap<AddAlbum, AlbumDTO>().ReverseMap();
        
        CreateMap<AddSong, Song>().ReverseMap();
        CreateMap<Song, SongDTO>().ReverseMap();
        CreateMap<AddSong, SongDTO>().ReverseMap();
        
        CreateMap<AddPlaylist, Playlist>().ReverseMap();
        CreateMap<Playlist, PlaylistDTO>().ReverseMap();
        CreateMap<AddPlaylist, PlaylistDTO>().ReverseMap();

        CreateMap<Genre, GenreDTO>().ReverseMap();
        CreateMap<SongComposers, SongComposersDTO>().ReverseMap();
    }
}