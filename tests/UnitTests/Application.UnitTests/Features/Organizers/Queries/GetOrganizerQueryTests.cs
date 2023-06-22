using Domain.Common;
using Domain.Managers;
using Domain.Organizers;
using Domain.Organizers.Exceptions;
using Application.Features.Organizers.DTOs;
using Application.Features.Organizers.Queries;
using AutoMapper;
using Application.Features.Managers.DTOs;
using Application.Features.Players;

namespace Application.UnitTests.Features.Organizers.Queries;
public class GetOrganizerQueryTests
{
    private readonly IMapper _mapper;

    public GetOrganizerQueryTests()
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
        var (getOrganizerQueryHandler, getOrganizerQuery, organizerRepoMock) = GetHandlerAndMocks(HandlerCallOption.NullOrganizer);

        Func<Task<OrganizerResponse>> handlerFunc = () => getOrganizerQueryHandler.Handle(getOrganizerQuery, default);

        await handlerFunc.Should().ThrowAsync<OrganizerNotFoundException>();
        organizerRepoMock.Verify(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ShouldReturnAOrganizerResponse_WhenOrganizerIsFound()
    {
        var (getOrganizerQueryHandler, getOrganizerQuery, organizerRepoMock) = GetHandlerAndMocks(HandlerCallOption.Valid);

        OrganizerResponse organizer = await getOrganizerQueryHandler.Handle(getOrganizerQuery, default);

        organizerRepoMock.Verify(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        organizer.Should().NotBeNull();
    }

    private (GetOrganizerQueryHandler getOrganizerQueryHandler, GetOrganizerQuery getOrganizerQuery, Mock<IOrganizerRepository> organizerRepoMock) GetHandlerAndMocks(HandlerCallOption option)
    {
        Organizer? organizer = option switch
        {
            HandlerCallOption.NullOrganizer => null,
            HandlerCallOption.Valid => GetOrganizerWithTournamentAndTeams(),
            _ => throw new NotImplementedException()
        };

        var unitOfWorkFactoryMock = new Mock<IUnitOfWorkFactory>();
        var unitOfWorkMock = UnitOfWorkMock.Instance;
        var organizerRepoMock = new Mock<IOrganizerRepository>();
        
        unitOfWorkFactoryMock.Setup(uowf => uowf.CreateUnitOfWork(It.IsAny<string>())).Returns(unitOfWorkMock.Object);
        organizerRepoMock.Setup(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()).Result).Returns(organizer!);
        unitOfWorkMock.Setup(m => m.Organizers).Returns(organizerRepoMock.Object);

        var getOrganizerQuery = new GetOrganizerQuery();
        var getOrganizerQueryHandler = new GetOrganizerQueryHandler(unitOfWorkFactoryMock.Object, _mapper, new Mock<ILoggerManager>().Object);

        return(getOrganizerQueryHandler, getOrganizerQuery, organizerRepoMock);
    }

    enum HandlerCallOption
    {
        NullOrganizer,
        Valid
    }

    static Organizer GetOrganizerWithTournamentAndTeams()
    {
        var t1 = Team.Create("test", Manager.Create(new("test1", "test", "test", DateTime.Now, "", "", "", "", "")));
        var t2 = Team.Create("test", Manager.Create(new("test2", "test", "test", DateTime.Now, "", "", "", "", "")));
        var organizer = Organizer.Create(new("test", "test", "test@gamil.com", DateTime.Now, "", "", "", "", ""));
        organizer.CreateTournament("test tournament");
        organizer.RegisterTeam(t1);
        organizer.RegisterTeam(t2);
        return organizer;
    }
}