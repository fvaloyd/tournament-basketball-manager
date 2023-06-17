using Domain.Common;
using Domain.Managers;
using Domain.Managers.Exceptions;
using Application.Features.Managers.Commands;
using MassTransit;

namespace Application.UnitTests.Features.Managers.Commands;
public class CreateTeamCommandTests
{
    [Fact]
    public async Task ShouldThrowAnManagerNotFoundException_WhenNoManagerNotFound()
    {
        var unitOfWorkMock = UnitOfWorkMock.Instance;
        var (CreateTeamCommandHandlerFunc, mockManagerRepo, busMock) = new CreateTeamCommandHandlerBuilder(unitOfWorkMock).WithNulldManager().Build();

        await CreateTeamCommandHandlerFunc.Should().ThrowAsync<ManagerNotFoundException>();
        mockManagerRepo.Verify(m => m.GetByIdAsync(It.IsAny<Guid>(), default), Times.Once);
        unitOfWorkMock.Verify(u => u.SaveChangesAsync(default), Times.Never);
        busMock.Verify(m => m.Publish(It.IsAny<TeamCreatedEvent>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ShouldReturnTheTeamIdCreated_WhenValidHandlerIsCall()
    {
        var unitOfWorkMock = UnitOfWorkMock.Instance;
        var (CreateTeamCommandHandlerFunc, mockManagerRepo, busMock) = new CreateTeamCommandHandlerBuilder(unitOfWorkMock).WithValidManager().Build();

        var teamId = await CreateTeamCommandHandlerFunc();

        teamId.Should().NotBeEmpty();
        mockManagerRepo.Verify(m => m.GetByIdAsync(It.IsAny<Guid>(), default), Times.Once);
        unitOfWorkMock.Verify(u => u.SaveChangesAsync(default), Times.Once);
        busMock.Verify(m => m.Publish(It.IsAny<TeamCreatedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}

public class CreateTeamCommandHandlerBuilder
{
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<IManagerRepository> _managerRepoMock = new();
    private readonly Mock<IUnitOfWorkFactory> _unitOfWorkFactoryMock;
    public CreateTeamCommandHandlerBuilder(Mock<IUnitOfWork> unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _unitOfWorkFactoryMock = new Mock<IUnitOfWorkFactory>();
        _unitOfWork.Setup(m => m.Managers).Returns(_managerRepoMock.Object);
        _unitOfWorkFactoryMock.Setup(uowf => uowf.CreateUnitOfWork(It.IsAny<string>())).Returns(_unitOfWork.Object);
    }

    public CreateTeamCommandHandlerBuilder WithNulldManager()
    {
        _managerRepoMock.Setup(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()).Result).Returns(default(Manager)!);
        return this;
    }

    public CreateTeamCommandHandlerBuilder WithValidManager()
    {
        var manager = Manager.Create(new ManagerPersonalInfo("test", "", "", DateTime.Today, "test", "test", "test", "test", "test"));
        _managerRepoMock.Setup(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()).Result).Returns(manager);
        return this;
    }

    public (Func<Task<Guid?>> func, Mock<IManagerRepository> managerRepo, Mock<IBus> busMock) Build()
    {
        var busMock = new Mock<IBus>();
        var createTeamCommand = new CreateTeamCommand(){ManagerId = Guid.NewGuid(), TeamName = "test"};
        var createTeamCommandHandler = new CreateTeamCommandHandler(new Mock<ILoggerManager>().Object, _unitOfWorkFactoryMock.Object, busMock.Object);
        Task<Guid?> func() => createTeamCommandHandler.Handle(createTeamCommand, default);

        return (func, _managerRepoMock, busMock);
    }
}