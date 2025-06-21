using BookingSystem.Models;
using BookingSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _users;
        public UserController(UserService users) => _users = users;

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterRequest request)
        {
            var user = await _users.RegisterAsync(request.Email, request.Password);
            return Ok(new UserDto(user.Id, user.Email, user.EmailVerified));
        }
    }
}