using Domain.Common;
using Infrastructure.Sql.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Common;
public class HandlersUnitOfWorkFactory : IUnitOfWorkFactory
{
    private readonly IServiceProvider _serviceProvider;

    public HandlersUnitOfWorkFactory(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    public IUnitOfWork CreateUnitOfWork(string criteria)
    {
        IEnumerable<IUnitOfWork> unitOfWorkImplementations = _serviceProvider.GetServices<IUnitOfWork>();
        return criteria.EndsWith("CommandHandler", StringComparison.InvariantCultureIgnoreCase)
            ? unitOfWorkImplementations.FirstOrDefault(s => s.GetType() == typeof(SqlUnitOfWork)) ?? throw new Exception("Service not registed.")
            : unitOfWorkImplementations.FirstOrDefault(s => s.GetType() == typeof(MongoUnitOfWork)) ?? throw new Exception("Service not registed.");
    }
}