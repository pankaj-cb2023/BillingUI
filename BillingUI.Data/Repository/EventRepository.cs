using BillingUI.Common.Infrastructure;
using BillingUI.Common.Model;
using BillingUI.Data.IRepository;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingUI.Data.Repository
{


    public class EventRepository : IEventRepository
    {
        private readonly IDbConnection _dbConnection;
        public EventRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;

        }
        public async Task EnsureConnectionOpenAsync(SqlConnection sqlConnection)
        {
            if (sqlConnection.State == ConnectionState.Closed)
            {
                await sqlConnection.OpenAsync();
            }
        }

        public async Task<List<EventSource>> GetAllEventSourcesAsync()
        {
            var result = new List<EventSource>();

            try
            {
                // Ensure the connection is a valid SqlConnection and open it
                if (_dbConnection is SqlConnection sqlConnection)
                {
                    await EnsureConnectionOpenAsync(sqlConnection);

                    using var cmd = sqlConnection.CreateCommand();
                    cmd.CommandText = "usp_pmui_GetAllEventSources";

                    using var reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {

                        var eventSource = new EventSource
                        {
                            EventSourceID = reader.GetInt16(reader.GetOrdinal("EventSourceID")),
                            EventSourceCd = reader.GetString(reader.GetOrdinal("EventSourceCd")),
                            EventSourceDesc = reader.GetString(reader.GetOrdinal("EventSourceDesc"))
                        };
                        result.Add(eventSource);
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


        public async Task<List<EventType>> GetAllEventTypeAsync()
        {
            var result = new List<EventType>();

            try
            {
                if (_dbConnection is SqlConnection sqlConnection)
                {
                    await EnsureConnectionOpenAsync(sqlConnection);

                    using var cmd = sqlConnection.CreateCommand();
                    cmd.CommandText = "usp_pmui_GetAllEventTypes";

                    using var reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        var eventSource = new EventType
                        {
                            EventTypeID = reader.GetInt32(reader.GetOrdinal("EventTypeID")),
                            EventTypeCd = reader.GetString(reader.GetOrdinal("EventTypeCd")),
                            EventTypeDesc = reader.GetString(reader.GetOrdinal("EventTypeDesc"))
                        };
                        result.Add(eventSource);
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

        public async Task<EventLogResponseModel> CreateEventAsync(EventLog eventLogs)
        {

            // Prepare parameters to call the stored procedure
            var parameters = new List<SqlParameter>
                {
                    new("@SourceCd", eventLogs.SourceCd),
                    new("@TypeCd", eventLogs.TypeCd),
                    new("@RecordID", eventLogs.RecordID),
                    new("@Date", eventLogs.LogDt),
                    new("@User", eventLogs.LogUser),
                    new("@Details", eventLogs.EventDetails ?? null),
                    new("@Notes", eventLogs.Notes ?? null),
                    new("@AltRecordID", eventLogs.AltRecordId ?? 0)
                };
            EventLogResponseModel eventLog = new();


            try
            {
                if (_dbConnection is SqlConnection sqlConnection)
                {
                    await EnsureConnectionOpenAsync(sqlConnection);
                }

                if (_dbConnection is SqlConnection commandConnection)
                {
                    using var cmd = commandConnection.CreateCommand();
                    cmd.CommandText = "dbo.usp_pmui_CreateEvent";
                    cmd.CommandType = CommandType.StoredProcedure;

                    foreach (var param in parameters)
                    {
                        cmd.Parameters.Add(param);
                    }
                    if (cmd is SqlCommand sqlCmd)
                    {
                        try
                        {

                            //var  resultId = (int)await sqlCmd.ExecuteScalarAsync();
                            using var reader = await sqlCmd.ExecuteReaderAsync();
                            if (reader.Read())
                            {
                                return new EventLogResponseModel
                                {
                                    EventSourceID = reader.GetInt16(reader.GetOrdinal("EventSourceID")),
                                    EventTypeID = reader.GetInt32(reader.GetOrdinal("EventTypeID")),
                                    LogUser = reader.GetString(reader.GetOrdinal("LogUser")),
                                    LogDt = reader.GetDateTime(reader.GetOrdinal("LogDt")),
                                    RecordID = reader.GetInt64(reader.GetOrdinal("RecordID")),
                                    AltRecordId = reader.IsDBNull(reader.GetOrdinal("AltRecordId")) ? (long?)null : reader.GetInt64(reader.GetOrdinal("AltRecordId")),
                                    EventDetails = reader.IsDBNull(reader.GetOrdinal("EventDetails")) ? null : reader.GetString(reader.GetOrdinal("EventDetails")),
                                    Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes"))
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

            return eventLog;
        }

        public async Task<List<EventLogResponseModel>> GetAllEventAsync(EventLogRequestModel eventLogRequestModel)
        {
            var result = new List<EventLogResponseModel>();

            var parameters = new List<SqlParameter>
            {
                    new("@SourceCd", eventLogRequestModel.EventSource),
                    new("@RecordID", eventLogRequestModel.RecordID),
                    new("@AltRecordID", eventLogRequestModel.AltRecordID),
                    new("@TypeCd", eventLogRequestModel.EventType),
                    new("@ShowLocks", eventLogRequestModel.ShowLocks),
                    new("@ShowUpdates", eventLogRequestModel.ShowUpdates),
                    new("@StartDate", eventLogRequestModel.StartEventDate),
                    new("@EndDate", eventLogRequestModel.EndEventDate),                
                    new("@PageSize", eventLogRequestModel.PageSize),
                    new("@CurrentPage", eventLogRequestModel.CurrentPage),
                    new("@TotalRows", eventLogRequestModel.TotalRows),
                    new("@TotalPages", eventLogRequestModel.TotalPages)
            };

            EventLogRequestModel eventLog = new();
            try
            {
                if (_dbConnection is SqlConnection sqlConnection)
                {
                    await EnsureConnectionOpenAsync(sqlConnection);
                }

                if (_dbConnection is SqlConnection commandConnection)
                {
                    using var cmd = commandConnection.CreateCommand();
                    cmd.CommandText = "dbo.usp_pmui_GetPagedEvents";
                    cmd.CommandType = CommandType.StoredProcedure;

                    foreach (var param in parameters)
                    {
                        cmd.Parameters.Add(param);
                    }
                    if (cmd is SqlCommand sqlCmd)
                    {
                        try
                        {
                            using var reader = await cmd.ExecuteReaderAsync();
                            while (await reader.ReadAsync())
                            {
                                // Map the data from the reader to the Site object
                                var reponse = new EventLogResponseModel
                                {
                                    LogDt = reader.GetDateTime(reader.GetOrdinal("LogDt")),
                                    EventType = reader.GetString(reader.GetOrdinal("EventTypeCd")),
                                    LogUser = reader.GetString(reader.GetOrdinal("LogUser")),
                                    Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? string.Empty: reader.GetString(reader.GetOrdinal("Notes")),
                                    EventDetails = reader.GetString(reader.GetOrdinal("EventDetails"))
                                };

                                result.Add(reponse);
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

    }
}
