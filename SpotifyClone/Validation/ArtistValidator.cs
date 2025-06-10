using FluentValidation;
using SpotifyClone.Models;

namespace SpotifyClone.Validation;

public class ArtistValidator : AbstractValidator<Artist>
{
    public ArtistValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);
        RuleFor(x => x.ProfilePicture)
            .NotEmpty()
            .MaximumLength(100000);
    }
}