using graphApiService.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace graphApiService.Filters
{
    public class AccessFilter : Attribute, IActionFilter
    {
        private readonly List<string> _acceptedEnvironment;
        private readonly List<string> _acceptedRoles;

        public AccessFilter()
        {

        }

        public AccessFilter(string acceptedEnvironment, string acceptedRoles)
        {
            _acceptedEnvironment = acceptedEnvironment.Split(',').ToList();
            _acceptedRoles = acceptedRoles.Split(',').ToList();
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var httpContext = context.HttpContext;
            var environment = httpContext.Request.Headers["Environment"];
            var role = httpContext.User.Claims.FirstOrDefault(x => x.Type == Const.RoleClaimType).Value;
            if (!_acceptedRoles.Contains(role) && !_acceptedEnvironment.Contains(environment))
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
