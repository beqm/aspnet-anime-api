using FluentValidation;

namespace Application.Commands.Anime.UpdateAnime;

public class UpdateAnimeValidator : AbstractValidator<UpdateAnimeCommand>
{
    public UpdateAnimeValidator()
    {
        RuleFor(x => x.ID)
            .GreaterThan(0)
            .NotEmpty().WithMessage("ID is required.");
    }
}