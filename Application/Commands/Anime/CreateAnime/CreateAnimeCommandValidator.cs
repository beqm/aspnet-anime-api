using FluentValidation;

namespace Application.Commands.Anime.CreateAnime;

public class CreateAnimeValidator : AbstractValidator<CreateAnimeCommand>
{
    public CreateAnimeValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.");

        RuleFor(x => x.Author)
            .NotEmpty().WithMessage("Author is required.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.");

        RuleFor(x => x.ID)
            .NotEmpty().WithMessage("ID is required.");
    }
}