using graphApiService.Dtos.User;
using Microsoft.Azure.ActiveDirectory.GraphClient;

namespace graphApiService.Services
{
    public static class CastExtensions
    {
        public static UserProfileDto ToUserProfileDto(this IUser user)
        {
            return new UserProfileDto()
            {
                AccountEnabled = user.AccountEnabled,
                City = user.City,
                CompanyName = user.CompanyName,
                DisplayName = user.DisplayName,
                ObjectId = user.ObjectId
            };
        }

        public static IUser ToAdUser(this UserProfileDto user)
        {
            return new User()
            {
                AccountEnabled = user.AccountEnabled,
                City = user.City,
                CompanyName = user.CompanyName,
                DisplayName = user.DisplayName,
                ObjectId = user.ObjectId,
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
                SignInNames = user.SignInNames
            };
        }

        public static UserProfileDto ToUserProfileDto(this UserProfileCreatableDto user)
        {
            return new UserProfileDto()
            {
                AccountEnabled = user.AccountEnabled,
                DisplayName = user.DisplayName
            };
        }
    }
}
