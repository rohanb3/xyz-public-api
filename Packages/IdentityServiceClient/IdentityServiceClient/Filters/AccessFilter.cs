using IdentityServiceClient.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServiceClient.Filters
{
    class AccessFilter : Attribute, IActionFilter
    {
        public string Scopes { get; set; }
        public readonly IIdentityManager _identityManager;


        public AccessFilter(IIdentityManager identityManager)
        {
            _identityManager = identityManager ?? throw new ArgumentNullException(nameof(_identityManager));
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var role = context.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role)?.Value;
            var scopes = Scopes.Split(',');
            Task.Run(() => _identityManager.CheckPermissionExpiration()).Wait();
            var hasPermission = _identityManager.CheckPermission(role, scopes);
            if (!hasPermission)
            {
                context.Result = new ForbidResult();
                return;
            }
        }
    }
}
