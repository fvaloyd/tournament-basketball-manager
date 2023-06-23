using Domain.Managers;

namespace Shared;
public record ManagerResponse
{
    public Guid Id { get; init; }
    public ManagerPersonalInfo PersonalInfo { get; init; } = null!;
    public Guid TeamId { get; init; }
    public TeamResponse? Team { get; init; }
}