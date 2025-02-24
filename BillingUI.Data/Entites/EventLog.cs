using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingUI.Data.Entites
{
    public class EventLog
    {
        [Key]
        public long EventLogID { get; set; } 
        public short EventSourceID { get; set; } 
        public int EventTypeID { get; set; } 
        public string LogUser { get; set; } 
        public DateTime LogDt { get; set; }
        public long? RecordID { get; set; } 
        public string EventDetails { get; set; } 
        public string Notes { get; set; } 
        public long? AltRecordId { get; set; } 
    }
}
