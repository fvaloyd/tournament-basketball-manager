using AutoMapper;
using Domain.Organizers;
using Shared;

namespace Application.Features.Organizers;
public class OrganizerMappingProfile : Profile
{
    public OrganizerMappingProfile()
    {
        CreateMap<Match, MatchResponse>().ReverseMap();
        CreateMap<Organizer, OrganizerResponse>().ReverseMap();
        CreateMap<Tournament, TournamentResponse>().ReverseMap();
    }
}
