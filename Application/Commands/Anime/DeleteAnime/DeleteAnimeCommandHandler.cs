using MediatR;
using AutoMapper;
using Domain.Interfaces;

namespace Application.Commands.Anime.DeleteAnime;

public class DeleteAnimeCommandHandler : IRequestHandler<DeleteAnimeCommand, int>
{
    private readonly IAnimeRepository _repository;

    public DeleteAnimeCommandHandler(IAnimeRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(DeleteAnimeCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteByIdAsync(request.ID);
        return request.ID;
    }
}