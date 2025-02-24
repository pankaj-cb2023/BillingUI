using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingUI.Common.Model
{
    public class CommTypeModel
    {
        public int CommTypeId { get; set; }
        public string commTypeDs { get; set; }
        public DateTime CreateDt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDt { get; set; }
        public string ModifiedBy { get; set; }
    }
}
