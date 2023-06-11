using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.NoSql.Models;
public record MongoTeam
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; init; }
    public string Name { get; set; } = string.Empty;
    public IList<MongoPlayer> Players { get; init; } = new List<MongoPlayer>();

    [BsonRepresentation(BsonType.String)]
    public Guid TournamentId { get; init; }

    [BsonRepresentation(BsonType.String)]
    public Guid ManagerId { get; init; }
}