using FluentValidation;

namespace Application.Commands.Anime.DeleteAnime;

public class DeleteAnimeValidator : AbstractValidator<DeleteAnimeCommand>
{
    public DeleteAnimeValidator()
    {
        RuleFor(x => x.ID)
            .GreaterThan(0)
            .NotEmpty().WithMessage("ID is required.");

    }
}