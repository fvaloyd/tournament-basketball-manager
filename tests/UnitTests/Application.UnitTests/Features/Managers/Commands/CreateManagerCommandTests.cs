using Domain.Common;
using Domain.Managers;
using Application.Features.Managers.Commands;

namespace Application.UnitTests.Features.Managers.Commands;
public class CreateManagerCommandTests
{
    [Fact]
    public async Task ShouldReturnTheManagerCreatedId_WhenValidManagerPersonalInfoIsPassed()
    {
        var (createManagerCommandHandler, createManagerCommand, unitOfWorkMock, managerRepoMock) = GetHandlerAndMocks();

        Guid managerCreatedId = await createManagerCommandHandler.Handle(createManagerCommand, default);

        managerCreatedId.Should().NotBeEmpty();
        managerRepoMock.Verify(mr => mr.CreateAsync(It.IsAny<Manager>(), default), Times.Once);
        unitOfWorkMock.Verify(tuow => tuow.SaveChangesAsync(default), Times.Once);
    }

    static(
        CreateManagerCommandHandler createManagerCommandHandler,
        CreateManagerCommand createManagerCommand,
        Mock<IUnitOfWork> unitOfWorkMock,
        Mock<IManagerRepository> managerRepoMock
    )
    GetHandlerAndMocks()
    {
        var unitOfWorkFactoryMock = new Mock<IUnitOfWorkFactory>();
        var unitOfWorkMock = UnitOfWorkMock.Instance;
        unitOfWorkFactoryMock.Setup(uowf => uowf.CreateUnitOfWork(It.IsAny<string>())).Returns(unitOfWorkMock.Object);
        var managerRepoMock = new Mock<IManagerRepository>();
        unitOfWorkMock.Setup(m => m.Managers).Returns(managerRepoMock.Object);
        var validManagerPersonalInfo = new ManagerPersonalInfo("test", "test", "test@gmail.com", DateTime.Today, "test", "test", "test", "test", "test");
        var createManagerCommand = new CreateManagerCommand{ManagerPersonalInfo = validManagerPersonalInfo};
        var createManagerCommandHandler = new CreateManagerCommandHandler(unitOfWorkFactoryMock.Object);
        return(
            createManagerCommandHandler,
            createManagerCommand,
            unitOfWorkMock,
            managerRepoMock
        );
    }
}