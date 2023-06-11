using MongoDB.Bson;
using Domain.Managers;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.NoSql.Models;
public record MongoManager
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; init; }
    public ManagerPersonalInfo ManagerPersonalInfo { get; init; } = null!;
    
    [BsonRepresentation(BsonType.String)]
    public Guid TeamId { get; init; }
    public MongoTeam? Team { get; init; }
}