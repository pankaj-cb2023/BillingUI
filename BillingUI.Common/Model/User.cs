using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingUI.Common.Model
{
    public class UserModel : SearchUserModel
    {
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public bool? IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public string? RoleName { get; set; }
        public int? RoleId { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public List<PermissionModel>? Permissions { get; set; }

    }

    public class PermissionModel
    {
        public bool CanRead { get; set; }
        public bool CanWrite { get; set; }
        public string? ModuleName { get; set; }
    }
    public class UserResponseModel 
    { 
        public List<UserModel>? Data { get; set; }
        public int TotalCount { get; set; }
    }

    public class SearchUserModel 
    {
        public string? SearchQuery { get; set; }

        [DefaultValue(1)]
        public int PageNumber { get; set; }
        [DefaultValue(10)]
        public int PageSize { get; set; }
    }


    public class AddUserRequestModel
    {
        public string? UserName { get; set; }
        public int Role { get; set; }
        public string? Email { get; set; }
    }

}
