using MediatR;
using AutoMapper;
using FluentResults;
using Application.Dtos;
using Domain.Interfaces;

namespace Application.Queries.Anime.GetAnimeList;

public class GetAnimeListQueryHandler : IRequestHandler<GetAnimeListQuery, Result<List<AnimeDto>>>
{
    private readonly IAnimeRepository _repository;
    private readonly IMapper _mapper;

    public GetAnimeListQueryHandler(IAnimeRepository repository, IMapper mapper)
    {
        _mapper = mapper;
        _repository = repository;
    }


    public async Task<Result<List<AnimeDto>>> Handle(GetAnimeListQuery request, CancellationToken cancellationToken)
    {
        var animes = await _repository.GetListAsync(request.Author, request.Description);

        var dto = _mapper.Map<List<AnimeDto>>(animes);
        return Result.Ok(dto);
    }
}