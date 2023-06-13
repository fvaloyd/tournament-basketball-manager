using AutoMapper;
using Domain.Players;
using Domain.Managers;
using Domain.Organizers;

namespace Infrastructure.NoSql.Models;
public class NoSqlModelsMappingProfile : Profile
{
    public NoSqlModelsMappingProfile()
    {
        CreateMap<Team, MongoTeam>().ReverseMap();
        CreateMap<Match, MongoMatch>().ReverseMap();
        CreateMap<Player, MongoPlayer>().ReverseMap();
        CreateMap<Manager, MongoManager>().ReverseMap();
        CreateMap<Organizer, MongoOrganizer>().ReverseMap();
        CreateMap<Tournament, MongoTournament>().ReverseMap();
    }
}
