using System.Security.Claims;
using System.Threading.Tasks;
using graphApiService.Helpers.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace graphApiService.Middlewares
{
    public class ProcessClaimsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly UserRolesOptions _userRolesOptions;

        public ProcessClaimsMiddleware(RequestDelegate next, IOptionsMonitor<UserRolesOptions> userRolesOptionsMonitor)
        {
            _next = next;
            _userRolesOptions = userRolesOptionsMonitor.CurrentValue;
        }

        public async Task Invoke(HttpContext context)
        { 
            foreach (var userClaim in context.User.Claims)
            {
                if (userClaim.Type.Contains(_userRolesOptions.ClaimType))
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
