namespace Xyzies.TWC.Public.Api.Models.Options
{
    /// <summary>
    /// Options model for relation service
    /// </summary>
    public class RelationOptions
    {
        /// <summary>
        /// Identity MS url
        /// </summary>
        public string IdentityServiceUrl { get; set; }
        /// <summary>
        /// Identity MS static token
        /// </summary>
        public string IdentityStaticToken { get; set; }
        /// <summary>
        /// VSP Video MS url
        /// </summary>
        public string VspVideoServiceUrl { get; set; }
    }
}
