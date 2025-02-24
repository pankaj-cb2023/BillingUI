using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingUI.Common.Model
{
    public class EventSource
    {
        public short EventSourceID { get; set; }
        public string EventSourceCd { get; set; }
        public string EventSourceDesc { get; set; }
    }
}
