using Domain.Common;
using Domain.Managers;
using Domain.Organizers;
using Domain.Organizers.Exceptions;
using Application.Features.Organizers.Commands;

namespace Application.UnitTests.Features.Organizers.Commands;
public class RegisterTeamCommandTests
{
    [Fact]
    public async Task ShouldThrowAOrganizerNotFoundException_WhenOrganizerNotFound()
    {
        var (registerTeamCommandHandler, registerTeamCommand, unitOfWorkMock, organizerRepoMock, teamRepoMock) = GetHandlerAndMocks(HandlerCallOption.NullOrganizer);

        Func<Task> handlerFunc = () => registerTeamCommandHandler.Handle(registerTeamCommand, default);

        await handlerFunc.Should().ThrowAsync<OrganizerNotFoundException>();
        unitOfWorkMock.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        teamRepoMock.Verify(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        organizerRepoMock.Verify(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ShouldThrowATeamNotFoundException_WhenTeamNotFound()
    {
        var (registerTeamCommandHandler, registerTeamCommand, unitOfWorkMock, organizerRepoMock, teamRepoMock) = GetHandlerAndMocks(HandlerCallOption.NullTeam);

        Func<Task> handlerFunc = () => registerTeamCommandHandler.Handle(registerTeamCommand, default);

        await handlerFunc.Should().ThrowAsync<TeamNotFoundException>();
        unitOfWorkMock.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        teamRepoMock.Verify(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        organizerRepoMock.Verify(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ShouldRegisterTheTeam_WhenOrganzierIsInValidState()
    {
        var (registerTeamCommandHandler, registerTeamCommand, unitOfWorkMock, organizerRepoMock, teamRepoMock) = GetHandlerAndMocks(HandlerCallOption.Valid);

        await registerTeamCommandHandler.Handle(registerTeamCommand, default);

        unitOfWorkMock.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        teamRepoMock.Verify(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        organizerRepoMock.Verify(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    static (
    RegisterTeamCommandHandler matchTeamsCommandHandler,
    RegisterTeamCommand matchTeamsCommand,
    Mock<IUnitOfWork> unitOfWorkMock,
    Mock<IOrganizerRepository> organizerRepoMock,
    Mock<ITeamRepository> teamRepoMock
    )
    GetHandlerAndMocks(HandlerCallOption option)
    {
        Organizer? organizer = option switch
        {
            HandlerCallOption.NullOrganizer => null,
            HandlerCallOption.NullTeam => Organizer.Create(new("", "", "", DateTime.Today, "", "", "", "", "")),
            HandlerCallOption.Valid => Organizer.Create(new("", "", "", DateTime.Today, "", "", "", "", "")),
            _ => throw new NotImplementedException()
        };
        if (option is HandlerCallOption.Valid || option is HandlerCallOption.NullTeam)
            organizer!.CreateTournament("test");
        Team? team = option switch
        {
            HandlerCallOption.NullTeam => null,
            HandlerCallOption.NullOrganizer => Team.Create("", Manager.Create(new("", "", "", DateTime.Today, "", "", "", "", ""))),
            HandlerCallOption.Valid => Team.Create("", Manager.Create(new("", "", "", DateTime.Today, "", "", "", "", ""))),
            _ => throw new NotImplementedException()
        };
        var unitOfWorkMock = UnitOfWorkMock.Instance;
        var organizerRepoMock = new Mock<IOrganizerRepository>();
        var teamRepoMock = new Mock<ITeamRepository>();
        organizerRepoMock.Setup(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()).Result).Returns(organizer!);
        teamRepoMock.Setup(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()).Result).Returns(team!);
        unitOfWorkMock.Setup(m => m.Organizers).Returns(organizerRepoMock.Object);
        unitOfWorkMock.Setup(m => m.Teams).Returns(teamRepoMock.Object);
        var registerTeamCommand = new RegisterTeamCommand() { OrganizerId = Guid.NewGuid(), TeamId = team is null ? Guid.NewGuid() : team.Id };
        var registerTeamCommandHandler = new RegisterTeamCommandHandler(unitOfWorkMock.Object, new Mock<ILoggerManager>().Object);

        return (
            registerTeamCommandHandler,
            registerTeamCommand,
            unitOfWorkMock,
            organizerRepoMock,
            teamRepoMock
        );
    }

    enum HandlerCallOption
    {
        NullOrganizer,
        NullTeam,
        Valid
    }
}