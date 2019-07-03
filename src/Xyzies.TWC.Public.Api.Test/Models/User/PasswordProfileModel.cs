using System;
using System.Collections.Generic;
using System.Text;

namespace Xyzies.TWC.Public.Api.Tests.Models.User
{
    public class PasswordProfileModel
    {
        public string Password { get; set; }

        public bool? ForceChangePasswordNextLogin { get; set; }
    }
}
