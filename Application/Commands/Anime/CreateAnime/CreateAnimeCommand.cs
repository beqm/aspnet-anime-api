using MediatR;
using Application.Dtos;

namespace Application.Commands.Anime.CreateAnime;

public record CreateAnimeCommand(int ID, string Title, string Author, string Description) : IRequest<AnimeDto>;