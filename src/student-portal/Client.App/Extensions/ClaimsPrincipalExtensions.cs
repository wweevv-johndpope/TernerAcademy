using System.Linq;
using System.Security.Claims;

namespace Client.App.Extensions
{
    internal static class ClaimsPrincipalExtensions
    {
        internal static string GetEmail(this ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

        internal static string GetFirstName(this ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value;

        internal static string GetLastName(this ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Surname)?.Value;

        internal static string GetUserId(this ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        internal static string GetUsername(this ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;


    }
}
