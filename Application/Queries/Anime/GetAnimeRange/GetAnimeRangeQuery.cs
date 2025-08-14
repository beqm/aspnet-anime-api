using MediatR;
using Application.Dtos;

namespace Application.Queries.Anime.GetAnimeRange;

public record GetAnimeRangeQuery(int start, int end) : IRequest<IEnumerable<AnimeDto>>;