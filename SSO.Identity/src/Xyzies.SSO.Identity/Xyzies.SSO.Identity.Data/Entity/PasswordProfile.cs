using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Xyzies.SSO.Identity.Data.Entity
{
    public class PasswordProfile
    {
        public string Password { get; set; }

        public bool? ForceChangePasswordNextLogin { get; set; }

        public bool? EnforceChangePasswordPolicy { get; set; }
    }
}
