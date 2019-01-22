namespace graphApiService.Dtos.User {
    public class UserProfileDto {
        public string ObjectId { get; set; }
        public string DisplayName { get; set; }
        public string City { get; set; }
        public string CompanyName { get; set; }
        public bool AccountEnabled { get; set; }
    }
}