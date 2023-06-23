using AutoMapper;
using Domain.Managers;
using Shared;

namespace Application.Features.Managers;
public class ManagerMappingProfile : Profile
{
    public ManagerMappingProfile()
    {
        CreateMap<Manager, ManagerResponse>().ReverseMap();
        CreateMap<Team, TeamResponse>().ReverseMap();
    }
}
