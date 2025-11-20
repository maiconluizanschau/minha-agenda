using System.Security.Claims;

namespace Agenda.Api.Application.Security
{
    public interface IJwtTokenService
    {
        string GenerateToken(string username, IEnumerable<Claim> additionalClaims);
    }
}
