using MongoDB.Bson;
using Infrastructure.NoSql.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure;
public record MongoTournament
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public IList<MongoTeam> Teams { get; init; } = new List<MongoTeam>();
    public IList<MongoMatch> Matches { get; set; } = new List<MongoMatch>();

    [BsonRepresentation(BsonType.String)]
    public Guid OrganizerId { get; init; }
}
