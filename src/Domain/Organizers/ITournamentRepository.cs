namespace Domain.Organizers;
public interface ITournamentRepository
{
    Task CreateAsync(Tournament tournament, CancellationToken cancellationToken = default);
}
