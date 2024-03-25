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

    public int TotalGameWins { get; set;} = 0;

    public int TotalRoundWins { get; set;} = 0;

    public double AveragePointsPerGame {get; set;} = 0.0;

    public double AveragePointsPerRound {get; set;} = 0.0;

    public int TotalPoints {get; set;} = 0;

    public int WinRanking {get; set;} = 0;

    public int PointRanking {get; set;} = 0;

    public String ImageLink {get; set;} = String.Empty;
}