using System.Collections.Generic;
using System.Linq;


// TODO: Add tests
// TODO: Add comments

namespace Xyzies.SSO.Identity.Services.Service
{
    public class SelectConditions
    {
        public static string GetSelectQueryString(List<string> paramNames)
        {
            return paramNames != null && paramNames.Count != 0 
                ? $"$select={string.Join(',', paramNames)}" 
                : "";
        }
    }
}
