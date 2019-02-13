using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace IdentityServiceClient.Middlewares
{
    public class ClaimsManagerMiddleware
    {
        private readonly RequestDelegate _next;

        public ClaimsManagerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var role = context.User.Claims.FirstOrDefault(claim => claim.Type == Const.Permissions.RoleClaimType)?.Value;

            if (role != null)
            {
                var identity = new ClaimsIdentity();
                identity.AddClaim(new Claim(ClaimTypes.Role, role));
                context.User.AddIdentity(identity);
            }

            await _next(context);
        }
    }
    public static class ClaimsManagerMiddlewareExtension
    {
        public static IApplicationBuilder UseClaimsManager(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseMiddleware<ClaimsManagerMiddleware>();
        }
    }
}
