using Domain.Common;
using Application.Features.Organizers.DTOs;
using Application.Features.Organizers.Queries;
using Domain.Organizers;
using AutoMapper;
using Application.Features.Managers.DTOs;
using Application.Features.Players;
using Match = Domain.Organizers.Match;
using Domain.Managers;
using Domain.Services;
using Domain.Organizers.Exceptions;

namespace Application.UnitTests.Features.Organizers.Queries;
public class GetTournamentMatchesTests
{
    private readonly IMapper _mapper;

    public GetTournamentMatchesTests()
    {
        _mapper = new MapperConfiguration(x =>
        {
            x.AddProfile(new OrganizerMappingProfile());
            x.AddProfile(new ManagerMappingProfile());
            x.AddProfile(new PlayerMappingProfile());
        }).CreateMapper();
    }

    [Fact]
    public async Task ShouldThrowAOrganizerNotFoundException_WhenOrganizerIsNotFound()
    {
        var (getTournamentMatchesQueryHandler, getTournamentMatchesQuery, organizerRepoMock) = GetHandlerAndMocks(HandlerCallOption.NullOrganizer);        

        Func<Task<IEnumerable<MatchResponse>>> act = () => getTournamentMatchesQueryHandler.Handle(getTournamentMatchesQuery, default);

        await act.Should().ThrowAsync<OrganizerNotFoundException>();
        organizerRepoMock.Verify(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ShouldReturnACollectionOfMatches_WhenValidOrganizerIsFound()
    {
        var (getTournamentMatchesQueryHandler, getTournamentMatchesQuery, organizerRepoMock) = GetHandlerAndMocks(HandlerCallOption.ValidOrganizer);

        IEnumerable<MatchResponse> matches = await getTournamentMatchesQueryHandler.Handle(getTournamentMatchesQuery, default);

        organizerRepoMock.Verify(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        matches.Should().NotBeEmpty();
    }

    private (GetTournamentMatchesQueryHandler getTournamentMatchesQueryHandler, GetTournamentMatchesQuery getTournamentMatchesQuery, Mock<IOrganizerRepository> organizerRepoMock) GetHandlerAndMocks(HandlerCallOption option)
    {
        Organizer? organizer = option switch
        {
            HandlerCallOption.ValidOrganizer => GetOrganizerWithTeamsPaired(),
            HandlerCallOption.NullOrganizer => null,
            _ => throw new NotImplementedException()
        };

        var unitOfWorkFactoryMock = new Mock<IUnitOfWorkFactory>();
        var unitOfWorkMock = UnitOfWorkMock.Instance;
        var organizerRepoMock = new Mock<IOrganizerRepository>();

        organizerRepoMock.Setup(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()).Result).Returns(organizer!);
        unitOfWorkMock.Setup(m => m.Organizers).Returns(organizerRepoMock.Object);
        unitOfWorkFactoryMock.Setup(uowf => uowf.CreateUnitOfWork(It.IsAny<string>())).Returns(unitOfWorkMock.Object);

        var getTournamentMatchesQuery = new GetTournamentMatchesQuery();
        var getTournamentMatchesQueryHandler = new GetTournamentMatchesQueryHandler(unitOfWorkFactoryMock.Object, new Mock<ILoggerManager>().Object, _mapper);

        return (getTournamentMatchesQueryHandler, getTournamentMatchesQuery, organizerRepoMock);
    }

    enum HandlerCallOption
    {
        NullOrganizer,
        ValidOrganizer
    }

    private Organizer GetOrganizerWithTeamsPaired()
    {
        var manager1 = Manager.Create(new("manager1", "test", "test@gamil.com", DateTime.Now, "RD", "Santo Domingo", "peaton 5 av. indepedencia km 10 1/2", "4", "10300"));
        manager1.CreateTeam("TeamA");
        var manager2 = Manager.Create(new("manager2", "test", "test@gamil.com", DateTime.Now, "RD", "Santo Domingo", "peaton 5 av. indepedencia km 10 1/2", "4", "10300"));
        manager2.CreateTeam("TeamB");
        var organizer = Organizer.Create(new("organizer", "test", "test@gamil.com", DateTime.Now, "RD", "Santo Domingo", "peaton 5 av. indepedencia km 10 1/2", "4", "10300"));
        organizer.CreateTournament("Tournament");
        organizer.RegisterTeam(manager1.Team!);
        organizer.RegisterTeam(manager2.Team!);
        organizer.MatchTeams(new RandomTeamMatchMaker());
        return organizer;
    }
}