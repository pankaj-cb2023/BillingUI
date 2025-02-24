using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingUI.Common.Model
{
    public class UserRoleModel
    {
        public int RoleId { get; set; }
        public string? RoleName { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModuleName { get; set; }
        public int RolePermissionId { get; set; }
        public bool? CanRead { get; set; }
        public bool? CanWrite { get; set; }

    }

    public class UserViewRoleModel
    {
        public List<UserRoleModel>? Data { get; set; }
       
    }
}
