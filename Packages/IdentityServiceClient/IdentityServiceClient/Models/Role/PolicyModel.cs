using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServiceClient.Models.Role
{
    public class PolicyModel
    {
        public Guid PolicyId { get; set; }

        public string PolicyName { get; set; }

        public IList<ScopeModel> Scopes { get; set; } = new List<ScopeModel>();
    }
}
