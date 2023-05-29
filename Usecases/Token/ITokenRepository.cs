using System.Security.Claims;
namespace Project.UseCases.Tokens
{
    public interface ITokenRepository
    {
        string BuildToken(Claim[] claims);
        bool IsTokenValid(string token);
        ClaimsPrincipal GetClaimsPrincipalFromToken(string token);
        string GetTokenAliveTime();
    }
}