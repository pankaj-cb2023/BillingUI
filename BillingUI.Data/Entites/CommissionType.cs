using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingUI.Data.Entites
{
    public class CommissionType:BaseEntity
    {

        [Key]
        public int CommTypeId { get; set; }
        public string CommTypeDs { get; set; } 
      
    }
}
