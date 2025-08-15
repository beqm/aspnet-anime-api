using MediatR;
using FluentResults;
using Domain.Interfaces;
using Application.Errors;
using AnimeModel = Domain.Models.Anime;
using AutoMapper;
using Application.Dtos;

namespace Application.Commands.Anime.UpdateAnime;

public class UpdateAnimeCommandHandler : IRequestHandler<UpdateAnimeCommand, Result<AnimeDto>>
{
    private readonly IAnimeRepository _repository;
    private readonly IMapper _mapper;

    public UpdateAnimeCommandHandler(IAnimeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<AnimeDto>> Handle(UpdateAnimeCommand request, CancellationToken cancellationToken)
    {
        var anime = await _repository.GetByIdAsync(request.ID);
        if (anime == null)
            return Result.Fail(new NotFound($"Anime with Id {request.ID} not found."));

        if (request.Title != null)
        {
            var existing = await _repository.GetByTitleAsync(request.Title);
            if (existing != null && existing.ID != request.ID)
                return Result.Fail(new Conflict($"Anime with Title '{request.Title}' already exists."));
        }

        anime.Update(request.Title, request.Author, request.Description);
        await _repository.UpdateAsync(anime);

        var dto = _mapper.Map<AnimeDto>(anime);
        return Result.Ok(dto);
    }
}