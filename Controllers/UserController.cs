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


        [HttpGet("{userId}")]
        public async Task<ActionResult<UserDto>> GetProfile(int userId)
        {
            var user = await _users.GetByIdAsync(userId);
            if (user == null) return NotFound();
            return Ok(new UserDto(user.Id, user.Email, user.EmailVerified));
        }

        [HttpPost("change-password")]
        public async Task<ActionResult> ChangePassword(ChangePasswordRequest request)
        {
            var ok = await _users.ChangePasswordAsync(request.UserId, request.CurrentPassword, request.NewPassword);
            if (!ok) return BadRequest();
            return Ok();
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPassword(ResetPasswordRequest request)
        {
            var ok = await _users.ResetPasswordAsync(request.Email, request.NewPassword);
            if (!ok) return NotFound();
            return Ok();
        }
    }
}