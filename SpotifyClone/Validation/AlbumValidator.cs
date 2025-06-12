using FluentValidation;
using SpotifyClone.Models;

namespace SpotifyClone.Validation;

public class AlbumValidator : AbstractValidator<Album>
{
    public AlbumValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);
        RuleFor(x => x.AlbumCover)
            .NotEmpty()
            .MaximumLength(100000);
        RuleFor(x => x.ReleaseYear)
            .NotEmpty()
            .LessThanOrEqualTo(DateTime.Now.Year);
    }
}