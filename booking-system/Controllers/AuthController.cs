using BookingSystem.Models;
using BookingSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserService _users;
        private readonly AuthService _auth;
        private readonly FirebaseAuthService _firebase;
        public AuthController(UserService users, AuthService auth, FirebaseAuthService firebase)
        {
            _users = users;
            _auth = auth;
            _firebase = firebase;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
        {
            var user = await _users.AuthenticateAsync(request.Email, request.Password);
            if (user == null) return Unauthorized();
            var token = _auth.GenerateToken(user.Id, user.Email);
            return Ok(new LoginResponse(token));
        }

        [HttpPost("firebase")]
        public async Task<ActionResult<LoginResponse>> FirebaseLogin(FirebaseLoginRequest request)
        {
            var decoded = await _firebase.VerifyIdTokenAsync(request.IdToken);
            if (decoded == null) return Unauthorized();

            var email = decoded.Claims.TryGetValue("email", out var e) ? e?.ToString() : null;
            if (email == null) return Unauthorized();

            var user = await _users.GetByEmailAsync(email) ?? await _users.RegisterAsync(email, string.Empty);
            var token = _auth.GenerateToken(user.Id, email);
            return Ok(new LoginResponse(token));
        }
    }
}