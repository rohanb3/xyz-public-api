﻿using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Xyzies.TWC.Public.Data.Entities;

namespace Xyzies.TWC.Public.Api.Models
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class CreateBranchModel
    {
        [Required]
        public string BranchName { get; set; }

        [MaxLength(250)]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Please enter valid phone no.")]
        public string Phone { get; set; }

        public string Fax { get; set; }

        public string State { set; get; }

        public string City { get; set; }

        [JsonProperty("address")]
        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        [RegularExpression(@"^[0-9]+$", ErrorMessage = "ZipCode must be numbers only.")]
        public string ZipCode { get; set; }

        public string GeoLat { get; set; }

        public string GeoLng { get; set; }

        //TODO What is this status. It is not used in Post and Put
        /// <summary>
        /// 0 - deactivated / 1 - activated
        /// </summary>
        public bool IsStatusActive { get; set; }

        public bool IsEnabled { set; get; }

        public int CompanyId { get; set; }

        // TODO: Add new props to store branch contact
        public BranchContact BranchContacts { get; set; }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}


