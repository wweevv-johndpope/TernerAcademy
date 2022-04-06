using System.Security.Claims;

namespace Client.App.Extensions
{
    internal static class ClaimsIdentityExtensions
    {
        internal static string GetEmail(this ClaimsIdentity claimsIdentity)
            => claimsIdentity.FindFirst(x => x.Type == nameof(ClaimTypes.Email))?.Value;

        internal static string GetFirstName(this ClaimsIdentity claimsIdentity)
            => claimsIdentity.FindFirst(x => x.Type == nameof(ClaimTypes.GivenName))?.Value;

        internal static string GetLastName(this ClaimsIdentity claimsIdentity)
            => claimsIdentity.FindFirst(x => x.Type == nameof(ClaimTypes.Surname))?.Value;

        internal static string GetUserId(this ClaimsIdentity claimsIdentity)
            => claimsIdentity.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        internal static string GetUsername(this ClaimsIdentity claimsIdentity)
            => claimsIdentity.FindFirst(x => x.Type == ClaimTypes.Name)?.Value;


    }
}
