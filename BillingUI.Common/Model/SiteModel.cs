using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingUI.Common.Model
{
    public class SiteModel
    {
        public string SiteId { get; set; }
        public string ContractNo { get; set; }
        public string Status { get; set; }
        public string SiteName { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string ContactPhone { get; set; }
        public string ContactFax { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string County { get; set; }
        public string Branch { get; set; }
        public string TechId { get; set; }
        public string SalesRepId { get; set; }
        public int? Beds { get; set; }
        public string AdditionalEquipment { get; set; }
        public string UserEntered { get; set; }
        public string UserEdited { get; set; }
        public string DateEntered { get; set; }
        public string DateEdited { get; set; }
        public string BillingAgent { get; set; }
        public decimal? CreditLimit { get; set; }
        public string RateId { get; set; }
        public int? MaxValFails { get; set; }
        public decimal? MinDayRev { get; set; }
        public int? MinDayCalls { get; set; }
        public int? PinGroup { get; set; }
        public string SiteCode { get; set; }
        public string Region { get; set; }
        public string ContactPhoneExtension { get; set; }
        public string ContactEmailAddress { get; set; }
        public string PayPhone { get; set; }
        public string Notes { get; set; }
        public string CommissionRate { get; set; }
        public string CommissionRule { get; set; }
        public string CommissionTerm { get; set; }
    }

    public class SiteSearchVM
    {
        public string? SiteId { get; set; }
        public string? SiteName { get; set; }
        public string? State { get; set; }
        public string? ContractNo { get; set; }

        [DefaultValue(1)]
        public int PageNumber { get; set; }
        [DefaultValue(10)]
        public int PageSize { get; set; }

    }
    public class AddSiteVM
    {
        public int? billingRateId { get; set; }
        public string? siteId { get; set; }
        public string? contractId { get; set; }

    }
    public class GetSiteDetailVM 
    {
        public string? SiteId { get; set; }
        public string? SiteName { get; set; }
        public string? ContractNo { get; set; }
    }


}
