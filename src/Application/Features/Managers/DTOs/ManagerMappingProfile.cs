using AutoMapper;
using Domain.Managers;

namespace Application.Features.Managers.DTOs;
public class ManagerMappingProfile : Profile
{
    public ManagerMappingProfile()
    {
        CreateMap<Manager, ManagerResponse>().ReverseMap();
        CreateMap<Team, TeamResponse>().ReverseMap();
    }
}
