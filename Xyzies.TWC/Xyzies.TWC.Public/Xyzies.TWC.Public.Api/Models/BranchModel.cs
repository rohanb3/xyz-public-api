﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xyzies.TWC.Public.Data.Entities;

namespace Xyzies.TWC.Public.Api.Models
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class BranchModel
    {
        [ScaffoldColumn(false)]
        public int BranchId { get; set; }
        public string BranchName { get; set; }

        [MaxLength(250)]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Please enter valid phone no.")]
        public string Phone { get; set; }

        [Phone(ErrorMessage = "Please enter valid fax no.")]
        public string Fax { get; set; }

        public string Address { get; set; }
        public string City { get; set; }

        [RegularExpression(@"^[0-9]+$", ErrorMessage = "ZipCode must be numbers only.")]
        public string ZipCode { get; set; }
        public string GeoLat { get; set; }
        public string GeoLon { get; set; }
        public string Status { get; set; }
        public string State { set; get; }

        [DataType(DataType.DateTime)]
        public DateTime? CreatedDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? ModifiedDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }

        public int CompanyId { get; set; }

        [ScaffoldColumn(false)]
        public virtual IList<BranchContact> BranchContacts { get; set; } = new List<BranchContact>();

        //[ScaffoldColumn(false)]
        //public virtual Company ParentCompany { get; set; }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}