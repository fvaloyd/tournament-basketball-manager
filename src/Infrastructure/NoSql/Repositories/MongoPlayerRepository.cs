using Mapster;
using MapsterMapper;
using Domain.Players;
using MongoDB.Driver;
using Infrastructure.NoSql.Models;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;

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
        => await _collection.AsQueryable().ProjectToType<Player>().ToListAsync();

    public async Task<Player> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var mongoPlayer = await _collection.AsQueryable().FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        return _mapper.Map<Player>(mongoPlayer!);
    }
}