using MediatR;
using FluentResults;
using Domain.Interfaces;
using Application.Errors;

namespace Application.Commands.Anime.DeleteAnime;

public class DeleteAnimeCommandHandler : IRequestHandler<DeleteAnimeCommand, Result<Unit>>
{
    private readonly IAnimeRepository _repository;

    public DeleteAnimeCommandHandler(IAnimeRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Unit>> Handle(DeleteAnimeCommand request, CancellationToken cancellationToken)
    {
        var result = await _repository.DeleteByIdAsync(request.ID);
        if (result == 0)
        {
            return Result.Fail(new NotFound($"Anime with Id {request.ID} not found."));
        }
        return Result.Ok();
    }
}