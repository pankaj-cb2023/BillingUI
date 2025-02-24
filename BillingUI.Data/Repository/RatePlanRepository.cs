using BillingUI.Common.Model;
using BillingUI.Data.Entites;
using BillingUI.Data.IRepository;
using BillingUI.Common.Infrastructure;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace BillingUI.Data.Repository
{
    public class RatePlanRepository : IRatePlanRepository
    {
        private readonly IDbConnection _dbConnection;
        private readonly IConfiguration _configuration;


        /// <summary>
        /// parameterized constructor for DI
        /// </summary>
        /// <param name="dbConnection"></param>

        public RatePlanRepository(IDbConnection dbConnection, IConfiguration configuration)
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

        public async Task<BillingRateSite> Search(BillingRateSearchModel billingRateSearchModel)
        {
            var parameters = new List<SqlParameter>
            {
                new("@ContractId", billingRateSearchModel.ContractNo ?? null),
                new("@SiteID", billingRateSearchModel.SiteId ?? null),
                new("@SiteName", billingRateSearchModel.SiteName ?? null),
                new("@State", billingRateSearchModel.State ?? null),
                new("@CallTypeCd", billingRateSearchModel.CallTypeCd),
                new("@StartDt", billingRateSearchModel.StartDt ?? null),
                new("@Pending", billingRateSearchModel.Pending),
                new("@Historical", billingRateSearchModel.Historical),
                new("@ActiveFl", billingRateSearchModel.Active),
                new("PageNumber",billingRateSearchModel.PageNumber),
                new("PageSize",billingRateSearchModel.PageSize),
                new("SortColumn",billingRateSearchModel.SortColumn),
                new("SortOrder",billingRateSearchModel.SortOrder),
               
            };

            var records = new BillingRateSite();
            try
            {
                if (_dbConnection is SqlConnection sqlConnection)
                {
                    await EnsureConnectionOpenAsync(sqlConnection);
                }

                if (_dbConnection is SqlConnection commandConnection)
                {
                    using var cmd = commandConnection.CreateCommand();
                    cmd.CommandText = "dbo.usp_pmui_SearchBillingRate";
                    cmd.CommandType = CommandType.StoredProcedure;


                    foreach (var param in parameters)
                    {
                        cmd.Parameters.Add(param);
                    }

                    if (cmd is SqlCommand sqlCmd)
                    {
                        try
                        {
                            using var reader = sqlCmd.ExecuteReader();

                            while (reader.Read())
                            {
                                records.TotalCount = reader.GetInt32(reader.GetOrdinal("TotalCount"));

                                var ratePlan = new BillingRateModel
                                {
                                    BillingRateId = reader.GetInt32(reader.GetOrdinal("BillingRateId")),
                                    ContractNo = reader.GetString(reader.GetOrdinal("ContractId")),
                                    SiteId = reader.IsDBNull(reader.GetOrdinal("SiteId")) ? null : reader.GetString(reader.GetOrdinal("SiteId")),
                                    SiteName = reader.IsDBNull(reader.GetOrdinal("SiteName")) ? null : reader.GetString(reader.GetOrdinal("SiteName")),
                                    State = reader.IsDBNull(reader.GetOrdinal("State")) ? null : reader.GetString(reader.GetOrdinal("State")),
                                    CallTypeCd = reader.IsDBNull(reader.GetOrdinal("CallTypeCd")) ? null : reader.GetString(reader.GetOrdinal("CallTypeCd")),
                                    CommTypeDs = reader.IsDBNull(reader.GetOrdinal("CommTypeDs")) ? null : reader.GetString(reader.GetOrdinal("CommTypeDs")),
                                    SecurusRatePerMinAmt = reader.IsDBNull(reader.GetOrdinal("SecurusRatePerMinAmt")) ? 0 : reader.GetDecimal(reader.GetOrdinal("SecurusRatePerMinAmt")),
                                    CommRatePerMinAmt = reader.IsDBNull(reader.GetOrdinal("CommRatePerMinAmt")) ? 0 : reader.GetDecimal(reader.GetOrdinal("CommRatePerMinAmt")),
                                    AgencyRatePerMinAmt = reader.IsDBNull(reader.GetOrdinal("AgencyRatePerMinAmt")) ? 0 : reader.GetDecimal(reader.GetOrdinal("AgencyRatePerMinAmt")),
                                    RoundingThresholdNr = reader.IsDBNull(reader.GetOrdinal("RoundingThresholdNr")) ? 0 : reader.GetInt32(reader.GetOrdinal("RoundingThresholdNr")),
                                    AuditNoteTxt = reader.IsDBNull(reader.GetOrdinal("AuditNoteTxt")) ? null : reader.GetString(reader.GetOrdinal("AuditNoteTxt")),
                                    StartDt = reader.IsDBNull(reader.GetOrdinal("StartDt")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("StartDt")),
                                    EndDt = reader.IsDBNull(reader.GetOrdinal("EndDt")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("EndDt")),
                                    Active = reader.IsDBNull(reader.GetOrdinal("ActiveFl")) ? false : reader.GetBoolean(reader.GetOrdinal("ActiveFl")),
                                    CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? null : reader.GetString(reader.GetOrdinal("CreatedBy")),
                                    CreateDt = reader.GetDateTime(reader.GetOrdinal("CreateDt")),
                                    ModifiedBy = reader.IsDBNull(reader.GetOrdinal("ModifiedBy")) ? null : reader.GetString(reader.GetOrdinal("ModifiedBy")),
                                    ModifiedDt = reader.IsDBNull(reader.GetOrdinal("ModifiedDt")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("ModifiedDt")),
                                    SortOrder = billingRateSearchModel.SortOrder
                                };
                                records.Data.Add(ratePlan);
                            }

                        }
                        catch (Exception ex)
                        {

                            throw;
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                throw new ApplicationException("An error occurred while executing the database operation.", sqlEx);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred.", ex);
            }
            return records;
        }

        public async Task<BillingRateSite> SearchSite(BillingRateSearchModel billingRateSearch)
        {
            var parameters = new List<SqlParameter>
            {
                new("@ContractId", billingRateSearch.ContractNo ?? null),
                new("@SiteID", billingRateSearch.SiteId ?? null),
                new("@SiteName", billingRateSearch.SiteName ?? null),
                new("@State", billingRateSearch.State ?? null),
                new("@CallTypeCd", billingRateSearch.CallTypeCd),
                new("@StartDt", billingRateSearch.StartDt ?? null),
                new("@ActiveFl", billingRateSearch.Active),
                new("PageNumber",billingRateSearch.SPageNumber),
                new("PageSize",billingRateSearch.SPageSize),
                new("SortColumn",billingRateSearch.SortColumn),
                new("SortOrder",billingRateSearch.SortOrder)
            };

            var records = new BillingRateSite();
            try
            {
                if (_dbConnection is SqlConnection sqlConnection)
                {
                    await EnsureConnectionOpenAsync(sqlConnection);
                }

                if (_dbConnection is SqlConnection commandConnection)
                {
                    using var cmd = commandConnection.CreateCommand();
                    cmd.CommandText = "dbo.usp_pmui_SearchBillingRateSite";
                    cmd.CommandType = CommandType.StoredProcedure;


                    foreach (var param in parameters)
                    {
                        cmd.Parameters.Add(param);
                    }

                    if (cmd is SqlCommand sqlCmd)
                    {
                        try
                        {
                            using var reader = sqlCmd.ExecuteReader();

                            while (reader.Read())
                            {
                                records.TotalCount = reader.GetInt32(reader.GetOrdinal("TotalCount"));
                                 
                                var ratePlan = new BillingRateModel
                                {
                                    BillingRateId = reader.IsDBNull(reader.GetOrdinal("BillingRateId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("BillingRateId")),
                                    ContractNo = reader.IsDBNull(reader.GetOrdinal("ContractId")) ? null : reader.GetString(reader.GetOrdinal("ContractId")),
                                    SiteId = reader.IsDBNull(reader.GetOrdinal("SiteId")) ? null : reader.GetString(reader.GetOrdinal("SiteId")),
                                    SiteName = reader.IsDBNull(reader.GetOrdinal("SiteName")) ? null : reader.GetString(reader.GetOrdinal("SiteName")),
                                    State = reader.IsDBNull(reader.GetOrdinal("State")) ? null : reader.GetString(reader.GetOrdinal("State")),
                                    CallTypeCd = reader.IsDBNull(reader.GetOrdinal("CallTypeCd")) ? null : reader.GetString(reader.GetOrdinal("CallTypeCd")),
                                    CommTypeDs = reader.IsDBNull(reader.GetOrdinal("CommTypeDs")) ? null : reader.GetString(reader.GetOrdinal("CommTypeDs")),
                                    SecurusRatePerMinAmt = reader.IsDBNull(reader.GetOrdinal("SecurusRatePerMinAmt")) ? 0 : reader.GetDecimal(reader.GetOrdinal("SecurusRatePerMinAmt")),
                                    CommRatePerMinAmt = reader.IsDBNull(reader.GetOrdinal("CommRatePerMinAmt")) ? 0 : reader.GetDecimal(reader.GetOrdinal("CommRatePerMinAmt")),
                                    AgencyRatePerMinAmt = reader.IsDBNull(reader.GetOrdinal("AgencyRatePerMinAmt")) ? 0 : reader.GetDecimal(reader.GetOrdinal("AgencyRatePerMinAmt")),
                                    RoundingThresholdNr = reader.IsDBNull(reader.GetOrdinal("RoundingThresholdNr")) ? 0 : reader.GetInt32(reader.GetOrdinal("RoundingThresholdNr")),
                                    AuditNoteTxt = reader.IsDBNull(reader.GetOrdinal("AuditNoteTxt")) ? null : reader.GetString(reader.GetOrdinal("AuditNoteTxt")),
                                    StartDt = reader.IsDBNull(reader.GetOrdinal("StartDt")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("StartDt")),
                                    EndDt = reader.IsDBNull(reader.GetOrdinal("EndDt")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("EndDt")),
                                    Active = reader.IsDBNull(reader.GetOrdinal("ActiveFl")) ? false : reader.GetBoolean(reader.GetOrdinal("ActiveFl")),
                                    CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? null : reader.GetString(reader.GetOrdinal("CreatedBy")),
                                    CreateDt = reader.IsDBNull(reader.GetOrdinal("CreateDt")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("CreateDt")),
                                    ModifiedBy = reader.IsDBNull(reader.GetOrdinal("ModifiedBy")) ? null : reader.GetString(reader.GetOrdinal("ModifiedBy")),
                                    ModifiedDt = reader.IsDBNull(reader.GetOrdinal("ModifiedDt")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("ModifiedDt")),
                                    SortOrder = billingRateSearch.SortOrder
                                };
                                records.Data.Add(ratePlan);
                            }

                        }
                        catch (Exception ex)
                        {

                            throw;
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                throw new ApplicationException("An error occurred while executing the database operation.", sqlEx);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred.", ex);
            }

            return records;
        }

        public async Task<BillingRateSite> GetSitesDataAsync(BillingRateSearchModel siteSearchVM)
        {
            var parameters = new List<SqlParameter>
            {
                new("@ContractId", siteSearchVM.ContractNo ?? null),
                new("@SiteID", siteSearchVM.SiteId ?? null),
                new("@SiteName", siteSearchVM.SiteName ?? null),
                new("@State", siteSearchVM.State ?? null),
                new("@CallTypeCd", siteSearchVM.CallTypeCd),
                new("@StartDt", siteSearchVM.StartDt ?? null),
                new("@Pending", siteSearchVM.Pending),
                new("@Historical", siteSearchVM.Historical),
                new("@ActiveFl", siteSearchVM.Active),
                new("PageNumber",siteSearchVM.PageNumber),
                new("PageSize",siteSearchVM.PageSize),
                new("SortColumn",siteSearchVM.SortColumn),
                new("SortOrder",siteSearchVM.SortOrder)
            };

            var records = new BillingRateSite();
            try
            {
                if (_dbConnection is SqlConnection sqlConnection)
                {
                    if (sqlConnection.State == ConnectionState.Closed)
                    {
                        await sqlConnection.OpenAsync();
                    }
                }

                if (_dbConnection is SqlConnection commandConnection)
                {
                    using var cmd = commandConnection.CreateCommand();
                    cmd.CommandText = "usp_pmui_GetSiteDetail";
                    cmd.CommandType = CommandType.StoredProcedure;

                    foreach (var param in parameters)
                    {
                        cmd.Parameters.Add(param);
                    }

                    if (cmd is SqlCommand sqlCmd)
                    {
                        try
                        {
                            using var reader = sqlCmd.ExecuteReader();
                            while (reader.Read())
                            {
                                records.TotalCount = reader.GetInt32(reader.GetOrdinal("TotalCount"));

                                var ratePlan = new BillingRateModel
                                {
                                    BillingRateId = reader.GetInt32(reader.GetOrdinal("BillingRateId")),
                                    ContractNo = reader.GetString(reader.GetOrdinal("ContractId")),
                                    SiteId = reader.IsDBNull(reader.GetOrdinal("SiteId")) ? null : reader.GetString(reader.GetOrdinal("SiteId")),
                                    SiteName = reader.IsDBNull(reader.GetOrdinal("SiteName")) ? null : reader.GetString(reader.GetOrdinal("SiteName")),
                                    State = reader.IsDBNull(reader.GetOrdinal("State")) ? null : reader.GetString(reader.GetOrdinal("State")),
                                    CallTypeCd = reader.IsDBNull(reader.GetOrdinal("CallTypeCd")) ? null : reader.GetString(reader.GetOrdinal("CallTypeCd")),
                                    CommTypeDs = reader.IsDBNull(reader.GetOrdinal("CommTypeDs")) ? null : reader.GetString(reader.GetOrdinal("CommTypeDs")),
                                    SecurusRatePerMinAmt = reader.IsDBNull(reader.GetOrdinal("SecurusRatePerMinAmt")) ? 0 : reader.GetDecimal(reader.GetOrdinal("SecurusRatePerMinAmt")),
                                    CommRatePerMinAmt = reader.IsDBNull(reader.GetOrdinal("CommRatePerMinAmt")) ? 0 : reader.GetDecimal(reader.GetOrdinal("CommRatePerMinAmt")),
                                    AgencyRatePerMinAmt = reader.IsDBNull(reader.GetOrdinal("AgencyRatePerMinAmt")) ? 0 : reader.GetDecimal(reader.GetOrdinal("AgencyRatePerMinAmt")),
                                    RoundingThresholdNr = reader.IsDBNull(reader.GetOrdinal("RoundingThresholdNr")) ? 0 : reader.GetInt32(reader.GetOrdinal("RoundingThresholdNr")),
                                    AuditNoteTxt = reader.IsDBNull(reader.GetOrdinal("AuditNoteTxt")) ? null : reader.GetString(reader.GetOrdinal("AuditNoteTxt")),
                                    StartDt = reader.IsDBNull(reader.GetOrdinal("StartDt")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("StartDt")),
                                    EndDt = reader.IsDBNull(reader.GetOrdinal("EndDt")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("EndDt")),
                                    Active = reader.IsDBNull(reader.GetOrdinal("ActiveFl")) ? false : reader.GetBoolean(reader.GetOrdinal("ActiveFl")),
                                    CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? null : reader.GetString(reader.GetOrdinal("CreatedBy")),
                                    CreateDt = reader.GetDateTime(reader.GetOrdinal("CreateDt")),
                                    ModifiedBy = reader.IsDBNull(reader.GetOrdinal("ModifiedBy")) ? null : reader.GetString(reader.GetOrdinal("ModifiedBy")),
                                    ModifiedDt = reader.IsDBNull(reader.GetOrdinal("ModifiedDt")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("ModifiedDt")),
                                    SortOrder = siteSearchVM.SortOrder
                                };
                                records.Data.Add(ratePlan);
                            }

                        }
                        catch (Exception ex)
                        {

                            throw;
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                throw new ApplicationException("An error occurred while executing the database operation.", sqlEx);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred.", ex);
            }
            return records;
        }

        public async Task<BillingRateModel> UpdateBillingRate(BillingRateUpdateModel billingRateUpdateModel)
        {
            var parameters = new List<SqlParameter>
            {
                new("@BillingRateId", billingRateUpdateModel.BillingRateId ?? (object)DBNull.Value),
                new("@SiteId", billingRateUpdateModel.SiteId ?? null),
                new("@SiteName", billingRateUpdateModel.SiteName ?? null),
            };

            BillingRateModel result = new();

            try
            {
                if (_dbConnection is SqlConnection sqlConnection)
                {
                    // Ensure the connection is open
                    await EnsureConnectionOpenAsync(sqlConnection);
                }

                if (_dbConnection is SqlConnection commandConnection)
                {
                    using var cmd = commandConnection.CreateCommand();
                    cmd.CommandText = "usp_pmui_Update_BillingRate";
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Add parameters to the command
                    foreach (var param in parameters)
                    {
                        cmd.Parameters.Add(param);
                    }

                    if (cmd is SqlCommand sqlCmd)
                    {
                        try
                        {
                            // Execute the command and get the upserted data
                            using var reader = await sqlCmd.ExecuteReaderAsync();
                            if (reader.Read())
                            {
                                result = new BillingRateModel
                                {
                                    BillingRateId = reader.GetInt32(reader.GetOrdinal("BillingRateId")),
                                    ContractNo = reader.GetString(reader.GetOrdinal("ContractId")),
                                    SiteId = reader.GetString(reader.GetOrdinal("SiteId")),
                                    SiteName = reader.IsDBNull(reader.GetOrdinal("SiteName")) ? null : reader.GetString(reader.GetOrdinal("SiteName")),
                                    State = reader.IsDBNull(reader.GetOrdinal("State")) ? null : reader.GetString(reader.GetOrdinal("State")),
                                    CallTypeCd = reader.GetString(reader.GetOrdinal("CallTypeCd")),
                                    CommTypeDs = reader.GetString(reader.GetOrdinal("CommTypeDs")),
                                    SecurusRatePerMinAmt = reader.GetDecimal(reader.GetOrdinal("SecurusRatePerMinAmt")),
                                    CommRatePerMinAmt = reader.GetDecimal(reader.GetOrdinal("CommRatePerMinAmt")),
                                    AgencyRatePerMinAmt = reader.GetDecimal(reader.GetOrdinal("AgencyRatePerMinAmt")),
                                    RoundingThresholdNr = reader.GetInt32(reader.GetOrdinal("RoundingThresholdNr")),
                                    EndDt = reader.IsDBNull(reader.GetOrdinal("EndDt")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("EndDt")),
                                    Active = reader.GetBoolean(reader.GetOrdinal("ActiveFl")),
                                    CreatedBy = reader.GetString(reader.GetOrdinal("CreatedBy")),
                                    CreateDt = reader.GetDateTime(reader.GetOrdinal("CreateDt")),
                                    ModifiedBy = reader.IsDBNull(reader.GetOrdinal("ModifiedBy")) ? null : reader.GetString(reader.GetOrdinal("ModifiedBy")),
                                    ModifiedDt = reader.IsDBNull(reader.GetOrdinal("ModifiedDt")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("ModifiedDt"))
                                };
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new ApplicationException("An error occurred while executing the database operation.", ex);
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                throw new ApplicationException("An error occurred while executing the database operation.", sqlEx);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred.", ex);
            }

            return result;
        }

        public async Task<BillingRateModel> AddBillingRate(BillingRateAddModel billingRateAddModel)
        {
            // Define default values for all parameters
            var parameters = new List<SqlParameter>
            {             
                new("@ContractId", billingRateAddModel.contractNoForModal ?? null),
                new("@SiteId", billingRateAddModel.SiteId ?? null),
                new("@CallTypeCd",billingRateAddModel.callTypeForModal?? null),
                new("@SecurusRatePerMinAmt", billingRateAddModel.securusRate ?? null),
                new("@CommRatePerMinAmt", billingRateAddModel.commRate ?? null),          
                new("@CommTypeId", billingRateAddModel.commType ?? null),
                new("@AgencyRatePerMinAmt",  billingRateAddModel.agencyRate ?? null),
                new("@IncludeAgencyRateInTotalFl",  billingRateAddModel.includeAgencyInTotal ?? null),
                new("@StartDt",billingRateAddModel.startDate),
                new("@ActiveFl", billingRateAddModel.activeStatus ?? false),
                new("@RoundingThresholdNr",  billingRateAddModel.roundingThreshold ?? null),
                new("@AuditNoteTxt",billingRateAddModel.auditNote ?? null),
                new("@User", "A16" ?? null)
            };

            BillingRateModel result = new();
            try
            {
                if (_dbConnection is SqlConnection sqlConnection)
                {                  
                    await EnsureConnectionOpenAsync(sqlConnection);
                }

                if (_dbConnection is SqlConnection commandConnection)
                {
                    using var cmd = commandConnection.CreateCommand();
                    cmd.CommandText = "dbo.usp_pmui_UpsertBillingRate";
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Add parameters to the command
                    foreach (var param in parameters)
                    {
                        cmd.Parameters.Add(param);
                    }

                    if (cmd is SqlCommand sqlCmd)
                    {
                        try
                        {
                            // Execute the command and get the upserted data
                            var resultId = await sqlCmd.ExecuteScalarAsync(); // Assuming the procedure returns the ID
                            // Fetch the details of the newly inserted billing rate
                            using var reader = await sqlCmd.ExecuteReaderAsync();
                            if (reader.Read())
                            {
                                result = new BillingRateModel
                                {
                                    BillingRateId = Convert.ToInt32(resultId), 
                                    ContractNo = reader.GetString(reader.GetOrdinal("ContractId")),
                                    SiteId = reader.GetString(reader.GetOrdinal("SiteId")),
                                    CallTypeCd = reader.GetString(reader.GetOrdinal("CallTypeCd")),
                                    SecurusRatePerMinAmt = reader.GetDecimal(reader.GetOrdinal("SecurusRatePerMinAmt")),
                                    CommRatePerMinAmt = reader.GetDecimal(reader.GetOrdinal("CommRatePerMinAmt")),
                                    CommTypeId = reader.GetInt32(reader.GetOrdinal("CommTypeId")),
                                    AgencyRatePerMinAmt = reader.GetDecimal(reader.GetOrdinal("AgencyRatePerMinAmt")),
                                    IncludeAgencyRateInTotalFl = reader.GetBoolean(reader.GetOrdinal("IncludeAgencyRateInTotalFl")),
                                    StartDt = reader.GetDateTime(reader.GetOrdinal("StartDt")),
                                    EndDt = reader.GetDateTime(reader.GetOrdinal("EndDt")),                                    
                                    RoundingThresholdNr = reader.GetInt32(reader.GetOrdinal("RoundingThresholdNr")),
                                    AuditNoteTxt = reader.GetString(reader.GetOrdinal("AuditNoteTxt")),
                                    CreatedBy = reader.GetString(reader.GetOrdinal("CreatedBy")),
                                    CreateDt = reader.GetDateTime(reader.GetOrdinal("CreateDt")),
                                    ModifiedBy = reader.IsDBNull(reader.GetOrdinal("ModifiedBy")) ? null : reader.GetString(reader.GetOrdinal("ModifiedBy")),
                                    ModifiedDt = reader.GetDateTime(reader.GetOrdinal("ModifiedDt"))
                                };
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new ApplicationException("An error occurred while executing the database operation.", ex);
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                throw new ApplicationException("An error occurred while executing the database operation.", sqlEx);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred.", ex);
            }

            return result;
        }       

        public async Task<bool> AddSites(List<AddSiteVM> siteSearchVMs)
        {
            try
            {
                // Ensure the connection is a valid SqlConnection and open it
                if (_dbConnection is SqlConnection sqlConnection)
                {
                    await EnsureConnectionOpenAsync(sqlConnection);

                    foreach (var siteSearchVM in siteSearchVMs)
                    {
                        using var cmd = sqlConnection.CreateCommand();
                        cmd.CommandText = "dbo.usp_pmui_AddSites"; // Replace with your stored procedure name
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add parameters to the command
                        cmd.Parameters.Add(new SqlParameter("@BillingRateID", siteSearchVM.billingRateId ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@SiteiD", siteSearchVM.siteId ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@ContractiD", siteSearchVM.contractId ?? (object)DBNull.Value));


                        using var reader = await cmd.ExecuteReaderAsync();
                        while (await reader.ReadAsync())
                        {

                            var resultCode = reader.GetInt32(reader.GetOrdinal("ResultCode"));
                            if (resultCode != 1)
                            {
                                throw new Exception($"Failed to add site {siteSearchVM.siteId}.");
                            }
                        }
                    }
                }

                return true;
            }
            catch (SqlException sqlEx)
            {
                throw new ApplicationException("An error occurred while executing the database operation.", sqlEx);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred.", ex);
            }


        }

        public async Task<bool> UpdateSite(string siteId, string SiteName)
        {
            try
            {
                // Ensure the connection is a valid SqlConnection and open it
                if (_dbConnection is SqlConnection sqlConnection)
                {
                    await EnsureConnectionOpenAsync(sqlConnection);


                    using var cmd = sqlConnection.CreateCommand();
                    cmd.CommandText = "dbo.usp_pmui_UpdateSite";
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Add parameters to the command                        
                    cmd.Parameters.Add(new SqlParameter("@SiteId", siteId ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@SiteName", SiteName ?? (object)DBNull.Value));


                    using var reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {

                        var resultCode = reader.GetInt32(reader.GetOrdinal("ResultCode"));
                        if (resultCode != 1)
                        {
                            throw new Exception($"Failed to add site {siteId}.");
                        }
                    }
                }
                return true;

            }
            catch (SqlException sqlEx)
            {
                throw new ApplicationException("An error occurred while executing the database operation.", sqlEx);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred.", ex);
            }



        }


        public async Task<GetSiteDetailVM> GetSiteById(string siteId, string contractNo)
        {
            try
            {               
                if (_dbConnection is SqlConnection sqlConnection)
                {
                    await EnsureConnectionOpenAsync(sqlConnection);

                    using var cmd = sqlConnection.CreateCommand();
                    cmd.CommandText = "dbo.usp_pmui_GetSiteDetail";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@SiteId", siteId ?? (object)DBNull.Value));

                    using var reader = await cmd.ExecuteReaderAsync();
                   
                    GetSiteDetailVM siteDetail = null;

                    if (await reader.ReadAsync())
                    {
                        siteDetail = new GetSiteDetailVM
                        {
                            SiteId = reader.GetString(reader.GetOrdinal("siteId")),
                            SiteName = reader.GetString(reader.GetOrdinal("site_name")),
                            ContractNo = reader.GetString(reader.GetOrdinal("contract_no"))
                        };
                    }
                    
                    return siteDetail;
                }

                throw new InvalidOperationException("Invalid database connection.");
            }
            catch (SqlException sqlEx)
            {
                throw new ApplicationException("An error occurred while executing the database operation.", sqlEx);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred.", ex);
            }
        }


        public async Task<List<CommTypeModel>> GetAllCommType()
        {
            try
            {
                // Ensure the connection is a valid SqlConnection and open it
                if (_dbConnection is SqlConnection sqlConnection)
                {
                    await EnsureConnectionOpenAsync(sqlConnection);

                    using var cmd = sqlConnection.CreateCommand();
                    cmd.CommandText = "usp_pmui_GetCommissionTypes";
                    cmd.CommandType = CommandType.StoredProcedure;                   

                    using var reader = await cmd.ExecuteReaderAsync();

                    // Initialize the return model
                    var result = new List<CommTypeModel>();


                    while (await reader.ReadAsync())
                    {
                        var commType = new CommTypeModel
                        {                         
                            CommTypeId = reader.GetInt16(reader.GetOrdinal("CommTypeId")),
                            commTypeDs = reader.GetString(reader.GetOrdinal("CommTypeDs")),
                            //CreateDt = !reader.IsDBNull(reader.GetOrdinal("createDt")) ? reader.GetDateTime(reader.GetOrdinal("createDt")) : DateTime.UtcNow,
                            //CreatedBy = !reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? reader.GetString(reader.GetOrdinal("CreatedBy")) : null,
                            //ModifiedDt = !reader.IsDBNull(reader.GetOrdinal("ModifiedDt")) ? reader.GetDateTime(reader.GetOrdinal("ModifiedDt")) : DateTime.UtcNow,
                            //ModifiedBy = !reader.IsDBNull(reader.GetOrdinal("ModifiedBy")) ? reader.GetString(reader.GetOrdinal("ModifiedBy")) : null,
                           
                        };

                       result.Add(commType);

                    }

                    return result;
                }

                throw new InvalidOperationException("Invalid database connection.");
            }
            catch (SqlException sqlEx)
            {
                throw new ApplicationException("An error occurred while executing the database operation.", sqlEx);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred.", ex);
            }
        }
        

        public async Task<int> AddUserAsync(UserModel userModel)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserName", SqlDbType.NVarChar) { Value = userModel.UserName },
                new SqlParameter("@Email", SqlDbType.NVarChar) { Value = userModel.Email },
                new SqlParameter("@IsActive", SqlDbType.Bit) { Value = userModel.IsActive },
                new SqlParameter("@CreatedBy", SqlDbType.NVarChar) { Value = userModel.CreatedBy },
                new SqlParameter("@CreatedAt", SqlDbType.DateTime) { Value = DateTime.UtcNow },
                new SqlParameter("@ModifiedBy", SqlDbType.NVarChar) { Value = userModel.ModifiedBy ?? (object)DBNull.Value },
                new SqlParameter("@ModifiedAt", SqlDbType.DateTime) { Value = userModel.ModifiedAt ?? (object)DBNull.Value }
            };

            try
            {
                // Ensure SQL connection is open
                if (_dbConnection is SqlConnection sqlConnection)
                {
                    if (sqlConnection.State == ConnectionState.Closed)
                    {
                        await sqlConnection.OpenAsync();
                    }

                    // Create the command to execute the stored procedure
                    using (var cmd = sqlConnection.CreateCommand())
                    {
                        cmd.CommandText = "dbo.usp_AddUser"; // Stored procedure name
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add parameters to the command
                        cmd.Parameters.AddRange(parameters.ToArray());

                        // Execute the command and retrieve the new user ID
                        var result = await cmd.ExecuteScalarAsync(); // Returns the new UserId
                        if (result != DBNull.Value)
                        {
                            return Convert.ToInt32(result); // Return the new user ID
                        }
                        else
                        {
                            throw new ApplicationException("Failed to add user.");
                        }
                    }
                }
                else
                {
                    throw new InvalidOperationException("Invalid database connection.");
                }
            }
            catch (SqlException sqlEx)
            {
                // SQL specific exceptions
                throw new ApplicationException("An error occurred while executing the database operation.", sqlEx);
            }
            catch (Exception ex)
            {
                // Other general exceptions
                throw new ApplicationException("An unexpected error occurred while adding the user.", ex);
            }
        }


    }
}

