using MediatR;
using AutoMapper;
using Application.Dtos;
using Domain.Interfaces;

namespace Application.Queries.Anime.GetAnimeByTitle;

public class GetAnimeByTitleQueryHandler : IRequestHandler<GetAnimeByTitleQuery, AnimeDto?>
{
    private readonly IAnimeRepository _repository;
    private readonly IMapper _mapper;

    public GetAnimeByTitleQueryHandler(IAnimeRepository repository, IMapper mapper)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<AnimeDto?> Handle(GetAnimeByTitleQuery request, CancellationToken cancellationToken)
    {
        var anime = await _repository.GetByTitleAsync(request.Title);

        if (anime == null)
        {
            throw new KeyNotFoundException($"Anime with Title {request.Title} not found.");
        }

        return _mapper.Map<AnimeDto>(anime);
    }
}