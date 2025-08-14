using AutoMapper;
using Domain.Models;
using Application.Dtos;

namespace Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Anime, AnimeDto>();
    }
}