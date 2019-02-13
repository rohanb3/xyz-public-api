using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServiceClient
{
    public class Const
    {
        public class Permissions
        {
            public const string RoleClaimType = "extension_Group";
        }
        public class GraphApi
        {
            public const string GraphApiEndpoint = "https://graph.windows.net/";
            public const string ApiVersionParameter = "api-version";
            public const string ApiVersion = "1.6";
            public const string UserEntity = "users";
        }
        public class Cache
        {
            public const string PermissionKey = "Permission";
            public const string ExpirationKey = "Expiration";
            public const string PermissionHash = "Hash";
        }
    }
}
