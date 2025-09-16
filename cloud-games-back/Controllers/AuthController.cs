using cloud_games_back.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace cloud_games_back.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
     

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            var token = await _authService.LoginAsync(login.Email, login.Senha);
            if (token == null) return Unauthorized("Credenciais inválidas");
            return Ok(new { token });
        }

        public async Task<IActionResult> RefreshToken()
        {
          
            return Ok("");
        }

        public class LoginDto
        {
            public string Email { get; set; } = string.Empty;
            public string Senha { get; set; } = string.Empty;
        }
    }
}
