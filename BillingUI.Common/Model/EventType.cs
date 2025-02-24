using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingUI.Common.Model
{
    public class EventType
    {       
        public int EventTypeID { get; set; } // int
        public string EventTypeCd { get; set; } // char(3)
        public string EventTypeDesc { get; set; } // varchar(100)
    }
}
