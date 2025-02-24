using BillingUI.Business.IService;
using BillingUI.Common.Infrastructure;
using BillingUI.Common.Model;
using BillingUI.Data.IRepository;
using System.Data;


namespace BillingUI.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository UserRepository)
        {
            _userRepository = UserRepository;

        }

        public async Task<ServiceResult<UserModel>> GetUser(string userName)
        {
            var serviceResult = new ServiceResult<UserModel>();
            var data = await _userRepository.GetUser(userName);
            if (data!=null)
            {
                serviceResult.SetSuccess(data, "Get user Successfully!");
            }
            else
            {
                serviceResult.SetError("Error to get user!");
            }
            return serviceResult;
        }

        public async Task<ServiceResult<UserResponseModel>> SearchUsersAsync(UserModel searchTerm)
        {
            var serviceResult = new ServiceResult<UserResponseModel>();
            var data = await _userRepository.SearchUsersAsync(searchTerm);
            if (data != null)
            {
                serviceResult.SetSuccess(data, "Get user Successfully!");
            }
            else
            {
                serviceResult.SetError("Error to get user!");
            }
            return serviceResult;
        }

        public async Task<ServiceResult<UserViewRoleModel>> GetRoles(int? roleId)
        {

            var serviceResult = new ServiceResult<UserViewRoleModel>();
            var data = await _userRepository.GetRoles(roleId);
            if (data != null)
            {
                serviceResult.SetSuccess(data, "Get site Successfully!");
            }
            else
            {
                serviceResult.SetError("Error to get site!");
            }
            return serviceResult;

        }

        public async Task<ServiceResult<UserModel>> AddUsersAsync(AddUserRequestModel userModel)
        {
            var serviceResult = new ServiceResult<UserModel>();
            var data = await _userRepository.AddUsersAsync(userModel);
            if (data != null)
            {
                serviceResult.SetSuccess(data, "Get user Successfully!");
            }
            else
            {
                serviceResult.SetError("Error to get user!");
            }
            return serviceResult;
        }

        public async Task<ServiceResult<bool>> DeleteUser(int userId)
        {
            var serviceResult = new ServiceResult<bool>();
            var data = await _userRepository.DeleteUser(userId);
            if (data)
            {
                serviceResult.SetSuccess(data, "User deleted Successfully!");
            }
            else
            {
                serviceResult.SetError("Error to get user!");
            }
            return serviceResult;
        }


    }

}






