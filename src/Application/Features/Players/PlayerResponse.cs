using AutoMapper;
using Domain.Players;

namespace Application.Features.Players;
public record PlayerResponse
{
    public Guid Id { get; init; }
    public PlayerPersonalInfo PersonalInfo { get; init; } = null!;
    public Position Position { get; init; }
    public Guid TeamId { get; set; }
}

public class PlayerMappingProfile : Profile
{
    public PlayerMappingProfile()
    {
        CreateMap<Player, PlayerResponse>().ReverseMap();
    }
}