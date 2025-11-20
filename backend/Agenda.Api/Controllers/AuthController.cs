using System.Security.Claims;
using Agenda.Api.Application.Dtos;
using Agenda.Api.Application.Security;
using Microsoft.AspNetCore.Mvc;

namespace Agenda.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtTokenService _jwtTokenService;

        // Usuários de exemplo em memória
        // Em um cenário real, isso viria de um banco de dados.
        private static readonly Dictionary<string, (string Password, string Role)> Users =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["admin"] = ("P@ssw0rd", "Admin"),
                ["user"] = ("P@ssw0rd", "User")
            };

        public AuthController(IJwtTokenService jwtTokenService)
        {
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("login")]
        public ActionResult<LoginResponseDto> Login([FromBody] LoginRequestDto request)
        {
            if (!Users.TryGetValue(request.Username, out var userInfo) ||
                userInfo.Password != request.Password)
            {
                return Unauthorized(new { message = "Usuário ou senha inválidos." });
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, userInfo.Role)
            };

            var token = _jwtTokenService.GenerateToken(request.Username, claims);

            return Ok(new LoginResponseDto
            {
                Token = token,
                Username = request.Username
            });
        }
    }
}
