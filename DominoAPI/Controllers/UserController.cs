using DominoAPI.Services;
using DominoAPI.UserObjects;
using DominoAPI.UserRepositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DominoAPI.Controllers;

[ApiController]
[Route("user")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;

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
        return Ok(newUser);
    }

    [HttpPost("login")]
    public async Task<ActionResult<User>> Login(String username, String password)
    {
        User success = await _userRepository.login(username, password);
        if (success != null)
        {
            return Ok(success);
        }
        return BadRequest("Failed to login user. Incorrect username or password");
    }

    [HttpPost("UpdateProfileImage")]
    public async Task<ActionResult<User>> UpdateProfileImage(String username, String imageURL)
    {
        User result = await _userRepository.updateProfileImage(username, imageURL);
        if (result != null)
        {
            return Ok(result);
        }
        return BadRequest("Could not find or update that image.");
    }

    [HttpGet("ProfileImage/{username}")]
    public async Task<ActionResult<string>> GetProfileImage(string username)
    {
        string profileImageURL = await _userRepository.GetProfileImage(username);
        if (!string.IsNullOrEmpty(profileImageURL))
        {
            return Ok(profileImageURL);
        }
        return NotFound("Profile image not found for the given username.");
    }
}
