using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingUI.Data.Entites
{
    public class BaseEntity
    {
        public DateTime CreateDt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDt { get; set; }
        public string ModifiedBy { get; set; }
    }
}
