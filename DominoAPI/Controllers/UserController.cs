using DominoAPI.UserObjects;
using DominoAPI.UserRepositories;
using DominoAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace DominoAPI.Controllers;

[ApiController]
[Route("user")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    public UserController(IUserRepository userRepository) =>
        _userRepository = userRepository;
    
    [HttpPost("register")]
    public Task<User> Register(String username, String password, String email)
    {
        User newUser = new User{
            Username = username,
            Password = password,
            Email = email
        };
        return _userRepository.registerUser(newUser);
    }
}