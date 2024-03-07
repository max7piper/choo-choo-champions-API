using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DominoAPI.UserObjects;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; } = null!;

    public string Username { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public int Wins { get; set;} = 0;

    public int Losses { get; set;} = 0;

    public double AverageScore {get; set;} = 0.0;
}