using DominoAPI.GameObjects;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DominoAPI.Services;

public class DominoService
{
    private readonly IMongoCollection<Domino> _dominoCollection;

    public DominoService(
        IOptions<MexicanTrainDominoDatabaseSettings> mexicanTrainDominoDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            mexicanTrainDominoDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            mexicanTrainDominoDatabaseSettings.Value.DatabaseName);

        _dominoCollection = mongoDatabase.GetCollection<Domino>(
            mexicanTrainDominoDatabaseSettings.Value.DominoCollectionName);
    }

    public async Task<List<Domino>> GetAsync() =>
        await _dominoCollection.Find(_ => true).ToListAsync();

    public async Task<Domino?> GetAsync(string id) =>
        await _dominoCollection.Find(x => x.id == id).FirstOrDefaultAsync();
}