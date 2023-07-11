using Refit;

namespace Shared;
public interface IOrganizerClient
{
    [Post("/api/organizers")]
    Task<Guid> CreateAsync([Body]object request);

    [Get("/api/organizers")]
    Task<IEnumerable<OrganizerResponse>> GetAllAsync();

    [Post("/api/organizers/{id}/tournaments")]
    Task<Guid> CreateTournamentAsync(Guid id, string tournamentName);

    [Post("/api/organizers/{id}/tournaments/teams/{teamId}")]
    Task RegisterTeamAsync(Guid id, Guid teamId);

    [Delete("/api/organizers/{id}/tournaments/teams/{teamId}")]
    Task DiscardTeamAsync(Guid id, Guid teamId);

    [Post("/api/organizers/{id}/tournaments/matches")]
    Task MatchTeamsAsync(Guid id);

    [Delete("/api/organizers/{id}/tournaments")]
    Task FinishTournamentAsync(Guid id);

    [Get("/api/organizers/{id}")]
    Task<OrganizerResponse> GetOrganizerAsync(string id);

    [Get("/api/organizers/{id}/tournaments/matches")]
    Task<IEnumerable<MatchResponse>> GetTournamentMatchesAsync(Guid id);
}