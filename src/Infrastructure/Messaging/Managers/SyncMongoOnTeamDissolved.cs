using Application.Features.Managers.Commands;
using Domain.Common;
using Infrastructure.Sql.Repositories;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Messaging.Managers;
public class SyncMongoOnTeamDissolved : IConsumer<TeamDissolvedEvent>
{
    private readonly IUnitOfWork _sqlUnitOfWork;
    private readonly IUnitOfWork _mongoUnitOfWork;

    public SyncMongoOnTeamDissolved(IServiceProvider serviceProvider)
    {
        IEnumerable<IUnitOfWork> unitOfWorkImplementations = serviceProvider.GetServices<IUnitOfWork>();
        _sqlUnitOfWork = unitOfWorkImplementations.FirstOrDefault(s => s.GetType() == typeof(SqlUnitOfWork)) ?? throw new Exception("Service not registed.");
        _mongoUnitOfWork = unitOfWorkImplementations.FirstOrDefault(s => s.GetType() == typeof(MongoUnitOfWork)) ?? throw new Exception("Service not registed.");
    }
    public async Task Consume(ConsumeContext<TeamDissolvedEvent> context)
    {
        var manager = await _sqlUnitOfWork.Managers.GetByIdAsync(context.Message.ManagerId, context.CancellationToken);
        await _mongoUnitOfWork.Managers.UpdateAsync(manager, context.CancellationToken);
    }
}