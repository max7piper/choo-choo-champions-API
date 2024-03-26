using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using DominoAPI.Services;
using DominoAPI.UserObjects;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using Azure.Communication.Email;
using Azure;

namespace DominoAPI.UserRepositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserService _userService;
        private readonly IConfiguration _config;

        public UserRepository(UserService userService, IConfiguration config)
        {
            _userService = userService;
            _config = config;
        }
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

        public async Task<User> UpdateProfileImage(String username, byte[] imageData)
        {
            User user = await _userService.GetByUsername(username);
            if(user!=null){
                user.ImageLink = imageData;
                await _userService.UpdateAsync(username, user);
            }
            return await _userService.GetByUsername(username);
        }

        public async Task<byte[]> GetProfileImage(string username)
        {
            User user = await _userService.GetByUsername(username);
            if (user != null && user.ImageLink != null)
            {
                return user.ImageLink;
            }
            return null!;
        }

        public Task<User> GetUser(string username)
        {
            return _userService.GetByUsername(username)!;
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

        public string GetJWT(string username)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
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

        public async Task<bool> SendEmailVerificationCode(string username, string emailAddress)
        {
            string connectionString = _config["AzureEmailConnectionString"]!;
            Random random = new Random();
            int randomCode = random.Next(100000,999999);
            string htmlText = "<html><h1>Use this code to verify your email: " + randomCode + "</h1l></html>";
            string plainText = "Use this code to verify your email: " + randomCode;
            try
            {
                var emailClient = new EmailClient(connectionString);
                EmailSendOperation emailSendOperation = emailClient.Send(
                    WaitUntil.Completed,
                    senderAddress: "DoNotReply@d1044c8c-c563-4296-ad20-acfd871f4d53.azurecomm.net",
                    recipientAddress: emailAddress,
                    subject: "Email Verification for ChooChooChampions!",
                    htmlContent: htmlText,
                    plainTextContent: plainText);
                User user = await _userService.GetByUsername(username);
                user.EmailConfirmation = randomCode;
                await _userService.UpdateAsync(username, user);
                return true;
            } catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return false;
            }
            
        }

        public async Task<User> VerifyEmailVerificationCode(string username, int verificationCode)
        {
            var user = await _userService.GetByUsername(username);
            // -1 is our placeholder value so we must ensure that will not be spoofed and used
            if(user != null && user.EmailConfirmation == verificationCode && verificationCode != -1)
            {
                user.EmailConfirmation = 1;
                await _userService.UpdateAsync(username, user);
                return user;
            }
            return null!;
        }
    }

}
