namespace Xyzies.SSO.Identity.Data.Helpers
{
    public class Consts
    {
        private const string _extensionPropertyTemplate = "extension_dc2c5f0a6f0845c385858b049a55e71e_";
        public const string RoleClaimType = "extension_Group";

        public const string RolePropertyName = _extensionPropertyTemplate + "Group";
        public const string RetailerIdPropertyName = _extensionPropertyTemplate + "RetailerId";
        public const string CompanyIdPropertyName = _extensionPropertyTemplate + "CompanyId";
        public const string ManagerIdPropertyName = _extensionPropertyTemplate + "ManagerId";

        public class GraphApi
        {
            public const string GraphApiEndpoint = "https://graph.windows.net/";
            public const string ApiVersionParameter = "api-version";
            public const string ApiVersion = "1.6";
            public const string UserEntity = "users";
        }
        public class Roles
        {
            public const string SalesRep = "SalesRep";
            public const string SuperAdmin = "SuperAdmin";
            public const string Admin = "Admin";
        }

    }
}
