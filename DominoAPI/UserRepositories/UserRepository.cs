using System.Security.Cryptography;
using DominoAPI.Services;
using DominoAPI.UserObjects;
using MongoDB.Libmongocrypt;
using SharpCompress.Crypto;
using System;
using System.Text;

namespace DominoAPI.UserRepositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserService _userService;

        public UserRepository(UserService userService) =>
            _userService = userService;

        public async Task<User> registerUser(User user)
        {
            user.Password = HashPassword(user.Password);
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