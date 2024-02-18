using System.Text;
using DataHub.Server.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace DataHub.Api.Configuration;

/// <summary>
/// Provides methods to configure authentication in the application.
/// </summary>
public static class AuthenticationConfig
{
    /// <summary>
    /// Configures authentication using the provided <see cref="IConfiguration"/>.
    /// </summary>
    /// <param name="services">The collection of services to add authentication to.</param>
    /// <param name="configuration">The configuration from which to retrieve authentication settings.</param>
    /// <returns>The modified collection of services.</returns>
    public static IServiceCollection AddAuthenticationConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection(AuthOptions.Name).GetValue<string>(nameof(AuthOptions.SecretKey))))
                };
            });

        return services;
    }
}