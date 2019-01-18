using Newtonsoft.Json;

namespace graphApiService.Dtos.User
{
    public class UserProfileEditableDto
    {
        public  bool AccountEnabled { get; set; }
        public string DisplayName { get; set; }
        public string City { get; set; }
        public string CompanyName { get; set; }
    }
}
