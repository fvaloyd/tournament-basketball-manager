using Refit;

namespace Shared;
public interface IPlayerClient
{
    [Get("/api/players")]
    Task<IEnumerable<PlayerResponse>> GetPlayersAsync();
    [Post("/api/players")]
    Task<Guid> CreateAsync([Body]object request);
}