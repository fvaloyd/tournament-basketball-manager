using MapsterMapper;
using MongoDB.Driver;
using Domain.Managers;
using Infrastructure.NoSql.Models;
using Microsoft.Extensions.Options;
using Domain.Organizers.Exceptions;

namespace Infrastructure;
public class MongoTeamRepository : ITeamRepository
{
    private readonly IMongoCollection<MongoManager> _collection;
    public readonly IMapper _mapper;
    public MongoTeamRepository(IOptions<MongoDatabaseSettings> dbSettingsOptions, IMapper mapper)
    {
        var dbSettings = dbSettingsOptions.Value;
        var client = new MongoClient(dbSettings.ConnectionString);
        var db = client.GetDatabase(dbSettings.DataBaseName);
        _collection = db.GetCollection<MongoManager>(dbSettings.PlayerCollectionName);
        _mapper = mapper;
    }
    public async Task<Team> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var filter = Builders<MongoManager>.Filter.Eq(m => m.TeamId, id);
        var organizer = await _collection.Find(filter).FirstOrDefaultAsync();
        return organizer.Team is null
            ? throw new TeamNotFoundException(id)
            : _mapper.Map<Team>(organizer.Team!);
    }
}
