using MongoDB.Bson;
using Domain.Organizers;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure;
public record MongoOrganizer
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }
    public OrganizerPersonalInfo PersonalInfo { get; set; } = null!;

    [BsonRepresentation(BsonType.String)]
    public Guid TournamentId { get; set; }
    public MongoTournament? Tournament { get; set; } = null!;
}
