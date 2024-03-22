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

        /// <summary>
        /// Logs a user in to the game
        /// </summary>
        /// <param name="username">The users username</param>
        /// <param name="password">The users password, unhashed</param>
        /// <returns></returns>
        public Task<User> login(String username, String password);

        /// <summary>
        /// Updates a user's profile image.
        /// </summary>
        /// <param name="username">The user that needs to be updated.</param>
        /// <returns></returns>
        public Task<User> updateProfileImage(String username, String imageURL);

        /// <summary>
        /// Retrieves the profile image URL associated with the given username.
        /// </summary>
        /// <param name="username">The username of the user whose profile image URL is to be retrieved.</param>
        /// <returns>The profile image URL, or null if not found.</returns>
        Task<string> GetProfileImage(string username);
    }
}
