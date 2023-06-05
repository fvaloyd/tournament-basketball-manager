using Domain.Players;
using Domain.Managers;
using Domain.Organizers;

namespace Domain.Common;
public interface IUnitOfWork
{
    IManagerRepository Managers { get; }
    IPlayerRepository Players { get; }
    IOrganizerRepository Organizers { get; }
    ITeamRepository Teams { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}