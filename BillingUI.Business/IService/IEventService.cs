using BillingUI.Common.Infrastructure;
using BillingUI.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingUI.Business.IService
{
    public interface IEventService
    {
        Task<ServiceResult<EventLogResponseModel>> CreateEvent(BillingRateModel billingRateModel, EventTypes eventTypes);
        Task<ServiceResult<List<EventLogResponseModel>>> GetAllEvents(EventLogRequestModel eventLogRequestModel);
    }
}
