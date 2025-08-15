using MediatR;
using AutoMapper;
using FluentResults;
using Application.Dtos;
using Domain.Interfaces;
using Application.Errors;
using AnimeModel = Domain.Models.Anime;

namespace Application.Commands.Anime.CreateAnime;

public class CreateAnimeCommandHandler : IRequestHandler<CreateAnimeCommand, Result<AnimeDto>>
{
    private readonly IAnimeRepository _repository;
    private readonly IMapper _mapper;

    public CreateAnimeCommandHandler(IAnimeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<AnimeDto>> Handle(CreateAnimeCommand request, CancellationToken cancellationToken)
    {
        var anime = AnimeModel.Create(request.Title, request.Author, request.Description);

        var existing = await _repository.GetByTitleAsync(request.Title);
        if (existing != null && existing.ID != request.ID)
            return Result.Fail(new Conflict($"Anime with Title '{request.Title}' already exists."));

        await _repository.AddAsync(anime);

        var dto = _mapper.Map<AnimeDto>(anime);
        return Result.Ok(dto);
    }
}