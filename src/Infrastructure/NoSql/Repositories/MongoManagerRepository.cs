using MongoDB.Driver;
using Domain.Managers;
using Infrastructure.NoSql.Models;
using Microsoft.Extensions.Options;
using AutoMapper;
using AutoMapper.QueryableExtensions;

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
        _collection = db.GetCollection<MongoManager>(dbSettings.ManagerCollectionName);
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
        var mongoManager = await _collection.Find(filter).SingleOrDefaultAsync(cancellationToken);
        return _mapper.Map<Manager>(mongoManager!);
    }

    public async Task UpdateAsync(Manager managerUpdated, CancellationToken cancellationToken)
    {
        var mongoManager = _mapper.Map<MongoManager>(managerUpdated);
        await _collection.ReplaceOneAsync(m => m.Id == managerUpdated.Id, mongoManager, cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<Manager>> GetByIds(IEnumerable<Guid> ids, CancellationToken cancellationToken)
    {
        var filter = Builders<MongoManager>.Filter.In(m => m.Id, ids);
        var mongoManagersCursor = await _collection.FindAsync(filter, default, cancellationToken);
        var mongoManagers = await mongoManagersCursor.ToListAsync(cancellationToken: cancellationToken);
        return mongoManagers.AsQueryable().ProjectTo<Manager>(_mapper.ConfigurationProvider);
    }
}
