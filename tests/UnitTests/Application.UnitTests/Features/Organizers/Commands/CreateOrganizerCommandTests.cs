using Domain.Common;
using Domain.Organizers;
using Application.Features.Organizers.Commands;
using MassTransit;

namespace Application.UnitTests.Features.Organizers.Commands;
public class CreateOrganizerCommandTests
{
    [Fact]
    public async Task ShouldCreateTheOrganizer()
    {
        var (createOrganizerCommandHandler, createOrganizerCommand, unitOfWorkMock, organizerRepoMock, busMock) = GetHandlerAndMocks();

        Guid organizerCreatedId = await createOrganizerCommandHandler.Handle(createOrganizerCommand, default);

        organizerCreatedId.Should().NotBeEmpty();
        unitOfWorkMock.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        organizerRepoMock.Verify(m => m.CreateAsync(It.IsAny<Organizer>(), It.IsAny<CancellationToken>()), Times.Once);
        busMock.Verify(m => m.Publish(It.IsAny<OrganizerCreatedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    static (
        CreateOrganizerCommandHandler createOrganizerCommandHandler,
        CreateOrganizerCommand createOrganizerCommand,
        Mock<IUnitOfWork> unitOfWorkMock,
        Mock<IOrganizerRepository> organizerRepoMock,
        Mock<IBus> busMock
    )
    GetHandlerAndMocks()
    {
        var busMock = new Mock<IBus>();
        var unitOfWorkFactoryMock = new Mock<IUnitOfWorkFactory>();
        var unitOfWorkMock = UnitOfWorkMock.Instance;
        unitOfWorkFactoryMock.Setup(uowf => uowf.CreateUnitOfWork(It.IsAny<string>())).Returns(unitOfWorkMock.Object);
        var organizerRepoMock = new Mock<IOrganizerRepository>();
        unitOfWorkMock.Setup(m => m.Organizers).Returns(organizerRepoMock.Object);
        var createOrganizerCommand = new CreateOrganizerCommand()
        {
            OrganizerPersonalInfo = new OrganizerPersonalInfo("", "", "", DateTime.Today, "", "", "", "", "")
        };
        var createOrganizerCommandHandler = new CreateOrganizerCommandHandler(unitOfWorkFactoryMock.Object, busMock.Object);

        return(
            createOrganizerCommandHandler,
            createOrganizerCommand,
            unitOfWorkMock,
            organizerRepoMock,
            busMock
        );
    }
}