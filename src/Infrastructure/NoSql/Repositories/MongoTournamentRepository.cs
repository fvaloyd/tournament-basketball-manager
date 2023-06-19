using AutoMapper;
using Domain.Organizers;
using Infrastructure.NoSql.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Infrastructure.NoSql.Repositories;
public class MongoTournamentRepository : ITournamentRepository
{
    private readonly IMongoCollection<MongoOrganizer> _collection;
    public readonly IMapper _mapper;
    public MongoTournamentRepository(IOptions<MongoDatabaseSettings> dbSettingsOptions, IMapper mapper)
    {
        var dbSettings = dbSettingsOptions.Value;
        var client = new MongoClient(dbSettings.ConnectionString);
        var db = client.GetDatabase(dbSettings.DataBaseName);
        _collection = db.GetCollection<MongoOrganizer>(dbSettings.OrganizerCollectionName);
        _mapper = mapper;
    }
    public async Task CreateAsync(Tournament tournament, CancellationToken cancellationToken = default)
    {
        var filter = Builders<MongoOrganizer>.Filter.Eq(o => o.Id, tournament.OrganizerId);
        var organizer = await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
        var organizerUpdated = organizer with { Tournament = _mapper.Map<MongoTournament>(tournament), TournamentId = tournament.Id };
        await _collection.ReplaceOneAsync(filter, organizerUpdated, cancellationToken: cancellationToken);
    }
}
