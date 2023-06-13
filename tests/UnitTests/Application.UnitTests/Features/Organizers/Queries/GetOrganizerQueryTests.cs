using Domain.Common;
using Domain.Managers;
using Domain.Organizers;
using Domain.Organizers.Exceptions;
using Application.Features.Organizers.DTOs;
using Application.Features.Organizers.Queries;
using AutoMapper;

namespace Application.UnitTests.Features.Organizers.Queries;
public class GetOrganizerQueryTests
{
    [Fact]
    public async Task ShouldThrowAOrganizerNotFoundException_WhenOrganizerNotFound()
    {
        var (getOrganizerQueryHandler, getOrganizerQuery, organizerRepoMock, mapperMock, _) = GetHandlerAndMocks(HandlerCallOption.NullOrganizer);

        Func<Task<OrganizerResponse>> handlerFunc = () => getOrganizerQueryHandler.Handle(getOrganizerQuery, default);

        await handlerFunc.Should().ThrowAsync<OrganizerNotFoundException>();
        organizerRepoMock.Verify(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        mapperMock.Verify(m => m.Map<OrganizerResponse>(It.IsAny<Organizer>()), Times.Never);
    }

    [Fact]
    public async Task ShouldReturnAOrganizerResponse_WhenOrganizerIsFound()
    {
        var (getOrganizerQueryHandler, getOrganizerQuery, organizerRepoMock, mapperMock, _) = GetHandlerAndMocks(HandlerCallOption.Valid);

        OrganizerResponse result = await getOrganizerQueryHandler.Handle(getOrganizerQuery, default);

        organizerRepoMock.Verify(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        mapperMock.Verify(m => m.Map<OrganizerResponse>(It.IsAny<Organizer>()), Times.Once);
    }

    static(
        GetOrganizerQueryHandler getOrganizerQueryHandler,
        GetOrganizerQuery getOrganizerQuery,
        Mock<IOrganizerRepository> organizerRepoMock,
        Mock<IMapper> mapperMock,
        Mock<IUnitOfWork> unitOfWorkMock
    )
    GetHandlerAndMocks(HandlerCallOption option)
    {
        Organizer? organizer = option switch
        {
            HandlerCallOption.NullOrganizer => null,
            HandlerCallOption.Valid => GetOrganizerWithTournamentAndTeams(),
            _ => throw new NotImplementedException()
        };
        var unitOfWorkMock = UnitOfWorkMock.Instance;
        var organizerRepoMock = new Mock<IOrganizerRepository>();
        organizerRepoMock.Setup(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()).Result).Returns(organizer!);
        unitOfWorkMock.Setup(m => m.Organizers).Returns(organizerRepoMock.Object);
        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(m => m.Map<OrganizerResponse>(It.IsAny<Organizer>())).Returns(new OrganizerResponse());
        var getOrganizerQuery = new GetOrganizerQuery();
        var getOrganizerQueryHandler = new GetOrganizerQueryHandler(unitOfWorkMock.Object, mapperMock.Object, new Mock<ILoggerManager>().Object);

        return(
            getOrganizerQueryHandler,
            getOrganizerQuery,
            organizerRepoMock,
            mapperMock,
            unitOfWorkMock
        );
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