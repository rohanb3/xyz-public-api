using System.Linq;
using graphApiService.Entities.User;

namespace graphApiService.Common
{
    public static class Extensions
    {
        public static Profile ToProfileDto(this AzureUser azureUser)
        {
            return new Profile()
            {
                AccountEnabled = azureUser.AccountEnabled,
                City = azureUser.City,
                CompanyName = azureUser.CompanyName,
                DisplayName = azureUser.DisplayName,
                ObjectId = azureUser.ObjectId,
                Surname = azureUser.Surname,
                Role = azureUser.Role,
                GivenName = azureUser.GivenName,
                UserName = azureUser.UserName,
                Email = azureUser.Email,
            };
        }

        public static Profile ToProfileDto(this ProfileCreatable user)
        {
            return new Profile()
            {
                AccountEnabled = user.AccountEnabled,
                //AvatarUrl = user.AvatarUrl,
                DisplayName = user.DisplayName,
                City = user.City,
                //CompanyId = user.CompanyId,
                GivenName = user.GivenName,
                //Phone = user.Phone,
                //RetailerId = user.RetailerId,
                UserName = user.SignInNames.FirstOrDefault(signInName => signInName.Type == "userName")?.Value,
                Email = user.SignInNames.FirstOrDefault(signInName => signInName.Type == "emailAddress")?.Value,
            };
        }

        public static AzureUser ToUserModel(this ProfileEditable user)
        {
            return new AzureUser()
            {
                AccountEnabled = user.AccountEnabled,
                AvatarUrl = user.AvatarUrl,
                DisplayName = user.DisplayName,
                GivenName = user.GivenName,
                Surname = user.Surname,
                City = user.City,
                Role = user.Role,
            };
        }
    }
}
