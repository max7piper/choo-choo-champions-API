using DominoAPI.UserObjects;
using DominoAPI.UserRepositories;
using DominoAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DominoAPI.Controllers;

[ApiController]
[Route("user")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    public UserController(IUserRepository userRepository) =>
        _userRepository = userRepository;
    
    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(String username, String password, String email)
    {
        User newUser = new User{
            Username = username,
            Password = password,
            Email = email
        };
        newUser = await _userRepository.registerUser(newUser);
        if(newUser == null){
            return BadRequest("Failed to register user");        
        }
        return Ok(newUser);
    }

    [HttpPost("login")]
    public async Task<ActionResult<bool>> Login(String username, String password)
    {
        bool success = await _userRepository.login(username,password);
        if(success){
            return Ok(success);
        }
        return BadRequest("Failed to login user. Incorrect username or password");
    }
}