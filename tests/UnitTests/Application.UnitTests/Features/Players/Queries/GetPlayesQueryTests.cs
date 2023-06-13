using Domain.Players;
using Application.Features.Players;
using Application.Features.Players.Queries;
using AutoMapper;
using Domain.Common;

namespace Application.UnitTests.Features.Players.Queries;
public class GetPlayersQueryTests
{
    [Fact]
    public async Task ShouldReturnACollectionOfPlayerResponse()
    {
        var (getPlayersQueryHandler, getPlayersQuery, playerRepoMock, mapperMock) = GetHandlerAndMocks();

        IEnumerable<PlayerResponse> result = await getPlayersQueryHandler.Handle(getPlayersQuery, default);

        playerRepoMock.Verify(m => m.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        mapperMock.Verify(m => m.Map<IEnumerable<PlayerResponse>>(It.IsAny<IEnumerable<Player>>()), Times.Once);
    }

    static (
        GetPlayersQueryHandler getPlayersQueryHandler,
        GetPlayersQuery getPlayersQuery,
        Mock<IPlayerRepository> playerRepoMock,
        Mock<IMapper> mapperMock
    )
    GetHandlerAndMocks()
    {
        var unitOfWorkFactoryMock = new Mock<IUnitOfWorkFactory>();
        var unitOfWorkMock = UnitOfWorkMock.Instance;
        unitOfWorkFactoryMock.Setup(uowf => uowf.CreateUnitOfWork(It.IsAny<string>())).Returns(unitOfWorkMock.Object);
        var mapperMock = new Mock<IMapper>();
        var playerRepoMock = new Mock<IPlayerRepository>();
        playerRepoMock.Setup(m => m.GetAllAsync(It.IsAny<CancellationToken>()).Result).Returns(new List<Player>());
        unitOfWorkMock.Setup(m => m.Players).Returns(playerRepoMock.Object);
        var getPlayersQuery = new GetPlayersQuery();
        var getPlayersQueryHandler = new GetPlayersQueryHandler(unitOfWorkFactoryMock.Object, mapperMock.Object);

        return(
            getPlayersQueryHandler,
            getPlayersQuery,
            playerRepoMock,
            mapperMock
        );
    }
}