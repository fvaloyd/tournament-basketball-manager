namespace Domain.Managers;
public interface ITeamRepository
{
    Task<Team> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}