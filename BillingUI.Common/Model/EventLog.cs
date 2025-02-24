using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingUI.Common.Model
{
    public class EventLog
    {     
        public string SourceCd { get; set; }
        public string TypeCd { get; set; }
        public string LogUser { get; set; }
        public DateTime LogDt { get; set; }
        public long? RecordID { get; set; }
        public string EventDetails { get; set; }
        public string Notes { get; set; }
        public long? AltRecordId { get; set; }
    }
    public class EventLogResponseModel
    {
        public long EventLogID { get; set; }
        public short EventSourceID { get; set; }
        public int EventTypeID { get; set; }
        public string LogUser { get; set; }
        public DateTime LogDt { get; set; }
        public long? RecordID { get; set; }
        public string EventDetails { get; set; }
        public string Notes { get; set; }
        public long? AltRecordId { get; set; }

        public DateTime? StartEventDate { get; set; }
        public string EventType { get; set; }


    }

    public class EventLogRequestModel
    {
        [DefaultValue("PMUI_RATES")]
        public string EventSource { get; set; } 
        public string? EventType { get; set; }   
        public long RecordID { get; set; }      
        public long? AltRecordID { get; set; }

        [DefaultValue(true)]
        public bool ShowUpdates { get; set; }   
        public bool ShowLocks { get; set; }
        [DefaultValue(10)]
        public int PageSize { get; set; }
        [DefaultValue(1)]
        public int CurrentPage { get; set; }         
     
        public DateTime? StartEventDate { get; set; } 
        public DateTime? EndEventDate { get; set; }
        [DefaultValue(0)]
        public int TotalRows { get; set; }
        [DefaultValue(0)]
        public int TotalPages { get; set; }   
    }

}
