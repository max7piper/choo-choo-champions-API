using System.Security.Cryptography;
using System.Text;
using DominoAPI.Services;
using DominoAPI.UserObjects;

namespace DominoAPI.UserRepositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserService _userService;

        public UserRepository(UserService userService) => _userService = userService;

        public async Task<User> login(string username, string password)
        {
            var users = await _userService.GetAsync();
            password = HashPassword(password);
            return users
                .Where(u => u.Username!.Equals(username) && u.Password!.Equals(password))
                .FirstOrDefault()!;
        }

        public async Task<User> registerUser(User user)
        {
            user.Password = HashPassword(user.Password);
            var users = await _userService.GetAsync();
            bool available = !users
                .Where(u => u.Username!.Equals(user.Username) || u.Email.Equals(user.Email))
                .Any();
            if (!available)
            {
                return null!;
            }
            await _userService.CreateAsync(user);
            return user;
        }

        public async Task<User> updateProfileImage(string username, string imageURL)
        {
            // get the user and update them
            User user = await _userService.GetByUsername(username);
            if (user != null)
            {
                user.ImageLink = imageURL;
                await _userService.UpdateAsync(username, user);
            }

            // return the updated user
            return await _userService.GetByUsername(username);
        }

        public async Task<string> GetProfileImage(string username)
        {
            User user = await _userService.GetByUsername(username);
            if (user != null && !string.IsNullOrEmpty(user.ImageLink))
            {
                return user.ImageLink;
            }
            return null!;
        }

        public Task<User> GetUser(string username)
        {
            return _userService.GetByUsername(username)!;
        }

        public async Task<bool> ChangePassword(string username, string oldPassword, string newPassword)
        {
            var user = await login(username, oldPassword);
            if (user != null)
            {
                user.Password = HashPassword(newPassword);
                await _userService.UpdateAsync(username, user);
                return true;
            }
            return false;
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Convert the input string to a byte array and compute the hash
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert the byte array to a hexadecimal string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
