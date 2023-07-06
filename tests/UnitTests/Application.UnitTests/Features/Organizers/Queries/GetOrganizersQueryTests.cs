using Application.Features.Managers;
using Application.Features.Organizers;
using Application.Features.Organizers.Queries;
using Application.Features.Players;
using AutoMapper;
using Domain.Common;
using Domain.Managers;
using Domain.Organizers;
using Shared;

namespace Application.UnitTests.Features.Organizers.Queries;
public class GetOrganizersQueryTests
{
    private readonly IMapper _mapper;

    public GetOrganizersQueryTests()
    {
        _mapper = new MapperConfiguration(x =>
        {
            x.AddProfile(new OrganizerMappingProfile());
            x.AddProfile(new ManagerMappingProfile());
            x.AddProfile(new PlayerMappingProfile());
        }).CreateMapper();
    }

    [Fact]
    public async void ShouldReturnACollectionOfOrganizerResponse()
    {
        var (getOrganizersQueryHandler, getOrganizerQuery, organizerRepoMock) = GetHandlerAndMocks();

        IEnumerable<OrganizerResponse> organizers = await getOrganizersQueryHandler.Handle(getOrganizerQuery, default);

        organizerRepoMock.Verify(m => m.GetAllOrganizersAsync(It.IsAny<CancellationToken>()), Times.Once);
        organizers.Should().NotBeEmpty();
    }

    private (GetOrganizersQueryHandler getOrganizersQueryHandler, GetOrganizersQuery getOrganizersQuery, Mock<IOrganizerRepository> organizerRepoMock) GetHandlerAndMocks()
    {
        List<Organizer> Organizers = new()
        {
            GetOrganizerWithTournamentAndTeams("o1"),
            GetOrganizerWithTournamentAndTeams("o2")
        };

        var unitOfWorkFactoryMock = new Mock<IUnitOfWorkFactory>();
        var unitOfWorkMock = UnitOfWorkMock.Instance;
        var organizerRepoMock = new Mock<IOrganizerRepository>();

        unitOfWorkFactoryMock.Setup(uowf => uowf.CreateUnitOfWork(It.IsAny<string>())).Returns(unitOfWorkMock.Object);
        organizerRepoMock.Setup(m => m.GetAllOrganizersAsync(It.IsAny<CancellationToken>()).Result).Returns(Organizers);
        unitOfWorkMock.Setup(m => m.Organizers).Returns(organizerRepoMock.Object);

        var getOrganizersQuery = new GetOrganizersQuery();
        var getOrganizersQueryHandler = new GetOrganizersQueryHandler(unitOfWorkFactoryMock.Object, _mapper);

        return (getOrganizersQueryHandler, getOrganizersQuery, organizerRepoMock);
    }

    static Organizer GetOrganizerWithTournamentAndTeams(string organizerName)
    {
        var t1 = Team.Create("test", Manager.Create(new("test1", "test", "test", DateTime.Now, "", "", "", "", "")));
        var t2 = Team.Create("test", Manager.Create(new("test2", "test", "test", DateTime.Now, "", "", "", "", "")));
        var organizer = Organizer.Create(new(organizerName, "test", "test@gamil.com", DateTime.Now, "", "", "", "", ""));
        organizer.CreateTournament("test tournament");
        organizer.RegisterTeam(t1);
        organizer.RegisterTeam(t2);
        return organizer;
    }
}