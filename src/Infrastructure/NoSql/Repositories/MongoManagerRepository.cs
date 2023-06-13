using MongoDB.Driver;
using Domain.Managers;
using Infrastructure.NoSql.Models;
using Microsoft.Extensions.Options;
using AutoMapper;

namespace Infrastructure;
public class MongoManagerRepository : IManagerRepository
{
    private readonly IMongoCollection<MongoManager> _collection;
    public readonly IMapper _mapper;
    public MongoManagerRepository(IOptions<MongoDatabaseSettings> dbSettingsOptions, IMapper mapper)
    {
        var dbSettings = dbSettingsOptions.Value;
        var client = new MongoClient(dbSettings.ConnectionString);
        var db = client.GetDatabase(dbSettings.DataBaseName);
        _collection = db.GetCollection<MongoManager>(dbSettings.PlayerCollectionName);
        _mapper = mapper;
    }
    public async Task CreateAsync(Manager manager, CancellationToken cancellationToken = default)
    {
        var mongoManager = _mapper.Map<MongoManager>(manager);
        await _collection.InsertOneAsync(mongoManager, default, cancellationToken);
    }

    public async Task<Manager> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<MongoManager>.Filter.Eq(m => m.Id, id);
        var mongoManager = await _collection.Find(filter).FirstOrDefaultAsync();
        return _mapper.Map<Manager>(mongoManager!);
    }
}
