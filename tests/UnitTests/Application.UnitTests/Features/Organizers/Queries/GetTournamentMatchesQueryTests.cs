using MapsterMapper;
using Domain.Common;
using Application.Features.Organizers.DTOs;
using Application.Features.Organizers.Queries;
using Domain.Organizers;

namespace Application.UnitTests.Features.Organizers.Queries;
public class GetTournamentMatchesTests
{
    [Fact]
    public async Task ShouldReturnACollectionOfMatchesResponse()
    {
        var unitOfWorkMock = UnitOfWorkMock.Instance;
        var organizerRepoMock = new Mock<IOrganizerRepository>();
        organizerRepoMock.Setup(m => m.GetTournamentMatches(It.IsAny<Guid>(), It.IsAny<CancellationToken>()).Result).Returns(new List<Domain.Organizers.Match>());
        unitOfWorkMock.Setup(m => m.Organizers).Returns(organizerRepoMock.Object);
        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(m => m.Map<IEnumerable<MatchResponse>>(It.IsAny<IEnumerable<Domain.Organizers.Match>>())).Returns(new List<MatchResponse>());
        var getTournamentMatchesQuery = new GetTournamentMatchesQuery();
        var getTournamentMatchesQueryHandler = new GetTournamentMatchesQueryHandler(unitOfWorkMock.Object, new Mock<ILoggerManager>().Object, mapperMock.Object);

        IEnumerable<MatchResponse> result = await getTournamentMatchesQueryHandler.Handle(getTournamentMatchesQuery, default);

        organizerRepoMock.Verify(m => m.GetTournamentMatches(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        mapperMock.Verify(m => m.Map<IEnumerable<MatchResponse>>(It.IsAny<IEnumerable<Domain.Organizers.Match>>()), Times.Once);
    }
}