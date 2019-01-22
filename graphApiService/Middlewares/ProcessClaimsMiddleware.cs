using System.Security.Claims;
using System.Threading.Tasks;
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
            ClaimsIdentity identity = new ClaimsIdentity();
            foreach (Claim userClaim in context.User.Claims)
            {
                if (userClaim.Type.Contains("extension_Group"))
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role,userClaim.Value));
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
