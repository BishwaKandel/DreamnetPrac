using System.Security.Claims;

namespace HRMSmvc.Extensions
{
    public static class ClaimsPrincipalExtension
    {
        public static string UserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                return null;
            }

            // The NameIdentifier claim (ClaimTypes.NameIdentifier) typically holds the user ID in ASP.NET Core Identity.
            return principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public static string Email(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                return null;
            }
            // The Email claim (ClaimTypes.Email) typically holds the user's email address.
            return principal.FindFirst(ClaimTypes.Email)?.Value;
        }
    }
}
