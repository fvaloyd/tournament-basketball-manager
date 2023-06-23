using Domain.Players;
using Application.Features.Players;
using Application.Features.Players.Queries;
using AutoMapper;
using Domain.Common;
using Application.Features.Organizers;
using Application.Features.Managers;
using Shared;

namespace Application.UnitTests.Features.Players.Queries;
public class GetPlayersQueryTests
{
    private readonly IMapper _mapper;

    public GetPlayersQueryTests()
    {
        _mapper = new MapperConfiguration(x =>
        {
            x.AddProfile(new OrganizerMappingProfile());
            x.AddProfile(new ManagerMappingProfile());
            x.AddProfile(new PlayerMappingProfile());
        }).CreateMapper();
    }

    [Fact]
    public async Task ShouldReturnACollectionOfPlayerResponse()
    {
        var (getPlayersQueryHandler, getPlayersQuery, playerRepoMock) = GetHandlerAndMocks();

        IEnumerable<PlayerResponse> players = await getPlayersQueryHandler.Handle(getPlayersQuery, default);

        playerRepoMock.Verify(m => m.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        players.Should().NotBeEmpty();
    }

    private (GetPlayersQueryHandler getPlayersQueryHandler, GetPlayersQuery getPlayersQuery, Mock<IPlayerRepository> playerRepoMock) GetHandlerAndMocks()
    {
        List<Player> players = new()
        {
            Player.Create(new("player", "test", "test@test.com", DateTime.Now, 1.80f, 80.5f, "RD", "SJO", "S", "57", "93000"), Position.PointGuard)
        };

        var unitOfWorkFactoryMock = new Mock<IUnitOfWorkFactory>();
        var unitOfWorkMock = UnitOfWorkMock.Instance;
        var playerRepoMock = new Mock<IPlayerRepository>();

        playerRepoMock.Setup(m => m.GetAllAsync(It.IsAny<CancellationToken>()).Result).Returns(players);
        unitOfWorkMock.Setup(m => m.Players).Returns(playerRepoMock.Object);
        unitOfWorkFactoryMock.Setup(uowf => uowf.CreateUnitOfWork(It.IsAny<string>())).Returns(unitOfWorkMock.Object);
        
        var getPlayersQuery = new GetPlayersQuery();
        var getPlayersQueryHandler = new GetPlayersQueryHandler(unitOfWorkFactoryMock.Object, _mapper);

        return (getPlayersQueryHandler, getPlayersQuery, playerRepoMock);
    }
}