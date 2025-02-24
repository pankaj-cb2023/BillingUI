using BillingUI.Common.Model;

namespace BillingUI.Data.IRepository
{
    public interface IUserRepository
    {
        Task<UserModel> GetUser(string userName);
        Task<UserResponseModel> SearchUsersAsync(UserModel searchTerm);

        Task<UserViewRoleModel> GetRoles(int? roleId);

        Task<UserModel> AddUsersAsync(AddUserRequestModel searchTerm);
        Task<bool> DeleteUser(int userId);
    }
}
