using MediatR;
using FluentResults;
using Application.Dtos;

namespace Application.Commands.Anime.UpdateAnime;

public record UpdateAnimeCommand(int ID, string? Title, string? Author, string? Description) : IRequest<Result<AnimeDto>>;