using MediatR;
using AutoMapper;
using FluentResults;
using Domain.Interfaces;
using Application.Errors;
using Application.Dtos;

namespace Application.Queries.Anime.GetAnimeById;

public class GetAnimeByIdQueryHandler : IRequestHandler<GetAnimeByIdQuery, Result<AnimeDto?>>
{
    private readonly IAnimeRepository _repository;
    private readonly IMapper _mapper;

    public GetAnimeByIdQueryHandler(IAnimeRepository repository, IMapper mapper)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<Result<AnimeDto?>> Handle(GetAnimeByIdQuery request, CancellationToken cancellationToken)
    {
        var anime = await _repository.GetByIdAsync(request.ID);
        if (anime == null)
        {
            Console.WriteLine("entrou aqui");
            return Result.Fail(new NotFound($"Anime with ID {request.ID} not found."));
        }

        var dto = _mapper.Map<AnimeDto?>(anime);
        return Result.Ok(dto);
    }
}