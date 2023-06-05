using Domain.Common;
using Domain.Managers;
using Domain.Managers.Exceptions;
using Application.Features.Managers.Commands;

namespace Application.UnitTests.Features.Managers.Commands;
public class CreateTeamCommandTests
{
    [Fact]
    public async Task ShouldThrowAnManagerNotFoundException_WhenNoManagerNotFound()
    {
        var unitOfWorkMock = UnitOfWorkMock.Instance;
        var (CreateTeamCommandHandlerFunc, mockManagerRepo) = new CreateTeamCommandHandlerBuilder(unitOfWorkMock).WithNulldManager().Build();

        await CreateTeamCommandHandlerFunc.Should().ThrowAsync<ManagerNotFoundException>();
        mockManagerRepo.Verify(m => m.GetByIdAsync(It.IsAny<Guid>(), default), Times.Once);
        unitOfWorkMock.Verify(u => u.SaveChangesAsync(default), Times.Never);
    }

    [Fact]
    public async Task ShouldReturnTheTeamIdCreated_WhenValidHandlerIsCall()
    {
        var unitOfWorkMock = UnitOfWorkMock.Instance;
        var (CreateTeamCommandHandlerFunc, mockManagerRepo) = new CreateTeamCommandHandlerBuilder(unitOfWorkMock).WithValidManager().Build();

        var teamId = await CreateTeamCommandHandlerFunc();

        teamId.Should().NotBeEmpty();
        mockManagerRepo.Verify(m => m.GetByIdAsync(It.IsAny<Guid>(), default), Times.Once);
        unitOfWorkMock.Verify(u => u.SaveChangesAsync(default), Times.Once);
    }
}

public class CreateTeamCommandHandlerBuilder
{
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<IManagerRepository> _managerRepoMock = new();
    public CreateTeamCommandHandlerBuilder(Mock<IUnitOfWork> unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _unitOfWork.Setup(m => m.Managers).Returns(_managerRepoMock.Object);
    }

    public CreateTeamCommandHandlerBuilder WithNulldManager()
    {
        _managerRepoMock.Setup(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()).Result).Returns(default(Manager)!);
        return this;
    }

    public CreateTeamCommandHandlerBuilder WithValidManager()
    {
        var manager = Manager.Create(new ManagerPersonalInfo("test", "", "", DateTime.Today, new Domain.Common.Address("", "", "", "", "")));
        _managerRepoMock.Setup(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()).Result).Returns(manager);
        return this;
    }

    public (Func<Task<Guid>> func, Mock<IManagerRepository> managerRepo) Build()
    {
        var createTeamCommand = new CreateTeamCommand(){ManagerId = Guid.NewGuid(), TeamName = "test"};
        var createTeamCommandHandler = new CreateTeamCommandHandler(new Mock<ILoggerManager>().Object, _unitOfWork.Object);
        Task<Guid> func() => createTeamCommandHandler.Handle(createTeamCommand, default);

        return (func, _managerRepoMock);
    }
}