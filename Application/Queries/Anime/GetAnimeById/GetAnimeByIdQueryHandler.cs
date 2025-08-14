using MediatR;
using AutoMapper;
using Application.Dtos;
using Domain.Interfaces;

namespace Application.Queries.Anime.GetAnimeById;

public class GetAnimeByIdQueryHandler : IRequestHandler<GetAnimeByIdQuery, AnimeDto?>
{
    private readonly IAnimeRepository _repository;
    private readonly IMapper _mapper;

    public GetAnimeByIdQueryHandler(IAnimeRepository repository, IMapper mapper)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<AnimeDto?> Handle(GetAnimeByIdQuery request, CancellationToken cancellationToken)
    {
        var anime = await _repository.GetByIdAsync(request.ID);

        if (anime == null)
        {
            throw new KeyNotFoundException($"Anime with ID {request.ID} not found.");
        }

        return _mapper.Map<AnimeDto>(anime);
    }
}