using MediatR;
using AutoMapper;
using Application.Dtos;
using Domain.Interfaces;

namespace Application.Queries.Anime.GetAnimeRange;

public class GetAnimeRangeQueryHandler : IRequestHandler<GetAnimeRangeQuery, IEnumerable<AnimeDto>>
{
    private readonly IAnimeRepository _repository;
    private readonly IMapper _mapper;

    public GetAnimeRangeQueryHandler(IAnimeRepository repository, IMapper mapper)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<IEnumerable<AnimeDto>> Handle(GetAnimeRangeQuery request, CancellationToken cancellationToken)
    {
        var anime = await _repository.GetRangeAsync(request.start, request.end);
        return _mapper.Map<IEnumerable<AnimeDto>>(anime);
    }
}