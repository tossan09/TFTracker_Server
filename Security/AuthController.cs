using Microsoft.AspNetCore.Mvc;

namespace TFTDataTrackerApi.Security
{
    [ApiController]
    [Route("auth")]
    public class AuthController(IConfiguration config, TokenService tokenService, ILogger<AuthController> logger) : ControllerBase
    {
        private readonly IConfiguration _config = config;
        private readonly TokenService _tokenService = tokenService;
        private readonly ILogger<AuthController> _logger = logger;

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel login)
        {
            if (login.Username == _config["Auth:Username"] && login.Password == _config["Auth:Password"])
            {
                var user = new User
                {
                    AdmUser = login.Username,
                    Roles = _config.GetSection("Auth:Roles").Get<List<string>>()!
                };

                var token = _tokenService.GerarToken(user);
                _logger.LogInformation("User {User} logged in successfully with roles {Roles}", user.AdmUser, string.Join(",", user.Roles));
                return Ok(new { token });
            }

            _logger.LogWarning("Failed login attempt with username {Username}", login.Username);
            return Unauthorized("Invalid credentials");
        }
    }
}

