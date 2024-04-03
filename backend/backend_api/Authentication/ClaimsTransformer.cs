using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Security.Claims;

namespace backend.Authentication
{
    /// <summary>
    /// Used to get the role within the claims structure used by keycloak, then it adds the role(s) in the ClaimsItentity of ClaimsPrincipal.Identity
    /// </summary>
    public class ClaimsTransformer : IClaimsTransformation
    {
        string realm = string.Empty;
        string auth_server_url = string.Empty;
        string ssl_required = string.Empty;
        string resource = string.Empty;

        public ClaimsTransformer(IConfiguration configuration)
        {
            realm = configuration.GetSection("keycloak").GetSection("realm").Value;
            auth_server_url = configuration.GetSection("keycloak").GetSection("auth-server-url").Value;
            ssl_required = configuration.GetSection("keycloak").GetSection("ssl_required").Value;
            resource = configuration.GetSection("keycloak").GetSection("resource").Value;
        }

        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)principal.Identity;

            // flatten resource_access because Microsoft identity model doesn't support nested claims
            // by map it to Microsoft identity model, because automatic JWT bearer token mapping already processed here
            if (claimsIdentity.IsAuthenticated && claimsIdentity.HasClaim((claim) => claim.Type == "resource_access"))
            {
                Claim resourceAccessClaim = claimsIdentity.FindFirst((claim) => claim.Type == "resource_access");
                Dictionary<string, JObject> resourceAccess = JsonConvert.DeserializeObject<Dictionary<string, JObject>>(resourceAccessClaim.Value);

                // Check if the resource access dictionary contains the desired resource
                if (resourceAccess.ContainsKey(resource))
                {
                    var roles = resourceAccess[resource]["roles"].ToObject<List<string>>();

                    // Add each role to the claims identity
                    foreach (var role in roles)
                    {
                        claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
                    }
                }

                //    var content = Newtonsoft.Json.Linq.JObject.Parse(userRole.Value);

                //foreach (var role in content[resource]["roles"])
                //{
                //    claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role.ToString()));
                //}
            }

            return Task.FromResult(principal);
        }
    }
}
