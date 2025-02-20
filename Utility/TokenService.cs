using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Linq;
using System.Security.Claims;

namespace Utility;
public class TokenService
{
    private readonly string _secretKey = "hey1234567890ojykjrkr6uluyk"; // Should match the one used for signing

    // Method to decrypt and validate the JWT token
    public ClaimsPrincipal DecryptToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_secretKey);

        try
        {
            // Validate and decode the JWT token
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };

            // Validate the token and return the claims
            var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
            return principal; // Contains claims (user info, roles, etc.)
        }
        catch (Exception ex)
        {
            // Token validation failed (invalid token, expired, etc.)
            throw new UnauthorizedAccessException("Invalid token", ex);
        }
    }
}
