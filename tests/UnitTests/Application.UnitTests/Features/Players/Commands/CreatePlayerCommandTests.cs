using Domain.Common;
using Domain.Players;
using Application.Features.Players.Commands;

namespace Application.UnitTests.Features.Players.Commands;
public class CreatePlayerCommandTests
{
    [Fact]
    public async Task ShouldCreateThePlayer()
    {
        var (createPlayerCommandHandler, createPlayerCommand, unitOfWorkMock, playerRepoMock) = GetHandlerAndMocks();

        Guid playerCreatedId = await createPlayerCommandHandler.Handle(createPlayerCommand, default);

        playerCreatedId.Should().NotBeEmpty();
        unitOfWorkMock.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        playerRepoMock.Verify(m => m.CreateAsync(It.IsAny<Player>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    static(
        CreatePlayerCommandHandler createPlayerCommandHandler,
        CreatePlayerCommand createPlayerCommand,
        Mock<IUnitOfWork> unitOfWorkMock,
        Mock<IPlayerRepository> playerRepoMock
    ) GetHandlerAndMocks()
    {
        var unitOfWorkMock = UnitOfWorkMock.Instance;
        var playerRepoMock = new Mock<IPlayerRepository>();
        unitOfWorkMock.Setup(m => m.Players).Returns(playerRepoMock.Object);
        var createPlayerCommand = new CreatePlayerCommand()
        {
            PlayerPersonalInfo = new PlayerPersonalInfo("", "", "", DateTime.Today, 1.80f, 80.1f, "", "", "", "", ""),
            Position = Position.PointGuard
        };
        var createPlayerCommandHandler = new CreatePlayerCommandHandler(unitOfWorkMock.Object);

        return(
            createPlayerCommandHandler,
            createPlayerCommand,
            unitOfWorkMock,
            playerRepoMock
        );
    }
}