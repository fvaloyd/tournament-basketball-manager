using Application.Features.Managers;
using Application.Features.Managers.Queries;
using Application.Features.Organizers;
using Application.Features.Players;
using AutoMapper;
using Domain.Common;
using Domain.Managers;
using Shared;

namespace Application.UnitTests.Features.Managers.Queries;
public class GetManagersQueryTests
{
    private readonly IMapper _mapper;

    public GetManagersQueryTests()
    {
        _mapper = new MapperConfiguration(x =>
        {
            x.AddProfile(new OrganizerMappingProfile());
            x.AddProfile(new ManagerMappingProfile());
            x.AddProfile(new PlayerMappingProfile());
        }).CreateMapper();
    }

    [Fact]
    public async void ShouldReturnACollectionOfManagerResponse()
    {
        var (getManagersQueryHandler, getManagersQuery, managerRepoMock) = GetHandlerAndMocks();

        IEnumerable<ManagerResponse> managers = await getManagersQueryHandler.Handle(getManagersQuery, default);

        managerRepoMock.Verify(m => m.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        managers.Should().NotBeEmpty();
    }

    private (GetManagersQueryHandler getManagerQueryHandler, GetManagersQuery getManagersQuery, Mock<IManagerRepository> organizerRepoMock) GetHandlerAndMocks()
    {
        List<Manager> managers = new()
        {
            Manager.Create(new("test1", "test", "test", DateTime.Now, "", "", "", "", "")),
            Manager.Create(new("test2", "test", "test", DateTime.Now, "", "", "", "", ""))
        };

        var unitOfWorkFactoryMock = new Mock<IUnitOfWorkFactory>();
        var unitOfWorkMock = UnitOfWorkMock.Instance;
        var managerRepoMock = new Mock<IManagerRepository>();

        unitOfWorkFactoryMock.Setup(uowf => uowf.CreateUnitOfWork(It.IsAny<string>())).Returns(unitOfWorkMock.Object);
        managerRepoMock.Setup(m => m.GetAllAsync(It.IsAny<CancellationToken>()).Result).Returns(managers);
        unitOfWorkMock.Setup(m => m.Managers).Returns(managerRepoMock.Object);

        var getManagersQuery = new GetManagersQuery();
        var getManagersQueryHandler = new GetManagersQueryHandler(unitOfWorkFactoryMock.Object, _mapper);

        return (getManagersQueryHandler, getManagersQuery, managerRepoMock);
    }
}
