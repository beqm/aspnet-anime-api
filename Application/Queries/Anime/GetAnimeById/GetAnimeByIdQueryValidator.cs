using FluentValidation;

namespace Application.Queries.Anime.GetAnimeById;

public class GetTaskByIdQueryValidator : AbstractValidator<GetAnimeByIdQuery>
{
    public GetTaskByIdQueryValidator()
    {
        RuleFor(x => x.ID)
            .NotEmpty().WithMessage("ID is required.");
    }
}