using MongoDB.Bson;
using Domain.Players;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.NoSql.Models;
public record MongoPlayer
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; init; }
    public PlayerPersonalInfo PersonalInfo { get; init; } = null!;
    public Position Position { get; init; }

    [BsonRepresentation(BsonType.String)]
    public Guid TeamId { get; set; }
}