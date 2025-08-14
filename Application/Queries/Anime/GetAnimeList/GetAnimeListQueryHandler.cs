using MediatR;
using AutoMapper;
using Application.Dtos;
using Domain.Interfaces;

namespace Application.Queries.Anime.GetAnimeList;

public class GetAnimeListQueryHandler : IRequestHandler<GetAnimeListQuery, IEnumerable<AnimeDto>>
{
    private readonly IAnimeRepository _repository;
    private readonly IMapper _mapper;

    public GetAnimeListQueryHandler(IAnimeRepository repository, IMapper mapper)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<IEnumerable<AnimeDto>> Handle(GetAnimeListQuery request, CancellationToken cancellationToken)
    {
        var anime = await _repository.GetListAsync(request.Author, request.Description);

        if (anime == null)
        {
            throw new KeyNotFoundException($"Anime not found.");
        }

        return _mapper.Map<IEnumerable<AnimeDto>>(anime);
    }
}