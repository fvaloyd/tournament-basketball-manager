using Domain.Common;
using Domain.Organizers;
using Domain.Organizers.Exceptions;
using Application.Features.Organizers.Commands;

namespace Application.UnitTests.Features.Organizers.Commands;
public class CreateTournamentCommandTests
{
    [Fact]
    public async Task ShouldThrowAOrganizerNotFoundException_WhenOrganizerNotFound()
    {
        var (createTournamentCommandHandler, createTournamentCommand, unitOfWorkMock, organizerRepoMock) = GetHandlerAndMocks(HandlerCallOption.NullOrganizer);

        Func<Task<Guid?>> handlerFunc = () => createTournamentCommandHandler.Handle(createTournamentCommand, default);

        await handlerFunc.Should().ThrowAsync<OrganizerNotFoundException>();
        organizerRepoMock.Verify(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ShouldCreateATournamnet_WhenOrganizerIsInValidState()
    {
        var (createTournamentCommandHandler, createTournamentCommand, unitOfWorkMock, organizerRepoMock) = GetHandlerAndMocks(HandlerCallOption.Valid);

        Guid? tournamentIdCreated = await createTournamentCommandHandler.Handle(createTournamentCommand, default);

        tournamentIdCreated.Should().NotBeEmpty();
        organizerRepoMock.Verify(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    static (
        CreateTournamentCommandHandler createTournamentCommandHandler,
        CreateTournamentCommand createTournamentCommand,
        Mock<IUnitOfWork> unitOfWorkMock,
        Mock<IOrganizerRepository> organizerRepoMock
    )
    GetHandlerAndMocks(HandlerCallOption option)
    {
        Organizer? organizer = option switch{
            HandlerCallOption.NullOrganizer => null,
            HandlerCallOption.Valid => Organizer.Create(new OrganizerPersonalInfo("", "", "", DateTime.Today, "", "", "", "", "")),
            _ => throw new NotImplementedException()
        };
        var unitOfWorkFactoryMock = new Mock<IUnitOfWorkFactory>();
        var unitOfWorkMock = UnitOfWorkMock.Instance;
        unitOfWorkFactoryMock.Setup(uowf => uowf.CreateUnitOfWork(It.IsAny<string>())).Returns(unitOfWorkMock.Object);
        var organizerRepoMock = new Mock<IOrganizerRepository>();
        organizerRepoMock.Setup(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()).Result).Returns(organizer!);
        unitOfWorkMock.Setup(m => m.Organizers).Returns(organizerRepoMock.Object);
        var createTournamentCommand = new CreateTournamentCommand()
        {
            OrganizerId = Guid.NewGuid(),
            TournamentName = "test"
        };
        var createTournamentCommandHandler = new CreateTournamentCommandHandler(new Mock<ILoggerManager>().Object, unitOfWorkFactoryMock.Object);

        return(
            createTournamentCommandHandler,
            createTournamentCommand,
            unitOfWorkMock,
            organizerRepoMock
        );
    }

    enum HandlerCallOption
    {
        NullOrganizer,
        Valid
    }
}