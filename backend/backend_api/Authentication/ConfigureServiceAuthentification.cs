using backend.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace MyWebApi.Authentication
{
    public static class ConfigureAuthentificationServiceExtensions
    {
        /// <summary>
        /// Method to build RSA Privary key for JWT
        /// </summary>
        /// <param name="publicKeyJWT"></param>
        /// <returns></returns>
        private static RsaSecurityKey BuildRSAKey(string publicKeyJWT)
        {
            RSA rsa = RSA.Create();

            rsa.ImportSubjectPublicKeyInfo(

                source: Convert.FromBase64String(publicKeyJWT),
                bytesRead: out _
            );

            var IssuerSigningKey = new RsaSecurityKey(rsa);

            return IssuerSigningKey;
        }

        /// <summary>
        /// Method to configure JWT
        /// </summary>
        /// <param name="services"></param>
        /// <param name="IsDevelopment"></param>
        /// <param name="publicKeyJWT"></param>
        public static void ConfigureJWT(this IServiceCollection services, bool IsDevelopment, string publicKeyJWT, IConfiguration configuration)
        {
            string realm = string.Empty;
            string auth_server_url = string.Empty;
            string ssl_required = string.Empty;
            string resource = string.Empty;
            try
            {
                realm = configuration.GetSection("keycloak").GetSection("realm").Value;
                auth_server_url = configuration.GetSection("keycloak").GetSection("auth-server-url").Value;
                ssl_required = configuration.GetSection("keycloak").GetSection("ssl_required").Value;
                resource = configuration.GetSection("keycloak").GetSection("resource").Value;

                services.AddTransient<IClaimsTransformation, ClaimsTransformer>();

                // Add Authentication
                AuthenticationBuilder AuthenticationBuilder = services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                });

                // Add JWT Bearer token validation to Authentication
                AuthenticationBuilder.AddJwtBearer(o =>
                {
                    #region == JWT Token Validation ===
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidateIssuer = true,
                        ValidIssuers = new[] { $"{auth_server_url}realms/{realm}" },
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = BuildRSAKey(publicKeyJWT),
                        ValidateLifetime = true
                    };
                    #endregion
                    #region === Event Authentification Handlers ===
                    o.Events = new JwtBearerEvents()
                    {
                        OnTokenValidated = c =>
                        {
                            Console.WriteLine("User successfully authenticated");
                            return Task.CompletedTask;
                        },
                        OnAuthenticationFailed = c =>
                        {
                            c.NoResult();
                            c.Response.StatusCode = 500;
                            c.Response.ContentType = "text/plain";
                            if (IsDevelopment)
                            {
                                return c.Response.WriteAsync(c.Exception.ToString());
                            }
                            return c.Response.WriteAsync("An error occured processing your authentication.");
                        }
                    };
                    #endregion
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception while Configuring JWT :\nMessage: {ex.Message}\nStacktrace : {ex.StackTrace}");
            }
        }
    }
}