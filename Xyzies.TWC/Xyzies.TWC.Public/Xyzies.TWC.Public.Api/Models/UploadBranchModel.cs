﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Xyzies.TWC.Public.Data.Entities;

namespace Xyzies.TWC.Public.Api.Models
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class UploadBranchModel
    {
        public string BranchName { get; set; }

        [MaxLength(250)]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Please enter valid phone no.")]
        public string Phone { get; set; }

        [Phone(ErrorMessage = "Please enter valid fax no.")]
        public string Fax { get; set; }


        public string State { set; get; }
        public string City { get; set; }
        public string Address { get; set; }

        [RegularExpression(@"^[0-9]+$", ErrorMessage = "ZipCode must be numbers only.")]
        public string ZipCode { get; set; }
        public string GeoLat { get; set; }
        public string GeoLon { get; set; }
        public string Status { get; set; }

        public int CompanyId { get; set; }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}


