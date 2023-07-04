using Refit;

namespace Shared;
public interface IManagerClient
{
    [Get("/api/managers/{id}")]
    Task<ManagerResponse> GetManagerAsync(Guid id);
    [Post("/api/managers")]
    Task<Guid> CreateAsync([Body]object request);
    [Post("/api/managers/{id}/teams")]
    Task<Guid> CreateTeamAsync(Guid id, string teamName);
    [Delete("/api/managers/{id}/teams")]
    Task DissolveTeamAsync(Guid id, Guid teamId);
    [Post("/api/managers/{id}/teams/players/{playerId}")]
    Task DraftPlayerAsync(Guid id, Guid playerId);
    [Delete("/api/managers/{id}/teams/players/{playerId}")]
    Task ReleasePlayerAsync(Guid id, Guid playerId);
}