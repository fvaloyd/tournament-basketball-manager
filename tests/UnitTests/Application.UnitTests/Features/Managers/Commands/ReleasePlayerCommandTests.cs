using Domain.Common;
using Domain.Players;
using Domain.Managers;
using Domain.Managers.Exceptions;
using Application.Features.Managers.Commands;
using MassTransit;

namespace Application.UnitTests.Features.Managers.Commands;
public class ReleasePlayerCommandTests
{
    [Fact]
    public async Task ShouldThrowAManagerNotFoundException_WhenManagerNotFound()
    {
        var (releasePlayerCommandHandler, releasePlayerCommand, unitOfWorkMock, managerRepoMock, busMock) = GetHandlerAndMocks(HandlerCallOption.NullManager);

        Func<Task> handlerFunc = () => releasePlayerCommandHandler.Handle(releasePlayerCommand, default);

        await handlerFunc.Should().ThrowAsync<ManagerNotFoundException>();
        unitOfWorkMock.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        managerRepoMock.Verify(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        busMock.Verify(m => m.Publish(It.IsAny<PlayerReleasedEvent>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ShouldSuccess_WhenManagerIsInValidState()
    {
        var (releasePlayerCommandHandler, releasePlayerCommand, unitOfWorkMock, managerRepoMock, busMock) = GetHandlerAndMocks(HandlerCallOption.Valid);

        await releasePlayerCommandHandler.Handle(releasePlayerCommand, default);

        managerRepoMock.Verify(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        busMock.Verify(m => m.Publish(It.IsAny<PlayerReleasedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    static(
        ReleasePlayerCommandHandler releasePlayerCommandHandler,
        ReleasePlayerCommand releasePlayerCommand,
        Mock<IUnitOfWork> unitOfWorkMock,
        Mock<IManagerRepository> managerRepoMock,
        Mock<IBus> busMock
    )
    GetHandlerAndMocks(HandlerCallOption option)
    {
        Player? player = null;
        Manager? manager = option switch
        {
            HandlerCallOption.NullManager => null,
            HandlerCallOption.Valid => Manager.Create(new ManagerPersonalInfo("test", "test", "test", DateTime.Now, "test", "test", "test", "test", "test"), "teamName"),
            _ => throw new NotImplementedException()
        };
        if (option is HandlerCallOption.Valid)
        {
            player = Player.Create(new("player2", "test", "player2@gmail.com", DateTime.Now, 6.6f, 90.5f, "test", "test", "test", "test", "test"), Position.ShootingGuard);
            manager!.DraftPlayer(player);
        }
        var busMock = new Mock<IBus>();
        var unitOfWorkFactoryMock = new Mock<IUnitOfWorkFactory>();
        var unitOfWorkMock = UnitOfWorkMock.Instance;
        unitOfWorkFactoryMock.Setup(uowf => uowf.CreateUnitOfWork(It.IsAny<string>())).Returns(unitOfWorkMock.Object);
        var managerRepoMock = new Mock<IManagerRepository>();
        managerRepoMock.Setup(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()).Result).Returns(manager!);
        unitOfWorkMock.Setup(m => m.Managers).Returns(managerRepoMock.Object);
        var releasePlayerCommand = new ReleasePlayerCommand()
        {
            ManagerId = Guid.NewGuid(),
            PlayerId = player is null ? Guid.NewGuid() : player.Id
        };
        var releasePlayerCommandHandler = new ReleasePlayerCommandHandler(new Mock<ILoggerManager>().Object, unitOfWorkFactoryMock.Object, busMock.Object);

        return(
            releasePlayerCommandHandler,
            releasePlayerCommand,
            unitOfWorkMock,
            managerRepoMock,
            busMock
        );
    }

    enum HandlerCallOption
    {
        NullManager,
        Valid
    }
}