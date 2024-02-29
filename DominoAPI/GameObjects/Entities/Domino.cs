using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DominoAPI.GameObjects;

public class Domino
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? id { get; set; } = null!;

    public int sideOne { get; set; } = 0;

    public int sideTwo { get; set; } = 0;

    public bool isDouble { get; set; } = false;
}