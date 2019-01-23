namespace graphApiService.Helpers.Azure
{
    public class AzureAdGraphApiOptions
    {
        public string AppId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Resource { get; set; }
        public string GrantType { get; set; }
        public string RequestUri { get; set; }
    }
}
