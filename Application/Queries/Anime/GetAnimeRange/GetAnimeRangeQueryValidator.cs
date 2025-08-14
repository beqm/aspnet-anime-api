using FluentValidation;

namespace Application.Queries.Anime.GetAnimeRange;

public class GetTaskRangeQueryValidator : AbstractValidator<GetAnimeRangeQuery>
{
    public GetTaskRangeQueryValidator()
    {
        RuleFor(x => x.start)
            .NotEmpty().WithMessage("Start is required.")
            .GreaterThan(0).WithMessage("Start must be greater than 0.");

        RuleFor(x => x.end)
            .NotEmpty().WithMessage("End is required.")
            .GreaterThan(0).WithMessage("End must be greater than 0.")
            .Must((query, end) => end >= query.start)
                .WithMessage("End must be greater than or equal to Start.");
    }
}