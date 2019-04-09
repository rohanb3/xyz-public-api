﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Xyzies.TWC.Public.Api.Models
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class CompanyModel
    {
        public int Id { get; set; }
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
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        public int Agentid { get; set; }
        public int Status { get; set; }
        public int StoreLocationCount { get; set; }
        public string PrimaryContactName { get; set; }
        public string PrimaryContactTitle { get; set; }

        public bool IsEnabled { get; set; } = true;

        [Phone(ErrorMessage = "Please enter valid fax no.")]
        public string Fax { get; set; }

        public string FedId { get; set; }
        public int TypeOfCompany { get; set; }
        public string StateEstablished { get; set; }
        public byte CompanyType { get; set; }
        public string CallerId { get; set; }
        public bool IsAgreement { get; set; }
        public string ActivityStatus { get; set; }
        public Guid CompanyKey { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CellNumber { get; set; }
        public string BankNumber { get; set; }
        public string BankName { get; set; }
        public string BankAccountNumber { get; set; }
        public string XyziesId { get; set; }
        public DateTime ApprovedDate { get; set; }
        public bool BankInfoGiven { get; set; }
        public int AccountManager { get; set; }
        public string CrmCompanyId { get; set; }
        public bool IsCallCenter { get; set; }
        public int ParentCompanyId { get; set; }
        public Guid TeamKey { get; set; }
        public Guid RetailerGroupKey { get; set; }
        public string SocialMediaAccount { get; set; }
        public string RetailerGoogleAccount { get; set; }
        public string RetailerGooglePassword { get; set; }
        public int PaymentMode { get; set; }
        public int CustomerDemographicId { get; set; }
        public int LocationTypeId { get; set; }
        public bool IsOwnerPassBackground { get; set; }
        public bool IsWebsite { get; set; }
        public bool IsSellsLifelineWireless { get; set; }
        public int NumberofStores { get; set; }
        public string BusinessDescription { get; set; }
        public string WebsiteList { get; set; }
        public bool IsSpectrum { get; set; }
        public int BusinessSource { get; set; }
        public string GeoLat { get; set; }
        public string GeoLon { get; set; }
        public bool IsMarketPlace { get; set; }
        public string MarketPlaceName { get; set; }
        public string PhysicalName { get; set; }
        public string MarketStrategy { get; set; }
        public bool NoSyncInfusion { get; set; }

        public int? CountSalesRep { get; set; }
        public int? CountBranch { get; set; }

        public IList<int> UserIds { get; set; } = new List<int>();

        public virtual IList<BranchModel> Branches { get; set; } = new List<BranchModel>();
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
