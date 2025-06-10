using FluentValidation;
using SpotifyClone.Models;

namespace SpotifyClone.Validation;

public class UserDetailsValidator : AbstractValidator<UserDetails>
{
    public UserDetailsValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .Length(2, 50);
        RuleFor(x => x.ProfilePicture)
            .MaximumLength(100000);
    }
}