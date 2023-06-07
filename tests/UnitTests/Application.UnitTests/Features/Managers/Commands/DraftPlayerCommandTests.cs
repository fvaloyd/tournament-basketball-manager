using Domain.Common;
using Domain.Players;
using Domain.Managers;
using Domain.Managers.Exceptions;
using Application.Features.Managers.Commands;

namespace Application.UnitTests.Features.Managers.Commands;
public class DraftPlayerCommandTests
{
    [Fact]
    public async Task ShouldThrowAManagerNotFoundException_WhenManagerNotFound()
    {
        var (draftPlayerCommandHandler, draftPlayerCommand, unitOfWorkMock, managerRepoMock, playerRepoMock) = GetHandlerAndMocks(HandlerCallOption.NullManager);

        Func<Task> handlerFunc = () => draftPlayerCommandHandler.Handle(draftPlayerCommand, default);

        await handlerFunc.Should().ThrowAsync<ManagerNotFoundException>();
        unitOfWorkMock.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        playerRepoMock.Verify(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        managerRepoMock.Verify(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ShouldThrowAPlayerNotFoundException_WhenPlayerNotFound()
    {
        var (draftPlayerCommandHandler, draftPlayerCommand, unitOfWorkMock, managerRepoMock, playerRepoMock) = GetHandlerAndMocks(HandlerCallOption.NullPlayer);

        Func<Task> handlerFunc = () => draftPlayerCommandHandler.Handle(draftPlayerCommand, default);

        await handlerFunc.Should().ThrowAsync<PlayerNotFoundException>();
        managerRepoMock.Verify(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        playerRepoMock.Verify(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ShouldSuccess_WhenManagerIsInValidState()
    {
        var (draftPlayerCommandHandler, draftPlayerCommand, unitOfWorkMock, managerRepoMock, playerRepoMock) = GetHandlerAndMocks(HandlerCallOption.Valid);

        await draftPlayerCommandHandler.Handle(draftPlayerCommand, default);

        managerRepoMock.Verify(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        playerRepoMock.Verify(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    static (DraftPlayerCommandHandler draftPlayerCommandHandler,
            DraftPlayerCommand draftPlayerCommand,
            Mock<IUnitOfWork> unitOfWorkMock,
            Mock<IManagerRepository> managerRepoMock,
            Mock<IPlayerRepository> playerRepoMock)
    GetHandlerAndMocks(HandlerCallOption option)
    {
        Player? player = option switch {
            HandlerCallOption.NullPlayer => null,
            HandlerCallOption.Valid => Player.Create(new("player2", "test", "player2@gmail.com", DateTime.Now, 6.6f, 90.5f, "1", "2", "3", "4", "5"), Position.ShootingGuard),
            HandlerCallOption.NullManager => Player.Create(new("player2", "test", "player2@gmail.com", DateTime.Now, 6.6f, 90.5f, "1", "2", "3", "4", "5"), Position.ShootingGuard),
            _ => throw new NotImplementedException()
        };
        Manager? manager = option switch {
            HandlerCallOption.NullManager => null,
            HandlerCallOption.Valid => Manager.Create(new ManagerPersonalInfo("test", "test", "test", DateTime.Now, "test", "test", "test", "test", "test"), "teamName"),
            HandlerCallOption.NullPlayer => Manager.Create(new ManagerPersonalInfo("test", "test", "test", DateTime.Now, "test", "test", "test", "test", "test"), "teamName"),
            _ => throw new NotImplementedException()
        };
        var unitOfWorkMock = UnitOfWorkMock.Instance;
        var managerRepoMock = new Mock<IManagerRepository>();
        var playerRepoMock = new Mock<IPlayerRepository>();
        managerRepoMock.Setup(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()).Result).Returns(manager!);
        playerRepoMock.Setup(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()).Result).Returns(player!);
        unitOfWorkMock.Setup(m => m.Managers).Returns(managerRepoMock.Object);
        unitOfWorkMock.Setup(m => m.Players).Returns(playerRepoMock.Object);
        var draftPlayerCommand = new DraftPlayerCommand(){ManagerId = Guid.NewGuid(), PlayerId = Guid.NewGuid()};
        var draftPlayerCommandHandler = new DraftPlayerCommandHandler(new Mock<ILoggerManager>().Object, unitOfWorkMock.Object);

        return (draftPlayerCommandHandler, draftPlayerCommand, unitOfWorkMock, managerRepoMock, playerRepoMock);
    }
    enum HandlerCallOption
    {
        NullManager,
        NullPlayer,
        Valid
    }
}