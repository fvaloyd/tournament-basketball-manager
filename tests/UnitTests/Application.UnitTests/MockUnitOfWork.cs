using Domain.Common;
using Domain.Players;
using Domain.Managers;
using Domain.Organizers;

namespace Application.UnitTests;
public static class UnitOfWorkMock
{
    public static Mock<IUnitOfWork> Instance => SetUp();
    private static Mock<IUnitOfWork> SetUp()
    {
        var mock = new Mock<IUnitOfWork>();
        mock.Setup(m => m.Managers).Returns(new Mock<IManagerRepository>().Object);
        mock.Setup(m => m.Organizers).Returns(new Mock<IOrganizerRepository>().Object);
        mock.Setup(m => m.Players).Returns(new Mock<IPlayerRepository>().Object);
        mock.Setup(m => m.Teams).Returns(new Mock<ITeamRepository>().Object);
        return mock;
    }
}