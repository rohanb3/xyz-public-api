using System.Security.Claims;
using System.Threading.Tasks;
using graphApiService.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace graphApiService.Middlewares
{
    public class ProcessClaimsMiddleware
    {
        private readonly RequestDelegate _next;

        public ProcessClaimsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        { 
            foreach (var userClaim in context.User.Claims)
            {
                if (userClaim.Type.Contains(Const.RoleClaimType))
                {
                    var identity = new ClaimsIdentity();
                    identity.AddClaim(new Claim(ClaimTypes.Role, userClaim.Value));
                    context.User.AddIdentity(identity);
                    break;
                }
            }

            await _next(context);
        }
    }
    public static class ProcessClaimsMiddlewareExtension
    {
        public static IApplicationBuilder UseProcessClaims(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseMiddleware<ProcessClaimsMiddleware>();
        }
    }
}
