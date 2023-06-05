using Domain.Common;
using MapsterMapper;
using Domain.Managers;
using Domain.Managers.Exceptions;
using Application.Features.Managers.DTOs;
using Application.Features.Managers.Queries;

namespace Application.UnitTests.Features.Managers.Queries;
public class GetManagerQueryTests
{
    [Fact]
    public void ShouldThrowAManagerNotFoundException_WhenManagerNotFound()
    {
        var (getManagerQueryHandler, getManagerQuery, managerRepoMock, _) = GetHandlerAndMocks(HandlerCallOption.NullManager);

        Func<Task<ManagerResponse>> handlerFunc = () => getManagerQueryHandler.Handle(getManagerQuery, default);

        handlerFunc.Should().ThrowAsync<ManagerNotFoundException>();
        managerRepoMock.Verify(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ShouldReturnAManagerResponse_WhenManagerWasFound()
    {
        var (getManagerQueryHandler, getManagerQuery, managerRepoMock, mapperMock) = GetHandlerAndMocks(HandlerCallOption.Valid);

        ManagerResponse managerResponse = await getManagerQueryHandler.Handle(getManagerQuery, default);

        managerRepoMock.Verify(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        mapperMock.Verify(m => m.Map<ManagerResponse>(It.IsAny<Manager>()), Times.Once);
    }

    static(
        GetManagerQueryHandler getManagerQueryHandler,
        GetManagerQuery getManagerQuery,
        Mock<IManagerRepository> managerRepoMock,
        Mock<IMapper> mapperMock
    )
    GetHandlerAndMocks(HandlerCallOption option)
    {
        Manager? manager = option switch{
            HandlerCallOption.NullManager => null,
            HandlerCallOption.Valid => Manager.Create(new ManagerPersonalInfo("test", "test", "test", DateTime.Now, new Address("test", "test", "test", "test", "test")), "teamName"),
            _ => throw new NotImplementedException()
        };
        var unitOfWorkMock = UnitOfWorkMock.Instance;
        var managerRepoMock = new Mock<IManagerRepository>();
        var mapperMock = new Mock<IMapper>();
        managerRepoMock.Setup(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()).Result).Returns(manager!);
        mapperMock.Setup(m => m.Map<ManagerResponse>(It.IsAny<Manager>())).Returns(new ManagerResponse());
        unitOfWorkMock.Setup(m => m.Managers).Returns(managerRepoMock.Object);
        var getManagerQuery = new GetManagerQuery(){ManagerId = Guid.NewGuid()};
        var getManagerQueryHandler = new GetManagerQueryHandler(unitOfWorkMock.Object, new Mock<ILoggerManager>().Object, mapperMock.Object);

        return(
            getManagerQueryHandler,
            getManagerQuery,
            managerRepoMock,
            mapperMock
        );
    }

    enum HandlerCallOption
    {
        NullManager,
        Valid
    }
}