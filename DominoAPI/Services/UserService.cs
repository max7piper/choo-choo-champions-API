using DominoAPI.UserObjects;
using DominoAPI.UserRepositories;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DominoAPI.Services;

public class UserService
{
    private readonly IMongoCollection<User> _userCollection;

    public UserService(IOptions<UserDatabaseSettings> userDatabaseSettings)
    {
        var mongoClient = new MongoClient(userDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(userDatabaseSettings.Value.DatabaseName);

        _userCollection = mongoDatabase.GetCollection<User>(
            userDatabaseSettings.Value.UserCollectionName
        );
    }

    public async Task<List<User>> GetAsync() => await _userCollection.Find(_ => true).ToListAsync();

    public async Task<User> GetByUsername(string username) =>
        await _userCollection.Find(u => u.Username.Equals(username)).FirstOrDefaultAsync();

    public async Task<User> GetByEmail(string email) =>
        await _userCollection.Find(u => u.Email == email).FirstOrDefaultAsync();

    public async Task CreateAsync(User newUser) => await _userCollection.InsertOneAsync(newUser);

    public async Task UpdateAsync(string username, User updatedUser) =>
        await _userCollection.ReplaceOneAsync(x => x.Username == username, updatedUser);
}
