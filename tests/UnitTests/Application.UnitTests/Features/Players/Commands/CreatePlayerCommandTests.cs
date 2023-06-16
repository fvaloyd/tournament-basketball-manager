using Domain.Common;
using Domain.Players;
using MassTransit;
using Application.Features.Players.Commands;

namespace Application.UnitTests.Features.Players.Commands;
public class CreatePlayerCommandTests
{
    [Fact]
    public async Task ShouldCreateThePlayer()
    {
        var (createPlayerCommandHandler, createPlayerCommand, unitOfWorkMock, playerRepoMock, busMock) = GetHandlerAndMocks();

        Guid playerCreatedId = await createPlayerCommandHandler.Handle(createPlayerCommand, default);

        playerCreatedId.Should().NotBeEmpty();
        unitOfWorkMock.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        playerRepoMock.Verify(m => m.CreateAsync(It.IsAny<Player>(), It.IsAny<CancellationToken>()), Times.Once);
        busMock.Verify(m => m.Publish(It.IsAny<PlayerCreatedEvent>(), It.IsAny<CancellationToken>()));
    }

    static(
        CreatePlayerCommandHandler createPlayerCommandHandler,
        CreatePlayerCommand createPlayerCommand,
        Mock<IUnitOfWork> unitOfWorkMock,
        Mock<IPlayerRepository> playerRepoMock,
        Mock<IBus> busMock
    ) GetHandlerAndMocks()
    {
        var busMock = new Mock<IBus>();
        var unitOfWorkFactoryMock = new Mock<IUnitOfWorkFactory>();
        var unitOfWorkMock = UnitOfWorkMock.Instance;
        unitOfWorkFactoryMock.Setup(uowf => uowf.CreateUnitOfWork(It.IsAny<string>())).Returns(unitOfWorkMock.Object);
        var playerRepoMock = new Mock<IPlayerRepository>();
        unitOfWorkMock.Setup(m => m.Players).Returns(playerRepoMock.Object);
        var createPlayerCommand = new CreatePlayerCommand()
        {
            PlayerPersonalInfo = new PlayerPersonalInfo("", "", "", DateTime.Today, 1.80f, 80.1f, "", "", "", "", ""),
            Position = Position.PointGuard
        };
        var createPlayerCommandHandler = new CreatePlayerCommandHandler(unitOfWorkFactoryMock.Object, busMock.Object);

        return(
            createPlayerCommandHandler,
            createPlayerCommand,
            unitOfWorkMock,
            playerRepoMock,
            busMock
        );
    }
}