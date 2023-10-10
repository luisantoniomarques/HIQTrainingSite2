using System.Security.Claims;
using System.Security.Principal;

namespace HIQTrainingSite.ModelsExtensions
{
    public static class IdentityExtensions
    {
        public static string GetColor(this IIdentity identity)
        {
            var color = ((ClaimsIdentity)identity).FindFirst("Color");
            // Test for null to avoid issues during local testing
            return (color != null) ? color.Value.Trim() : string.Empty;
        }
    }
}