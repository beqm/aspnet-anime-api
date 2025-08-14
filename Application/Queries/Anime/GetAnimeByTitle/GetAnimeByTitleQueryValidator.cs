using FluentValidation;

namespace Application.Queries.Anime.GetAnimeByTitle;

public class GetTaskByTitleQueryValidator : AbstractValidator<GetAnimeByTitleQuery>
{
    public GetTaskByTitleQueryValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.");
    }
}