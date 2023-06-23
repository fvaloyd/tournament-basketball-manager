using AutoMapper;
using Domain.Players;
using Shared;

namespace Application.Features.Players;
public class PlayerMappingProfile : Profile
{
    public PlayerMappingProfile()
    {
        CreateMap<Player, PlayerResponse>().ReverseMap();
    }
}