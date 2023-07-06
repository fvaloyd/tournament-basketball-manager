namespace Domain.Organizers;
public interface IOrganizerRepository
{
    Task CreateAsync(Organizer organizer, CancellationToken cancellationToken = default);
    Task<Organizer> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task UpdateAsync(Organizer organizerUpdated, CancellationToken cancellationToken = default);
    Task<IEnumerable<Organizer>> GetAllOrganizersAsync(CancellationToken cancellationToken = default);
}