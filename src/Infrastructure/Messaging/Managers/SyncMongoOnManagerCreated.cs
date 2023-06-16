using Application.Features.Managers.Commands;
using Domain.Common;
using Infrastructure.Sql.Repositories;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Messaging.Managers;
public class SyncMongoOnManagerCreated : IConsumer<ManagerCreatedEvent>
{
    private readonly IUnitOfWork _sqlUnitOfWork;
    private readonly IUnitOfWork _mongoUnitOfWork;

    public SyncMongoOnManagerCreated(IServiceProvider serviceProvider)
    {
        IEnumerable<IUnitOfWork> unitOfWorkImplementations = serviceProvider.GetServices<IUnitOfWork>();
        _sqlUnitOfWork = unitOfWorkImplementations.FirstOrDefault(s => s.GetType() == typeof(SqlUnitOfWork)) ?? throw new Exception("Service not registed.");
        _mongoUnitOfWork = unitOfWorkImplementations.FirstOrDefault(s => s.GetType() == typeof(MongoUnitOfWork)) ?? throw new Exception("Service not registed.");
    }
    public async Task Consume(ConsumeContext<ManagerCreatedEvent> context)
    {
        var managerCreated = await _sqlUnitOfWork.Managers.GetByIdAsync(context.Message.ManagerId, context.CancellationToken);
        await _mongoUnitOfWork.Managers.CreateAsync(managerCreated, context.CancellationToken);
    }
}
