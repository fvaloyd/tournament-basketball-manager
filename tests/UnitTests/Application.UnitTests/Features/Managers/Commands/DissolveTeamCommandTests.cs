using Domain.Common;
using Domain.Managers;
using Domain.Managers.Exceptions;
using Application.Features.Managers.Commands;

namespace Application.UnitTests.Features.Managers.Commands;
public class DissolveTeamCommandTests
{
    [Fact]
    public async Task ShouldThrowAManagerNotFoundException_WhenManagerNotFound()
    {
        var (dissolveTeamCommandHandler, dissolveTeamCommand, unitOfWorkMock, managerRepoMock) = GetHandlerAndMocks(HandlerCallOption.NullManager);

        Func<Task> handlerFunc = () => dissolveTeamCommandHandler.Handle(dissolveTeamCommand, default);

        await handlerFunc.Should().ThrowAsync<ManagerNotFoundException>();
        managerRepoMock.Verify(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ShouldDissolveTheTeam_WhenValidHandlerIsCall()
    {
        var (dissolveTeamCommandHandler, dissolveTeamCommand, unitOfWorkMock, managerRepoMock) = GetHandlerAndMocks(HandlerCallOption.Valid);

        await dissolveTeamCommandHandler.Handle(dissolveTeamCommand, default);

        unitOfWorkMock.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        managerRepoMock.Verify(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    static (
        DissolveTeamCommandHandler dissolveTeamCommandHandler,
        DissolveTeamCommand dissolveTeamCommand,
        Mock<IUnitOfWork> unitOfWorkMock,
        Mock<IManagerRepository> managerRepoMock
    )
    GetHandlerAndMocks(HandlerCallOption option)
    {
        Manager? manager = option switch {
            HandlerCallOption.NullManager => null,
            HandlerCallOption.Valid => Manager.Create(new ManagerPersonalInfo("test", "test", "test", DateTime.Now, new Address("test", "test", "test", "test", "test")), "teamName"),
            _ => throw new NotImplementedException()
        };
        var unitOfWorkMock = UnitOfWorkMock.Instance;
        var managerRepoMock = new Mock<IManagerRepository>();
        managerRepoMock.Setup(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()).Result).Returns(manager!);
        unitOfWorkMock.Setup(m => m.Managers).Returns(managerRepoMock.Object);
        var dissolveTeamCommand = new DissolveTeamCommand(){ManagerId = Guid.NewGuid()};
        var dissolveTeamCommandHandler = new DissolveTeamCommandHandler(new Mock<ILoggerManager>().Object, unitOfWorkMock.Object);
        return(
            dissolveTeamCommandHandler,
            dissolveTeamCommand,
            unitOfWorkMock,
            managerRepoMock
        );
    }

    enum HandlerCallOption
    {
        NullManager,
        Valid
    }
}