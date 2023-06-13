using AutoMapper;
using Domain.Organizers;

namespace Application.Features.Organizers.DTOs;
public class OrganizerMappingProfile : Profile
{
    public OrganizerMappingProfile()
    {
        CreateMap<Match, MatchResponse>().ReverseMap();
        CreateMap<Organizer, OrganizerResponse>().ReverseMap();
        CreateMap<Tournament, TournamentResponse>().ReverseMap();
    }
}
