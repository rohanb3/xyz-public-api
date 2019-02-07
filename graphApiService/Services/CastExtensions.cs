using System.Linq;
using graphApiService.Entities.User;

namespace graphApiService.Services
{
    public static class CastExtensions
    {
        public static ProfileDto ToProfileDto(this UserModel user)
        {
            return new ProfileDto()
            {
                AccountEnabled = user.AccountEnabled,
                City = user.City,
                CompanyName = user.CompanyName,
                DisplayName = user.DisplayName,
                ObjectId = user.ObjectId,
                Surname = user.Surname,
                Role = user.Role,
                GivenName = user.GivenName,
                UserName = user.UserName,
                Email = user.Email,
            };
        }

        public static ProfileDto ToProfileDto(this ProfileCreatableDto user)
        {
            return new ProfileDto()
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

        public static UserModel ToUserModel(this ProfileEditableDto user)
        {
            return new UserModel()
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
