using System.ComponentModel.DataAnnotations;

namespace Xyzies.TWC.Public.Api.Models
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class UploadCompanyModel
    {
        public string CompanyName { get; set; }
        public string LegalName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Please enter valid phone no.")]
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public int StoreID { get; set; }
        public int Agentid { get; set; }
        public int Status { get; set; }
        public string PrimaryContactName { get; set; }
        public string PrimaryContactTitle { get; set; }

        [Phone(ErrorMessage = "Please enter valid fax no.")]
        public string Fax { get; set; }
        public string FirstName { get; set; }
        public string GeoLat { get; set; }
        public string GeoLog { get; set; }
        public bool IsEnabled { get; set; }

    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
