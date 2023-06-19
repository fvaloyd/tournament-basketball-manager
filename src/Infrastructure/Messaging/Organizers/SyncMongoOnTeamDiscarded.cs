using Application.Features.Organizers.Commands;
using Domain.Common;
using Infrastructure.Sql.Repositories;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Messaging.Organizers;
public class SyncMongoOnTeamDiscarded : IConsumer<TeamDiscardedEvent>
{
    private readonly IUnitOfWork _sqlUnitOfWork;
    private readonly IUnitOfWork _mongoUnitOfWork;

    public SyncMongoOnTeamDiscarded(IServiceProvider serviceProvider)
    {
        IEnumerable<IUnitOfWork> unitOfWorkImplementations = serviceProvider.GetServices<IUnitOfWork>();
        _sqlUnitOfWork = unitOfWorkImplementations.FirstOrDefault(s => s.GetType() == typeof(SqlUnitOfWork)) ?? throw new Exception("Service not registed.");
        _mongoUnitOfWork = unitOfWorkImplementations.FirstOrDefault(s => s.GetType() == typeof(MongoUnitOfWork)) ?? throw new Exception("Service not registed.");
    }
    public async Task Consume(ConsumeContext<TeamDiscardedEvent> context)
    {
        var organizer = await _sqlUnitOfWork.Organizers.GetByIdAsync(context.Message.OrganizerId, context.CancellationToken);
        await _mongoUnitOfWork.Organizers.UpdateAsync(organizer, context.CancellationToken);
        var team = await _sqlUnitOfWork.Teams.GetByIdAsync(context.Message.TeamId, context.CancellationToken);
        var manager = await _sqlUnitOfWork.Managers.GetByIdAsync(team.ManagerId, context.CancellationToken);
        await _mongoUnitOfWork.Managers.UpdateAsync(manager, context.CancellationToken);
    }
}
