using MediatR;
using FluentResults;

namespace Application.Commands.Anime.DeleteAnime;

public record DeleteAnimeCommand(int ID) : IRequest<Result<Unit>>;