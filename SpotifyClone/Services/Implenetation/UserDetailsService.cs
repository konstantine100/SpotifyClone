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

public class UserDetailsService : IUserDetailsService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    
    public UserDetailsService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public ApiResponse<UserDetailsDTO> PostUserDetails(Guid userId, AddUserDetails request)
    {
        var user = _context.Users
            .Include(x => x.UserDetails)
            .FirstOrDefault(x => x.Id == userId);

        if (user == null)
        {
            var response = new ApiResponse<UserDetailsDTO>
            {
                Data = null,
                Message = "user not found",
                Status = StatusCodes.Status404NotFound
            };
            
            return response;
        }
        else
        {
            if (user.UserDetails != null)
            {
                var response = new ApiResponse<UserDetailsDTO>
                {
                    Data = null,
                    Message = "userdetails already exist",
                    Status = StatusCodes.Status403Forbidden
                };
            
                return response;
            }
            else
            {
                var userDetails = _mapper.Map<UserDetails>(request);
                var validator = new UserDetailsValidator();
                var result = validator.Validate(userDetails);

                if (!result.IsValid)
                {
                    var response = new ApiResponse<UserDetailsDTO>
                    {
                        Data = null,
                        Message = "wrong userdetails information",
                        Status = StatusCodes.Status403Forbidden
                    };
            
                    return response;
                }
                else
                {
                    userDetails.UserId = user.Id;
                    user.UserDetails = userDetails;
                    _context.SaveChanges();
                    
                    var response = new ApiResponse<UserDetailsDTO>
                    {
                        Data = _mapper.Map<UserDetailsDTO>(userDetails),
                        Message = null,
                        Status = StatusCodes.Status200OK
                    };
            
                    return response;
                }
            }
            
        }
    }

    public ApiResponse<List<UserDetailsDTO>> SearchUsers(string search)
    {
        var users = _context.UserDetails
            .Where(x => x.Username.ToLower().Contains(search.ToLower()))
            .ToList();
        
        var response = new ApiResponse<List<UserDetailsDTO>>
        {
            Data = _mapper.Map<List<UserDetailsDTO>>(users),
            Message = null,
            Status = StatusCodes.Status200OK
        };
            
        return response;
    }

    public ApiResponse<UserDetailsDTO> ChangeUserDetails(Guid userId, string changeParametr, string changeTo)
    {
        var user = _context.UserDetails
            .FirstOrDefault(x => x.Id == userId);

        if (user == null)
        {
            var response = new ApiResponse<UserDetailsDTO>
            {
                Data = null,
                Message = "user not found",
                Status = StatusCodes.Status404NotFound
            };
            
            return response;
        }
        else
        {
            if (changeParametr.ToLower() == "username")
            {
                user.Username = changeTo;
                var validator = new UserDetailsValidator();
                var result = validator.Validate(user);

                if (!result.IsValid)
                {
                    var response = new ApiResponse<UserDetailsDTO>
                    {
                        Data = null,
                        Message = "wrong userdetails information",
                        Status = StatusCodes.Status403Forbidden
                    };
            
                    return response;
                }
                else
                {
                    _context.SaveChanges();
                    
                    var response = new ApiResponse<UserDetailsDTO>
                    {
                        Data = _mapper.Map<UserDetailsDTO>(user),
                        Message = null,
                        Status = StatusCodes.Status200OK
                    };
            
                    return response;
                }
            }
            else if (changeParametr.ToLower() == "profilepicture")
            {
                user.ProfilePicture = changeTo;
                var validator = new UserDetailsValidator();
                var result = validator.Validate(user);

                if (!result.IsValid)
                {
                    var response = new ApiResponse<UserDetailsDTO>
                    {
                        Data = null,
                        Message = "wrong userdetails information",
                        Status = StatusCodes.Status403Forbidden
                    };
            
                    return response;
                }
                else
                {
                    _context.SaveChanges();
                    
                    var response = new ApiResponse<UserDetailsDTO>
                    {
                        Data = _mapper.Map<UserDetailsDTO>(user),
                        Message = null,
                        Status = StatusCodes.Status200OK
                    };
            
                    return response;
                }
            }
            else
            {
                var response = new ApiResponse<UserDetailsDTO>
                {
                    Data = null,
                    Message = "wrong change parametr",
                    Status = StatusCodes.Status403Forbidden
                };
            
                return response;
            }
        }
    }

    public ApiResponse<UserDetailsDTO> DeleteUserDetails(Guid userId)
    {
        var user = _context.UserDetails
            .FirstOrDefault(x => x.Id == userId);

        if (user == null)
        {
            var response = new ApiResponse<UserDetailsDTO>
            {
                Data = null,
                Message = "user not found",
                Status = StatusCodes.Status404NotFound
            };
            
            return response;
        }
        else
        {
            _context.UserDetails.Remove(user);
            _context.SaveChanges();
            
            var response = new ApiResponse<UserDetailsDTO>
            {
                Data = _mapper.Map<UserDetailsDTO>(user),
                Message = null,
                Status = StatusCodes.Status200OK
            };
            
            return response;
        }
    }
}