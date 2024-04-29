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

        /// <summary>
        /// Send the user an email to their provided email address. Returns false if email was invalid.
        /// </summary>
        /// <param name="emailAddress">The provided email address.</param>
        /// <returns>True if the email was sent successfully. False if not.</returns>
        public Task<bool> SendEmailVerificationCode(String username, String emailAddress);

        /// <summary>
        /// Verifies that the user registered with their sent verification code. 
        /// </summary>
        /// <param name="username">The user's username.</param>
        /// <param name="verificationCode">The user's verification code.</param>
        /// <returns></returns>
        public Task<User> VerifyEmailVerificationCode(String username, int verificationCode);

        /// <summary>
        /// Updates the username of a user.
        /// </summary>
        /// <param name="username">The current username of the user.</param>
        /// <param name="newUsername">The new username to be set.</param>
        /// <returns>The updated user object, or null if the operation fails.</returns>
        public Task<User?> UpdateUsername(string username, string newUsername);

        /// <summary>
        /// Updates the email address of a user.
        /// </summary>
        /// <param name="username">The username of the user whose email is being updated.</param>
        /// <param name="newEmail">The new email address to be set.</param>
        /// <returns>The updated user object, or null if the operation fails.</returns>
        public Task<User?> UpdateEmail(string username, string newEmail);

        /// <summary>
        /// Updates the users game statistics.
        /// </summary>
        /// <param name="username">The username of the user whose stats are being updated.</param>
        /// <param name="score">The score of the game.</param>
        /// <param name="wonRound">Whether the user won this round or not.</param>
        /// <param name="wonGame">Whether the user won this game or not.</param>
        /// <param name="endGame">Whether the game ended or not.</param>
        /// <returns></returns>
        public Task<User?> UpdateUserStats(string username, int score, bool wonRound, bool wonGame, bool endGame);
    }
}
