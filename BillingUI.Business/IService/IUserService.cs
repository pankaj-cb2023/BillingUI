using BillingUI.Common.Model;
using BillingUI.Common.Infrastructure;

namespace BillingUI.Business.IService
{
    public interface IUserService
    {

        Task<ServiceResult<UserModel>> GetUser(string userName);
        Task<ServiceResult<UserResponseModel>> SearchUsersAsync(UserModel searchTerm);
        Task<ServiceResult<UserViewRoleModel>> GetRoles(int? roleId);

        Task<ServiceResult<UserModel>> AddUsersAsync(AddUserRequestModel userModel);
        Task<ServiceResult<bool>> DeleteUser(int userId);

    }
}
