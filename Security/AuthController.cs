using Microsoft.AspNetCore.Mvc;

namespace TFTDataTrackerApi.Security
{
    [ApiController]
    [Route("auth")]
    public class AuthController(IConfiguration config, TokenService tokenService) : ControllerBase
    {
        private readonly IConfiguration _config = config;
        private readonly TokenService _tokenService = tokenService;

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
                return Ok(new { token });
            }

            return Unauthorized("Invalid credentials");
        }
    }
}

