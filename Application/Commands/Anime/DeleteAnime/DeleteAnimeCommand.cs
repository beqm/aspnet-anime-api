using MediatR;

namespace Application.Commands.Anime.DeleteAnime;

public record DeleteAnimeCommand(int ID) : IRequest<int>;