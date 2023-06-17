using Application.Features.Organizers.Commands;
using Domain.Common;
using Infrastructure.Sql.Repositories;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Messaging.Organizers;
public class SyncMongoOnTournamentCreated : IConsumer<TournamentCreatedEvent>
{
    private readonly IUnitOfWork _sqlUnitOfWork;
    private readonly IUnitOfWork _mongoUnitOfWork;

    public SyncMongoOnTournamentCreated(IServiceProvider serviceProvider)
    {
        IEnumerable<IUnitOfWork> unitOfWorkImplementations = serviceProvider.GetServices<IUnitOfWork>();
        _sqlUnitOfWork = unitOfWorkImplementations.FirstOrDefault(s => s.GetType() == typeof(SqlUnitOfWork)) ?? throw new Exception("Service not registed.");
        _mongoUnitOfWork = unitOfWorkImplementations.FirstOrDefault(s => s.GetType() == typeof(MongoUnitOfWork)) ?? throw new Exception("Service not registed.");
    }
    public async Task Consume(ConsumeContext<TournamentCreatedEvent> context)
    {
        var organizer = await _sqlUnitOfWork.Organizers.GetByIdAsync(context.Message.OrganizerId, context.CancellationToken);
        await _mongoUnitOfWork.Organizers.UpdateAsync(organizer, context.CancellationToken);
    }
}
