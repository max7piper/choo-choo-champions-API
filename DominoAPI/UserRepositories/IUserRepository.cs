using DominoAPI.UserObjects;

namespace DominoAPI.UserRepositories
{
    /// <summary>
    /// The interface for interacting with persisted user objects.
    /// </summary>
    public interface IUserRepository
    {
         /// <summary>
         /// Registers a user into the database
         /// </summary>
         /// <param name="user">The user object we are registering into the database.</param>
         /// <returns></returns>
         public Task<User> registerUser(User user);
    }
}