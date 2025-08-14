using FluentValidation;

namespace Application.Queries.Anime.GetAnimeList;

public class GetTaskListQueryValidator : AbstractValidator<GetAnimeListQuery>
{
    public GetTaskListQueryValidator()
    {
    }
}