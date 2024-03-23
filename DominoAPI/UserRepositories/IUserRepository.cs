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
        /// <returns>The User</returns>
        public Task<User> updateProfileImage(String username, String imageURL);

        /// <summary>
        /// Retrieves the profile image URL associated with the given username.
        /// </summary>
        /// <param name="username">The username of the user whose profile image URL is to be retrieved.</param>
        /// <returns>The profile image URL, or null if not found.</returns>
        public Task<string> GetProfileImage(string username);

        /// <summary>
        /// Retrieves a user's profile. 
        /// </summary>
        /// <param name="username"></param>
        /// <returns>The user associated with that profile, or none.</returns>
        public Task<User> GetUser(String username);

        /// <summary>
        /// Generates a JWT for the user, which will be used for verification purposes.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>The JWT.</returns>
        public String GetJWT(String username);

        /// <summary>
        /// Changes the password for a user.
        /// </summary>
        /// <param name="username">The username of the user whose password is being changed.</param>
        /// <param name="oldPassword">The old password for verification.</param>
        /// <param name="newPassword">The new password to set.</param>
        /// <returns>True if the password is changed successfully, otherwise false.</returns>
        public Task<bool> ChangePassword(string username, string oldPassword, string newPassword);

    }
}
