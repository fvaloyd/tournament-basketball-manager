using AutoMapper;
using MongoDB.Driver;
using Domain.Players;
using Domain.Managers.Exceptions;
using Infrastructure.NoSql.Models;
using Microsoft.Extensions.Options;
using AutoMapper.QueryableExtensions;

namespace Infrastructure.NoSql.Repositories;
public class MongoPlayerRepository : IPlayerRepository
{
    private readonly IMongoCollection<MongoPlayer> _collection;
    public readonly IMapper _mapper;
    public MongoPlayerRepository(IOptions<MongoDatabaseSettings> dbSettingsOptions, IMapper mapper)
    {
        var dbSettings = dbSettingsOptions.Value;
        var client = new MongoClient(dbSettings.ConnectionString);
        var db = client.GetDatabase(dbSettings.DataBaseName);
        _collection = db.GetCollection<MongoPlayer>(dbSettings.PlayerCollectionName);
        _mapper = mapper;
    }

    public async Task CreateAsync(Player player, CancellationToken cancellationToken = default)
    {
        var mongoPlayer = _mapper.Map<MongoPlayer>(player);
        await _collection.InsertOneAsync(mongoPlayer, default, cancellationToken);
    }

    public async Task<IEnumerable<Player>> GetAllAsync(CancellationToken cancellationToken = default) 
        => _collection.AsQueryable().ProjectTo<Player>(_mapper.ConfigurationProvider);

    public async Task<Player> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<MongoPlayer>.Filter.Eq(m => m.TeamId, id);
        var mongoPlayer = await _collection.Find(filter).FirstOrDefaultAsync();
        return mongoPlayer is null
            ? throw new PlayerNotFoundException(id)
            : _mapper.Map<Player>(mongoPlayer);
    }
}