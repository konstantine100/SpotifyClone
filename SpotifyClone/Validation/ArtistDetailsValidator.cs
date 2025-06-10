using FluentValidation;
using SpotifyClone.Models;

namespace SpotifyClone.Validation;

public class ArtistDetailsValidator : AbstractValidator<ArtistDetails>
{
    public ArtistDetailsValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(2000);
        RuleFor(x => x.Website)
            .MaximumLength(1000);
    }
}