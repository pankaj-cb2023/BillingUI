using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingUI.Common.Model
{
    public class BillingRateModel
    {
        public int? BillingRateId { get; set; }
        public string? SiteId { get; set; }
        public string? ContractNo { get; set; }
        public string? SiteName { get; set; }
        public string? State { get; set; }
        public string? CallTypeCd { get; set; }
        public DateTime? StartDt { get; set; }
        public DateTime? EndDt { get; set; }
        public bool? Pending { get; set; }
        public bool? Historical { get; set; }
        public bool? Active { get; set; }
        public decimal? SecurusRatePerMinAmt { get; set; }
        public decimal? CommRatePerMinAmt { get; set; }
        public decimal? AgencyRatePerMinAmt { get; set; }
        public bool? IncludeAgencyRateInTotalFl { get; set; }
        public int? CommTypeId { get; set; }
        public string? CommTypeDs { get; set; }
        public int? RoundingThresholdNr { get; set; }
        public string? AuditNoteTxt { get; set; }
        public DateTime? CreateDt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDt { get; set; }
        public string? ModifiedBy { get; set; }
        public string SortOrder { get; set; }


    }

    public class BillingRateSearchModel
    {
        public string? SiteId { get; set; }
        public string? ContractNo { get; set; }
        public string? SiteName { get; set; }
        public string? State { get; set; }
        public string? CallTypeCd { get; set; }
        public DateTime? StartDt { get; set; }
        public bool? Pending { get; set; }
        public bool? Historical { get; set; }
        public bool? Active { get; set; }

        [DefaultValue(1)]
        public int PageNumber { get; set; }
        [DefaultValue(10)]
        public int PageSize { get; set; }

        [DefaultValue("ASC")]
        public string? SortOrder { get; set; }
        public string? SortColumn { get; set; }
        
        public string? IsSiteEmpty { get; set; }

        [DefaultValue(1)]
        public int? SPageNumber { get; set; }
        [DefaultValue(10)]
        public int SPageSize { get; set; }

    }

    public class BillingRateUpdateModel
    {
        public int? BillingRateId { get; set; }
        public string? SiteName { get; set; }
        public string? SiteId { get; set; }

    }

    public class BillingRateSite
    {
        public BillingRateSite()
        {
            Data = [];
        }
        public List<BillingRateModel> Data { get; set; }
        public int TotalCount { get; set; }

    }

    public class BillingRateReponseModel
    {
        public BillingRateReponseModel()
        {
            Data = [];
            Site = [];         
        }
        public List<BillingRateModel> Data { get; set; }
        public List<BillingRateModel> Site { get; set; }   

        public int TotalCount { get; set; }
        public int SiteTotalCount { get; set; }
    }

    public class BillingRateAddModel 
    {
        public string? contractNoForModal { get; set; }
        public string? callTypeForModal { get; set; }
        public string? SiteId { get; set; }
        public string? SiteName { get; set; }
        public decimal? securusRate { get; set; }
        public decimal? commRate { get; set; }
        public decimal? agencyRate { get; set; }
        public decimal? includeAgencyInTotal { get; set; }
        public int? commType { get; set; }
        public int? roundingThreshold { get; set; }
        public bool? activeStatus { get; set; }
        public DateTime? startDate { get; set; }
        public string? auditNote { get; set; }

    }
}

