using System;
using System.Linq;
using graphApiService.Dtos.User;
using graphApiService.Helpers.Users;
using Microsoft.Azure.ActiveDirectory.GraphClient;

namespace graphApiService.Services
{
    public static class CastExtensions
    {
        public static UserProfileDto ToUserProfileDto(this IUser user)
        {
            var userRole = (user as User).GetExtendedProperties().Where(extProp =>
                    extProp.Key.Equals("extension_64dd8c06b51f4cb69670d2ffeacb6c8e_Group", StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault().Value?.ToString();

            return new UserProfileDto()
            {
                AccountEnabled = user.AccountEnabled,
                City = user.City,
                CompanyName = user.CompanyName,
                DisplayName = user.DisplayName,
                ObjectId = user.ObjectId,
                Surname = user.Surname,
                Role = userRole,
                GivenName = user.GivenName,
                UserName = user.SignInNames.FirstOrDefault(name => name.Type == "userName")?.Value,
                Email = user.SignInNames.FirstOrDefault(name => name.Type == "emailAddress")?.Value,
            };
        }

        public static IUser ToAdUser(this UserProfileCreatableDto user)
        {
            return new User()
            {
                AccountEnabled = user.AccountEnabled,
                DisplayName = user.DisplayName,
                CreationType = user.CreationType,
                MailNickname = user.MailNickname,
                PasswordProfile = user.PasswordProfile,
                UsageLocation = user.UsageLocation,
                SignInNames = user.SignInNames,
                Surname = user.Surname
            };
        }

        public static UserProfileDto ToUserProfileDto(this UserProfileCreatableDto user)
        {
            return new UserProfileDto()
            {
                AccountEnabled = user.AccountEnabled,
                AvatarUrl = user.AvatarUrl,
                DisplayName = user.DisplayName,
                City = user.City,
                CompanyId = user.CompanyId,
                GivenName = user.GivenName,
                Phone = user.Phone,
                RetailerId = user.RetailerId,
                UserName = user.SignInNames.FirstOrDefault(signInName => signInName.Type == "userName")?.Value,
                Email = user.SignInNames.FirstOrDefault(signInName => signInName.Type == "emailAddress")?.Value,
            };
        }
    }
}
