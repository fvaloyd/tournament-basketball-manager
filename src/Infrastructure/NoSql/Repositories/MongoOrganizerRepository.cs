using AutoMapper;
using MongoDB.Driver;
using Domain.Organizers;
using Microsoft.Extensions.Options;
using AutoMapper.QueryableExtensions;

namespace Infrastructure;
public class MongoOrganizerRepository : IOrganizerRepository
{
    private readonly IMongoCollection<MongoOrganizer> _collection;
    public readonly IMapper _mapper;
    public MongoOrganizerRepository(IOptions<MongoDatabaseSettings> dbSettingsOptions, IMapper mapper)
    {
        var dbSettings = dbSettingsOptions.Value;
        var client = new MongoClient(dbSettings.ConnectionString);
        var db = client.GetDatabase(dbSettings.DataBaseName);
        _collection = db.GetCollection<MongoOrganizer>(dbSettings.OrganizerCollectionName);
        _mapper = mapper;
    }

    public async Task CreateAsync(Organizer organizer, CancellationToken cancellationToken = default)
    {
        var mongoOrganizer = _mapper.Map<MongoOrganizer>(organizer);
        await _collection.InsertOneAsync(mongoOrganizer, default, cancellationToken);
    }

    public async Task<Organizer> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<MongoOrganizer>.Filter.Eq(m => m.Id, id);
        var mongoOrganizer = await _collection.Find(filter).SingleOrDefaultAsync(cancellationToken);
        return _mapper.Map<Organizer>(mongoOrganizer);
    }

    public async Task UpdateAsync(Organizer organizerUpdated, CancellationToken cancellationToken)
    {
        var mongoOrganizer = _mapper.Map<MongoOrganizer>(organizerUpdated);
        await _collection.ReplaceOneAsync(o => o.Id == organizerUpdated.Id, mongoOrganizer, cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<Organizer>> GetAllOrganizersAsync(CancellationToken cancellationToken = default)
        => _collection.AsQueryable().ProjectTo<Organizer>(_mapper.ConfigurationProvider);
}
