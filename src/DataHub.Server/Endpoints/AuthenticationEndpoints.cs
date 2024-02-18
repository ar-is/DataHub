using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DataHub.Server.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DataHub.Server.Endpoints;

internal static class AuthenticationEndpoints
{
    public static Results<Ok<AuthenticationResult>, UnauthorizedHttpResult> Authenticate(
        [FromQuery] string username, 
        [FromQuery] string password, 
        [FromServices] IOptions<AuthOptions> authOptions)
    {
        var auth = authOptions?.Value;
        if (username != auth.Username || password != auth.Password)
        {
            return TypedResults.Unauthorized();
        }

        var tokenString = GenerateJwtToken(username, auth);
        return TypedResults.Ok(new AuthenticationResult(tokenString));
    }

    private static string GenerateJwtToken(string username, AuthOptions authOptions)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authOptions.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(authOptions.ExpirationMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}