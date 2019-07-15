using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Xyzies.TWC.Public.Api.Models
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class BranchModel
    {
        public Guid Id { get; set; }
        public string BranchName { get; set; }

        [MaxLength(250)]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Please enter valid phone no.")]
        public string Phone { get; set; }

        [Phone(ErrorMessage = "Please enter valid fax no.")]
        public string Fax { get; set; }

        [JsonProperty("address")]
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }

        public string City { get; set; }

        [RegularExpression(@"^[0-9]+$", ErrorMessage = "ZipCode must be numbers only.")]
        public string ZipCode { get; set; }

        public string GeoLat { get; set; }

        public string GeoLng { get; set; }
        
        public string State { set; get; }

        public bool IsEnabled { set; get; } = true;

        [DataType(DataType.DateTime)]
        public DateTime? CreatedDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? ModifiedDate { get; set; }

        public int? CreatedBy { get; set; }

        public int? ModifiedBy { get; set; }

        public int CompanyId { get; set; }

        public int? CountSalesRep { get; set; }

        public IList<int> UserIds { get; set; } = new List<int>();

        public virtual IList<BranchContactModel> BranchContacts { get; set; } = new List<BranchContactModel>();
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}