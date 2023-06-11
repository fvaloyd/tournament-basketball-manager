using Infrastructure.NoSql.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure;
public record MongoMatch
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public Guid TournamentId { get; set; }
    public MongoTeam TeamA { get; set; } = null!;
    public MongoTeam TeamB { get; set; } = null!;
}
