using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingUI.Data.Entites
{
    public  class BillingRate : BaseEntity
    {
        [Key]
        public int BillingRateId { get; set; }
        public string? ContractId { get; set; }  
        public string? SiteId { get; set; }  
        public string? CallTypeCd { get; set; }    
        public decimal SecurusRatePerMinAmt { get; set; } 
        public decimal CommRatePerMinAmt { get; set; }
        public int CommTypeId { get; set; }
        public decimal AgencyRatePerMinAmt { get; set; } 
        public bool IncludeAgencyRateInTotalFl { get; set; } 
        public DateTime StartDt { get; set; } 
        public DateTime EndDt { get; set; } 
        public bool ActiveFl { get; set; } 
        public int RoundingThresholdNr { get; set; } 
        public string? AuditNoteTxt { get; set; }    
       
    }
}
