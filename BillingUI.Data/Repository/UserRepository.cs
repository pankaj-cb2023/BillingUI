using BillingUI.Common.Model;
using BillingUI.Data.Entites;
using BillingUI.Data.IRepository;
using BillingUI.Common.Infrastructure;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace BillingUI.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _dbConnection;
        private readonly IConfiguration _configuration;


        /// <summary>
        /// parameterized constructor for DI
        /// </summary>
        /// <param name="dbConnection"></param>

        public UserRepository(IDbConnection dbConnection, IConfiguration configuration)
        {
            _dbConnection = dbConnection;
            _configuration = configuration;
        }

        public async Task EnsureConnectionOpenAsync(SqlConnection sqlConnection)
        {
            if (sqlConnection.State == ConnectionState.Closed)
            {
                await sqlConnection.OpenAsync();
            }
        }

        public async Task<UserModel?> GetUser(string userName)
        {
            UserModel? userInfo = null;

            try
            {
                if (_dbConnection is SqlConnection sqlConnection)
                {
                    await EnsureConnectionOpenAsync(sqlConnection);

                    using var cmd = sqlConnection.CreateCommand();
                    cmd.CommandText = "dbo.usp_pmui_CheckUserExists";
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Input parameter
                    cmd.Parameters.Add(new SqlParameter("@UserName", SqlDbType.NVarChar, 100) { Value = userName });

                    // Output parameter
                    var outputParam = new SqlParameter("@UserExists", SqlDbType.NVarChar, 10)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputParam);

                    // Execute stored procedure and retrieve user details
                    using var reader = await cmd.ExecuteReaderAsync();

                    if (reader.Read()) // If user exists
                    {
                        userInfo = new UserModel
                        {
                            UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                            UserName = reader.GetString(reader.GetOrdinal("UserName")),
                            RoleId = reader.IsDBNull(reader.GetOrdinal("RoleId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("RoleId")),
                            RoleName = reader.IsDBNull(reader.GetOrdinal("RoleName")) ? null : reader.GetString(reader.GetOrdinal("RoleName")),
                            Permissions = []
                        };

                        // Fetch all permissions and modules
                        do
                        {
                            var permission = new PermissionModel
                            {
                                CanRead = !reader.IsDBNull(reader.GetOrdinal("CanRead")) && reader.GetBoolean(reader.GetOrdinal("CanRead")),
                                CanWrite = !reader.IsDBNull(reader.GetOrdinal("CanWrite")) && reader.GetBoolean(reader.GetOrdinal("CanWrite")),
                                ModuleName = reader.IsDBNull(reader.GetOrdinal("ModuleName")) ? null : reader.GetString(reader.GetOrdinal("ModuleName")),

                            };

                            userInfo.Permissions.Add(permission);
                        } while (reader.Read());
                    }

                    // Close the reader before fetching output parameter
                    reader.Close();

                    // Retrieve the output parameter value
                    string result = outputParam.Value?.ToString()?.Trim() ?? "False";

                    // If output param is "False", set userInfo to null
                    if (result.Equals("False", StringComparison.OrdinalIgnoreCase))
                    {
                        userInfo = null;
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                throw new Exception("An error occurred while retrieving user details from the database.", sqlEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while retrieving user details.", ex);
            }

            return userInfo;
        }



        public async Task<UserResponseModel> SearchUsersAsync(UserModel searchTerm)
        {
            var users = new UserResponseModel { Data = [] };
            var parameters = new List<SqlParameter>
            {
                new("@SearchTerm", searchTerm?.SearchQuery ?? (object)DBNull.Value),
                new("@PageNumber", searchTerm?.PageNumber > 0 ? searchTerm.PageNumber : 1), // Default to 1
                new("@PageSize", searchTerm?.PageSize > 0 ? searchTerm.PageSize: 5) // Default to 5                
            };

            try
            {
                if (_dbConnection is SqlConnection sqlConnection)
                {
                    await EnsureConnectionOpenAsync(sqlConnection);

                    using var cmd = sqlConnection.CreateCommand();
                    cmd.CommandText = "dbo.usp_pmui_SearchUser";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parameters.ToArray());

                    using var reader = await cmd.ExecuteReaderAsync();
                    var totalCount = 0;

                    while (await reader.ReadAsync())
                    {
                        totalCount = reader.GetInt32(reader.GetOrdinal("TotalRecords"));
                        users.Data.Add(new UserModel
                        {
                            UserId = reader.GetInt32(reader.GetOrdinal("Id")),
                            UserName = reader.GetString(reader.GetOrdinal("UserName")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),                          
                            RoleName = reader.GetString(reader.GetOrdinal("RoleName"))                          
                        });
                    }

                    users.TotalCount = totalCount;
                }
            }
            catch (SqlException sqlEx)
            {
                throw new Exception("An error occurred while retrieving users from the database.", sqlEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while searching for users.", ex);
            }

            return users;
        }

        public async Task<UserViewRoleModel> GetRoles(int? roleId)
        {
            var roles = new UserViewRoleModel { Data = new List<UserRoleModel>() };

            try
            {
                if (_dbConnection is SqlConnection sqlConnection)
                {
                    await EnsureConnectionOpenAsync(sqlConnection);

                    using var cmd = sqlConnection.CreateCommand();
                    cmd.CommandText = "dbo.usp_pmui_GetRoles";
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Add RoleId parameter if it's provided
                    cmd.Parameters.AddWithValue("@RoleId", (roleId.HasValue && roleId > 0) ? roleId.Value : (object)DBNull.Value);

                    using var reader = await cmd.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            // Check if RoleId is provided
                            if (roleId.HasValue && roleId> 0)
                            {
                                // If RoleId is provided, return detailed role permissions
                                var user = new UserRoleModel
                                {
                                    RoleId = reader.GetInt32(reader.GetOrdinal("RoleId")),
                                    RoleName = reader.GetString(reader.GetOrdinal("RoleName")),
                                    ModuleName = reader.GetString(reader.GetOrdinal("ModuleName")),
                                    RolePermissionId = reader.GetInt32(reader.GetOrdinal("RolePermissionId")),
                                    CanRead = reader.GetBoolean(reader.GetOrdinal("CanRead")),
                                    CanWrite = reader.GetBoolean(reader.GetOrdinal("CanWrite"))
                                };
                                roles.Data.Add(user);
                            }
                            else
                            {
                                // If RoleId is not provided, return basic role list
                                var user = new UserRoleModel
                                {
                                    RoleId = reader.GetInt32(reader.GetOrdinal("Id")),
                                    RoleName = reader.GetString(reader.GetOrdinal("RoleName"))
                                };
                                roles.Data.Add(user);
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("No roles were returned from the stored procedure.");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching roles.", ex);
            }

            return roles;
        }


        public async Task<UserModel> AddUsersAsync(AddUserRequestModel user)
        {
            UserModel resultUser = null;
            var parameters = new List<SqlParameter>
            {
                new("@UserName", user.UserName ?? (object)DBNull.Value),
                new("@Email", user.Email ?? (object)DBNull.Value),
                new("@RoleId", user.Role)
            };

            try
            {
                if (_dbConnection is SqlConnection sqlConnection)
                {
                    await EnsureConnectionOpenAsync(sqlConnection);

                    using var cmd = sqlConnection.CreateCommand();
                    cmd.CommandText = "dbo.usp_pmui_UpsertUserWithRole";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parameters.ToArray());

                    using var reader = await cmd.ExecuteReaderAsync();
                    if (await reader.ReadAsync())
                    {
                        // Map all the relevant user fields returned from the stored procedure
                        resultUser = new UserModel
                        {
                            UserId = reader.IsDBNull(reader.GetOrdinal("UserId")) ? 0 : reader.GetInt32(reader.GetOrdinal("UserId")),
                            UserName = reader.IsDBNull(reader.GetOrdinal("UserName")) ? string.Empty : reader.GetString(reader.GetOrdinal("UserName")),
                            Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? string.Empty : reader.GetString(reader.GetOrdinal("Email")),
                            IsActive = reader.IsDBNull(reader.GetOrdinal("IsActive")) ? false : reader.GetBoolean(reader.GetOrdinal("IsActive")),
                            CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? string.Empty : reader.GetString(reader.GetOrdinal("CreatedBy")),
                            ModifiedAt = reader.IsDBNull(reader.GetOrdinal("ModifiedAt")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("ModifiedAt")),
                            ModifiedBy = reader.IsDBNull(reader.GetOrdinal("ModifiedBy")) ? string.Empty : reader.GetString(reader.GetOrdinal("ModifiedBy")),
                            RoleId = reader.IsDBNull(reader.GetOrdinal("RoleId")) ? 0 : reader.GetInt32(reader.GetOrdinal("RoleId")),
                            
                        };
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                throw new Exception("An error occurred while adding the user to the database.", sqlEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while adding the user.", ex);
            }

            return resultUser;
        }

        public async Task<bool> DeleteUser(int userId)
        {
            bool isDeleted = false;

            try
            {
                if (_dbConnection is SqlConnection sqlConnection)
                {
                    await EnsureConnectionOpenAsync(sqlConnection);

                    using var cmd = sqlConnection.CreateCommand();
                    cmd.CommandText = "dbo.usp_pmui_DeleteUserRole";
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Input parameter
                    cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = userId });

                    // Output parameter
                    var outputParam = new SqlParameter("@IsDeleted", SqlDbType.NVarChar, 10)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputParam);

                    // Execute stored procedure
                    await cmd.ExecuteNonQueryAsync();

                    // Retrieve the output parameter value
                    string result = outputParam.Value?.ToString().Trim();

                    // Convert the result to boolean
                    isDeleted = result.Equals("True", StringComparison.OrdinalIgnoreCase);
                }
            }
            catch (SqlException sqlEx)
            {
                throw new Exception("An error occurred while deleting the user from the database.", sqlEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while deleting the user.", ex);
            }

            return isDeleted;
        }



    }
}

