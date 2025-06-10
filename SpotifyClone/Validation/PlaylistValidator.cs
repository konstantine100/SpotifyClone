using FluentValidation;
using SpotifyClone.Models;

namespace SpotifyClone.Validation;

public class PlaylistValidator : AbstractValidator<Playlist>
{
    public PlaylistValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);
        RuleFor(x => x.PlaylistPicture)
            .MaximumLength(10000);
    }
}