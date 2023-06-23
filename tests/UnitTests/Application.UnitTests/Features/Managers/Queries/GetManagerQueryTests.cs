using Domain.Common;
using Domain.Managers;
using Domain.Managers.Exceptions;
using Application.Features.Managers.Queries;
using AutoMapper;
using Application.Features.Players;
using Application.Features.Organizers;
using Application.Features.Managers;
using Shared;

namespace Application.UnitTests.Features.Managers.Queries;
public class GetManagerQueryTests
{
    private readonly IMapper _mapper;

    public GetManagerQueryTests()
    {
        _mapper = new MapperConfiguration(x =>
        {
            x.AddProfile(new OrganizerMappingProfile());
            x.AddProfile(new ManagerMappingProfile());
            x.AddProfile(new PlayerMappingProfile());
        }).CreateMapper();
    }

    [Fact]
    public async Task ShouldThrowAManagerNotFoundException_WhenManagerNotFoundAsync()
    {
        var (getManagerQueryHandler, getManagerQuery, managerRepoMock) = GetHandlerAndMocks(HandlerCallOption.NullManager);

        Func<Task<ManagerResponse>> handlerFunc = () => getManagerQueryHandler.Handle(getManagerQuery, default);

        await handlerFunc.Should().ThrowAsync<ManagerNotFoundException>();
        managerRepoMock.Verify(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ShouldReturnAManagerResponse_WhenManagerWasFound()
    {
        var (getManagerQueryHandler, getManagerQuery, managerRepoMock) = GetHandlerAndMocks(HandlerCallOption.Valid);

        ManagerResponse managerResponse = await getManagerQueryHandler.Handle(getManagerQuery, default);

        managerRepoMock.Verify(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        managerResponse.Should().NotBeNull();
    }

    private (GetManagerQueryHandler getManagerQueryHandler, GetManagerQuery getManagerQuery, Mock<IManagerRepository> managerRepoMock) GetHandlerAndMocks(HandlerCallOption option)
    {
        Manager? manager = option switch{
            HandlerCallOption.NullManager => null,
            HandlerCallOption.Valid => Manager.Create(new ManagerPersonalInfo("test", "test", "test", DateTime.Now, "test", "test", "test", "test", "test"), "teamName"),
            _ => throw new NotImplementedException()
        };

        var unitOfWorkFactoryMock = new Mock<IUnitOfWorkFactory>();
        var unitOfWorkMock = UnitOfWorkMock.Instance;
        var managerRepoMock = new Mock<IManagerRepository>();
        var loggerMock = new Mock<ILoggerManager>();

        unitOfWorkFactoryMock.Setup(uowf => uowf.CreateUnitOfWork(It.IsAny<string>())).Returns(unitOfWorkMock.Object);
        managerRepoMock.Setup(m => m.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()).Result).Returns(manager!);
        unitOfWorkMock.Setup(m => m.Managers).Returns(managerRepoMock.Object);
        
        var getManagerQuery = new GetManagerQuery(){ManagerId = manager?.Id ?? Guid.NewGuid()};
        
        var getManagerQueryHandler = new GetManagerQueryHandler(unitOfWorkFactoryMock.Object, loggerMock.Object, _mapper);

        return(getManagerQueryHandler, getManagerQuery, managerRepoMock);
    }

    enum HandlerCallOption
    {
        NullManager,
        Valid
    }
}