using Domain.Common;
using Domain.Managers;
using Domain.Organizers;
using Domain.Organizers.Exceptions;
using Application.Features.Organizers.Commands;

namespace Application.UnitTests.Features.Organizers.Commands;
public class FinishTournamentCommandTests
{
    [Fact]
    public async Task ShouldThrowAOrganizerNotFoundException_WhenOrganizerNotFound()
    {
        var (finishTournamentCommandHandler, finishTournamentCommand, unitOfWorkMock, organizerRepoMock) = GetHandlerAndMocks(HandlerCallOption.NullOrganizer);

        Func<Task> handlerFunc = () => finishTournamentCommandHandler.Handle(finishTournamentCommand, default);

        await handlerFunc.Should().ThrowAsync<OrganizerNotFoundException>();
        organizerRepoMock.Verify(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ShouldFinishTheTournament_WhenOrganizerIsInValidState()
    {
        var (finishTournamentCommandHandler, finishTournamentCommand, unitOfWorkMock, organizerRepoMock) = GetHandlerAndMocks(HandlerCallOption.Valid);

        await finishTournamentCommandHandler.Handle(finishTournamentCommand, default);

        unitOfWorkMock.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        organizerRepoMock.Verify(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    static(
        FinishTournamentCommandHandler finishTournamentCommandHandler,
        FinishTournamentCommand finishTournamentCommand,
        Mock<IUnitOfWork> unitOfWorkMock,
        Mock<IOrganizerRepository> organizerRepoMock
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
        var finishTournamentCommand = new FinishTournamentCommand() { OrganizerId = Guid.NewGuid() };
        var finishTournamentCommandHandler = new FinishTournamentCommandHandler(unitOfWorkMock.Object, new Mock<ILoggerManager>().Object);

        return(
            finishTournamentCommandHandler,
            finishTournamentCommand,
            unitOfWorkMock,
            organizerRepoMock
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