using BillingUI.Common.Infrastructure;
using BillingUI.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingUI.Data.IRepository
{
    public interface IEventRepository
    {
        Task<List<EventSource>> GetAllEventSourcesAsync();
        Task<List<EventType>> GetAllEventTypeAsync();
        Task<EventLogResponseModel> CreateEventAsync(EventLog eventLog);
        Task<List<EventLogResponseModel>> GetAllEventAsync(EventLogRequestModel eventLogRequestModel);
    }
}
