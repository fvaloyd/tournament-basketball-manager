using Domain.Common;
using Domain.Managers;
using Domain.Organizers;
using Domain.Organizers.Exceptions;
using Application.Features.Organizers.Commands;

namespace Application.UnitTests.Features.Organizers.Commands;
public class DiscardTeamCommandTests
{
    [Fact]
    public async Task ShouldThrowAOrganizerNotFoundException_WhenOrganizerNotFound()
    {
        var (discardTeamCommandHandler, discardTeamCommand, unitOfWorkMock, organizerRepoMock) = GetHandlerAndMocks(HandlerCallOption.NullOrganizer);

        Func<Task> handlerFunc = () => discardTeamCommandHandler.Handle(discardTeamCommand, default);

        await handlerFunc.Should().ThrowAsync<OrganizerNotFoundException>();
        unitOfWorkMock.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        organizerRepoMock.Verify(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ShouldDiscardTheTeam_WhenOrganizerIsInValidState()
    {
        var (discardTeamCommandHandler, discardTeamCommand, unitOfWorkMock, organizerRepoMock) = GetHandlerAndMocks(HandlerCallOption.Valid);

        await discardTeamCommandHandler.Handle(discardTeamCommand, default);

        unitOfWorkMock.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        organizerRepoMock.Verify(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    static(
        DiscardTeamCommandHandler discardTeamCommandHandler,
        DiscardTeamCommand discardTeamCommand,
        Mock<IUnitOfWork> unitOfWorkMock,
        Mock<IOrganizerRepository> organizerRepoMock
    )
    GetHandlerAndMocks(HandlerCallOption option)
    {
        (Organizer? organizer, Guid teamToDiscardId) = option switch
        {
            HandlerCallOption.NullOrganizer => (null, Guid.NewGuid()),
            HandlerCallOption.Valid => GetOrganizerWithTournamentAndTeams(),
            _ => throw new NotImplementedException()
        };
        var unitOfWorkFactoryMock = new Mock<IUnitOfWorkFactory>();
        var unitOfWorkMock = UnitOfWorkMock.Instance;
        unitOfWorkFactoryMock.Setup(uowf => uowf.CreateUnitOfWork(It.IsAny<string>())).Returns(unitOfWorkMock.Object);
        var organizerRepoMock = new Mock<IOrganizerRepository>();
        organizerRepoMock.Setup(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()).Result).Returns(organizer!);
        unitOfWorkMock.Setup(m => m.Organizers).Returns(organizerRepoMock.Object);
        var discardTeamCommand = new DiscardTeamCommand() { OrganizerId = Guid.NewGuid(), TeamId = teamToDiscardId };
        var discardTeamCommandHandler = new DiscardTeamCommandHandler(unitOfWorkFactoryMock.Object, new Mock<ILoggerManager>().Object);

        return(
            discardTeamCommandHandler,
            discardTeamCommand,
            unitOfWorkMock,
            organizerRepoMock
        );
    }

    enum HandlerCallOption
    {
        NullOrganizer,
        Valid
    }

    static (Organizer organizer, Guid teamToDiscardId) GetOrganizerWithTournamentAndTeams()
    {
        var t1 = Team.Create("test", Manager.Create(new("test1", "test", "test", DateTime.Now, "test", "test", "test", "test", "test")));
        var t2 = Team.Create("test", Manager.Create(new("test2", "test", "test", DateTime.Now, "test", "test", "test", "test", "test")));
        var organizer = Organizer.Create(new("test", "test", "test@gamil.com", DateTime.Now, "", "", "", "", ""));
        organizer.CreateTournament("test tournament");
        organizer.RegisterTeam(t1);
        organizer.RegisterTeam(t2);
        return (
            organizer,
            t1.Id
        );
    }
}