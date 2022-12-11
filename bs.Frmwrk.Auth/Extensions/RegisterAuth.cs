using bs.Frmwrk.Core.Globals.Config;
using bs.Frmwrk.Core.Models.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace bs.Frmwrk.Auth.Extensions
{
    public static class AuthExtensions
    {
        /// <summary>
        /// Set the JWT autentication from JWT token in Header (and from url query parameter 'access_token' for Signal R).
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="securitySettings">The security settings.</param>
        public static void SetAutentication(IServiceCollection services, ISecuritySettings securitySettings)
        {
            // Setting authentication using JWT Token in request header
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = securitySettings.ValidateTokenIssuer ?? false,
                        ValidateAudience = securitySettings.ValidateTokenAudience ?? false,
                        ValidAudience = securitySettings.ValidTokenAudience,
                        ValidIssuer = securitySettings.ValidTokenIssuer,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securitySettings.Secret))
                    };
                    //Added to autenticate Signal R (token is in the query part of the url)
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var path = context.HttpContext.Request.Path;
                            if (path.StartsWithSegments(UrlsConfigurations.SignalRHubPrefix))
                            {
                                // If the request is addressed to any hubs get the token from the access_token query parameter
                                var accessToken = context.Request.Query["access_token"];
                                if (!string.IsNullOrEmpty(accessToken))
                                {
                                    // Read the token out of the query string
                                    context.Token = accessToken;
                                }
                            }
                            return Task.CompletedTask;
                        }
                    };
                });
        }
    }
}