using FluentValidation;
using SpotifyClone.Models;

namespace SpotifyClone.Validation;

public class SongValidator : AbstractValidator<Song>
{
    public SongValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);
        RuleFor(x => x.Lyrics)
            .NotEmpty()
            .MaximumLength(10000);
        RuleFor(x => x.Duration)
            .NotEmpty()
            .GreaterThan(TimeSpan.Zero)
            .LessThanOrEqualTo(TimeSpan.FromHours(3));
        RuleFor(x => x.TrackNumber)
            .NotEmpty()
            .GreaterThan(0)
            .LessThanOrEqualTo(5000);
    }
}