namespace graphApiService.Common
{
    public class Const
    {
        public const string RoleClaimType = "extension_Group";
        public const string RolePropertyName = "extension_64dd8c06b51f4cb69670d2ffeacb6c8e_Group";

        public class GraphApi
        {
            public const string GraphApiEndpoint = "https://graph.windows.net/";
            public const string ApiVersionParameter = "api-version";
            public const string ApiVersion = "1.6";
            public const string UserEntity = "users";
        }
        public class Roles
        {
            public const string Saller = "Saller";
            public const string SuperAdmin = "SuperAdmin";
            public const string Admin = "Admin";
        }
    }
}
