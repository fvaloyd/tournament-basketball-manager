namespace Domain.Players;
public interface IPlayerRepository
{
    Task<IEnumerable<Player>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Player> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task CreateAsync(Player player, CancellationToken cancellationToken = default);
    Task UpdateAsync(Player playerUpdated, CancellationToken cancellationToken = default);
    Task<IEnumerable<Player>> GetByIdsAsync(IEnumerable<Guid> Ids, CancellationToken cancellationToken = default);
}