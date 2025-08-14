using MediatR;
using Application.Dtos;

namespace Application.Queries.Anime.GetAnimeByTitle;

public record GetAnimeByTitleQuery(string Title) : IRequest<AnimeDto?>;