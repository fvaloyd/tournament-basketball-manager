using Application.Features.Managers.Commands;
using AutoMapper;
using Domain.Common;
using Domain.Managers;
using Infrastructure.NoSql.Models;
using Infrastructure.Sql.Repositories;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Messaging.Managers;
public class SyncMongoOnTeamCreated : IConsumer<TeamCreatedEvent>
{
    private readonly IUnitOfWork _sqlUnitOfWork;
    private readonly IUnitOfWork _mongoUnitOfWork;

    public SyncMongoOnTeamCreated(IServiceProvider serviceProvider)
    {
        IEnumerable<IUnitOfWork> unitOfWorkImplementations = serviceProvider.GetServices<IUnitOfWork>();
        _sqlUnitOfWork = unitOfWorkImplementations.FirstOrDefault(s => s.GetType() == typeof(SqlUnitOfWork)) ?? throw new Exception("Service not registed.");
        _mongoUnitOfWork = unitOfWorkImplementations.FirstOrDefault(s => s.GetType() == typeof(MongoUnitOfWork)) ?? throw new Exception("Service not registed.");
    }
    public async Task Consume(ConsumeContext<TeamCreatedEvent> context)
    {
        var managerUpdated = await _sqlUnitOfWork.Managers.GetByIdAsync(context.Message.ManagerId, context.CancellationToken);
        await _mongoUnitOfWork.Managers.UpdateAsync(managerUpdated, context.CancellationToken);
    }
}