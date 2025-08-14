using MediatR;
using AutoMapper;
using Application.Dtos;
using Domain.Interfaces;
using AnimeModel = Domain.Models.Anime;

namespace Application.Commands.Anime.CreateAnime;

public class CreateAnimeCommandHandler : IRequestHandler<CreateAnimeCommand, AnimeDto>
{
    private readonly IAnimeRepository _repository;
    private readonly IMapper _mapper;

    public CreateAnimeCommandHandler(IAnimeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<AnimeDto> Handle(CreateAnimeCommand request, CancellationToken cancellationToken)
    {
        var task = AnimeModel.Create(request.Title, request.Author, request.Description);
        await _repository.AddAsync(task);
        return _mapper.Map<AnimeDto>(task);
    }
}