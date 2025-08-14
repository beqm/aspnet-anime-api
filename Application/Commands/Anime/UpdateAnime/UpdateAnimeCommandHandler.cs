using MediatR;
using AutoMapper;
using Domain.Interfaces;
using Application.Dtos;

namespace Application.Commands.Anime.UpdateAnime;

public class UpdateAnimeCommandHandler : IRequestHandler<UpdateAnimeCommand, AnimeDto>
{
    private readonly IAnimeRepository _repository;
    private readonly IMapper _mapper;

    public UpdateAnimeCommandHandler(IAnimeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<AnimeDto> Handle(UpdateAnimeCommand request, CancellationToken cancellationToken)
    {
        var anime = await _repository.GetByIdAsync(request.ID);
        if (anime == null)
        {
            throw new KeyNotFoundException($"Anime with Id {request.ID} not found.");
        }

        anime.Update(request.Title, request.Author, request.Description);

        await _repository.UpdateAsync(anime);
        return _mapper.Map<AnimeDto>(anime);
    }
}