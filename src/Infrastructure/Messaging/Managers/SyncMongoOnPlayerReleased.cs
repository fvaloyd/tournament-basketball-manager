using Application.Features.Managers.Commands;
using Domain.Common;
using Infrastructure.Sql.Repositories;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Messaging.Managers;
public class SyncMongoOnPlayerReleased : IConsumer<PlayerReleasedEvent>
{
    private readonly IUnitOfWork _sqlUnitOfWork;
    private readonly IUnitOfWork _mongoUnitOfWork;

    public SyncMongoOnPlayerReleased(IServiceProvider serviceProvider)
    {
        IEnumerable<IUnitOfWork> unitOfWorkImplementations = serviceProvider.GetServices<IUnitOfWork>();
        _sqlUnitOfWork = unitOfWorkImplementations.FirstOrDefault(s => s.GetType() == typeof(SqlUnitOfWork)) ?? throw new Exception("Service not registed.");
        _mongoUnitOfWork = unitOfWorkImplementations.FirstOrDefault(s => s.GetType() == typeof(MongoUnitOfWork)) ?? throw new Exception("Service not registed.");
    }
    public async Task Consume(ConsumeContext<PlayerReleasedEvent> context)
    {
        var manager = await _sqlUnitOfWork.Managers.GetByIdAsync(context.Message.ManagerId, context.CancellationToken);
        var player = await _sqlUnitOfWork.Players.GetByIdAsync(context.Message.PlayerId, context.CancellationToken);
        await _mongoUnitOfWork.Managers.UpdateAsync(manager, context.CancellationToken);
        await _mongoUnitOfWork.Players.UpdateAsync(player, context.CancellationToken);
    }
}
