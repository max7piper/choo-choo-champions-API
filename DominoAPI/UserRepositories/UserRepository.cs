using System.Security.Cryptography;
using DominoAPI.Services;
using DominoAPI.UserObjects;
using System.Text;

namespace DominoAPI.UserRepositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserService _userService;

        public UserRepository(UserService userService) =>
            _userService = userService;

        public async Task<bool> login(string username, string password)
        {
            var users = await _userService.GetAsync();
            Console.WriteLine(password + ",");
            password = HashPassword(password);
            Console.WriteLine(password);
            return users.Where(u => u.Username!.Equals(username) && u.Password!.Equals(password)).Any();
        }

        public async Task<User> registerUser(User user)
        {
            user.Password = HashPassword(user.Password);
            var users = await _userService.GetAsync();
            bool available = !users.Where(u => u.Username!.Equals(user.Username) || u.Email.Equals(user.Email)).Any();
            if(!available){
                return null!;
            }
            await _userService.CreateAsync(user);
            return user;
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