using MediatR;
using FluentResults;
using Application.Dtos;

namespace Application.Queries.Anime.GetAnimeById;

public record GetAnimeByIdQuery(int ID) : IRequest<Result<AnimeDto?>>;