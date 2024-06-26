using System.Reflection.Metadata;
using DominoAPI.Services;
using DominoAPI.UserObjects;
using DominoAPI.UserRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DominoAPI.Controllers;

[ApiController]
[Route("user")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly int EmailConfirmationConstant = 1;

    public UserController(IUserRepository userRepository) => _userRepository = userRepository;

    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(String username, String password, String email)
    {
        User newUser = new User
        {
            Username = username,
            Password = password,
            Email = email
        };
        newUser = await _userRepository.registerUser(newUser);
        if (newUser == null)
        {
            return BadRequest("Failed to register user");
        }
        newUser.Id = _userRepository.GetJWT(username);
        return Ok(newUser);
    }

    [HttpPost("login")]
    public async Task<ActionResult<User>> Login(String username, String password)
    {
        User success = await _userRepository.login(username, password);
        if (success != null)
        {
            success.Id = _userRepository.GetJWT(username);
            if (success.EmailConfirmation != EmailConfirmationConstant)
            {
                return BadRequest("Failed to login user. Please verify your email first.");
            }
            return Ok(success);
        }
        return BadRequest("Failed to login user. Incorrect username or password");
    }

    [Authorize]
    [HttpGet("Profile/{username}")]
    public async Task<ActionResult<User>> GetProfile(String username)
    {
        User user = await _userRepository.GetUser(username);
        if (user == null)
        {
            return NotFound("No user exists for that username.");
        }
        return Ok(user);
    }

    [HttpPost("changePassword")]
    public async Task<ActionResult> ChangePassword(string username, string oldPassword, string newPassword)
    {
        var result = await _userRepository.ChangePassword(username, oldPassword, newPassword);
        if (result)
        {
            return Ok("Password changed successfully.");
        }
        return BadRequest("Failed to change password. Incorrect username or old password.");
    }

    [HttpPost("sendEmailVerification")]
    public async Task<ActionResult> SendEmailVerification(string username, string email)
    {
        var result = await _userRepository.SendEmailVerificationCode(username, email);
        if (result)
        {
            return Ok("Email Verification has been sent.");
        }
        return BadRequest("Failed to send email, invalid email.");
    }

    [HttpPost("verifyEmailCode")]
    public async Task<ActionResult> VerifyEmailVerificationCode(string username, int code)
    {
        var result = await _userRepository.VerifyEmailVerificationCode(username, code);
        if (result != null)
        {
            result.Id = _userRepository.GetJWT(username);
            return Ok("Email is verified!");
        }
        return BadRequest("Entered code does not match. Click to resend code, or reenter code.");
    }

    [HttpPost("updateUsername")]
    public async Task<ActionResult<User>> UpdateUsername(string username, string newUsername)
    {
        var result = await _userRepository.UpdateUsername(username, newUsername);
        if (result != null)
        {
            return Ok(result);
        }
        return BadRequest("Username is invalid or already taken.");
    }

    [HttpPost("updateEmail")]
    public async Task<ActionResult<User>> UpdateEmail(string username, string newEmail)
    {
        var result = await _userRepository.UpdateEmail(username, newEmail);
        if (result != null)
        {
            return Ok(result);
        }
        return BadRequest("Email is invalid or already taken.");
    }

    [Authorize]
    [HttpPost("updateStats")]
    public async Task<ActionResult<User>> UpdateStats(string username, int score, bool wonRound, bool wonGame, bool endGame)
    {
        var result = await _userRepository.UpdateUserStats(username, score, wonRound, wonGame, endGame);
        if (result != null)
        {
            return Ok(result);
        }
        return BadRequest("Email is invalid or already taken.");
    }
}

