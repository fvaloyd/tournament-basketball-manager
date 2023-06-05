using Domain.Common;
using Domain.Organizers;
using Application.Features.Organizers.Commands;

namespace Application.UnitTests.Features.Organizers.Commands;
public class CreateOrganizerCommandTests
{
    [Fact]
    public async Task ShouldCreateTheOrganizer()
    {
        var (createOrganizerCommandHandler, createOrganizerCommand, unitOfWorkMock, organizerRepoMock) = GetHandlerAndMocks();

        Guid organizerCreatedId = await createOrganizerCommandHandler.Handle(createOrganizerCommand, default);

        organizerCreatedId.Should().NotBeEmpty();
        unitOfWorkMock.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        organizerRepoMock.Verify(m => m.CreateAsync(It.IsAny<Organizer>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    static (
        CreateOrganizerCommandHandler createOrganizerCommandHandler,
        CreateOrganizerCommand createOrganizerCommand,
        Mock<IUnitOfWork> unitOfWorkMock,
        Mock<IOrganizerRepository> organizerRepoMock
    )
    GetHandlerAndMocks()
    {
        var unitOfWorkMock = UnitOfWorkMock.Instance;
        var organizerRepoMock = new Mock<IOrganizerRepository>();
        unitOfWorkMock.Setup(m => m.Organizers).Returns(organizerRepoMock.Object);
        var createOrganizerCommand = new CreateOrganizerCommand()
        {
            OrganizerPersonalInfo = new OrganizerPersonalInfo("", "", "", DateTime.Today, new Address("", "", "", "", ""))
        };
        var createOrganizerCommandHandler = new CreateOrganizerCommandHandler(unitOfWorkMock.Object);

        return(
            createOrganizerCommandHandler,
            createOrganizerCommand,
            unitOfWorkMock,
            organizerRepoMock
        );
    }
}