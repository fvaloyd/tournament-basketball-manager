using MassTransit;
using Domain.Common;
using Infrastructure.Sql.Repositories;
using Application.Features.Players.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Messaging;
public class SyncMongoOnPlayerCreated : IConsumer<PlayerCreatedEvent>
{
    private readonly IUnitOfWork _sqlUnitOfWork;
    private readonly IUnitOfWork _mongoUnitOfWork;

    public SyncMongoOnPlayerCreated(IServiceProvider serviceProvider)
    {
        IEnumerable<IUnitOfWork> unitOfWorkImplementations = serviceProvider.GetServices<IUnitOfWork>();
        _sqlUnitOfWork = unitOfWorkImplementations.FirstOrDefault(s => s.GetType() == typeof(SqlUnitOfWork)) ?? throw new Exception("Service not registed.");
        _mongoUnitOfWork = unitOfWorkImplementations.FirstOrDefault(s => s.GetType() == typeof(MongoUnitOfWork)) ?? throw new Exception("Service not registed.");
    }

    public async Task Consume(ConsumeContext<PlayerCreatedEvent> context)
    {
        var playerCreated = await _sqlUnitOfWork.Players.GetByIdAsync(context.Message.PlayerId, context.CancellationToken);
        await _mongoUnitOfWork.Players.CreateAsync(playerCreated, context.CancellationToken);
    }
}