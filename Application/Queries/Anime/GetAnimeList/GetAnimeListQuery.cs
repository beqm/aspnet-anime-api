using MediatR;
using FluentResults;
using Application.Dtos;
namespace Application.Queries.Anime.GetAnimeList;

public record GetAnimeListQuery(string? Author = null, string? Description = null) : IRequest<Result<List<AnimeDto>>>;