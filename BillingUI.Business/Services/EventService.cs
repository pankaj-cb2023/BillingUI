using BillingUI.Business.IService;
using BillingUI.Common.Infrastructure;
using BillingUI.Common.Model;
using BillingUI.Data.Entites;
using BillingUI.Data.IRepository;
using BillingUI.Data.Repository;
using System.Text.Json;
using EventLog = BillingUI.Common.Model.EventLog;



namespace BillingUI.Business.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;

        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<ServiceResult<EventLogResponseModel>> CreateEvent(BillingRateModel billingRateModel, EventTypes eventTypes)
        {
            var serviceResult = new ServiceResult<EventLogResponseModel>();
            EventLog eventLog = new();

            try
            {
                var eventSrc = await _eventRepository.GetAllEventSourcesAsync();
                var eventType = await _eventRepository.GetAllEventTypeAsync();

                serviceResult.SetError(eventSrc == null ? "Error to fetch the event source!" :
                          eventType == null ? "Error to fetch the event type!" : null);

                eventLog.SourceCd = eventSrc.FirstOrDefault(i => i.EventSourceCd == "PMUI_RATES")?.EventSourceCd ?? null;
                eventLog.TypeCd = eventType.FirstOrDefault(i => i.EventTypeCd.Trim() == ((char)eventTypes).ToString())?.EventTypeCd.Trim();
                eventLog.LogUser = @"CORP\A0016645";
                eventLog.LogDt = DateTime.Now;
                eventLog.RecordID = billingRateModel.BillingRateId;
                eventLog.EventDetails = JsonSerializer.Serialize(billingRateModel);

                var data = await _eventRepository.CreateEventAsync(eventLog);

                if (data != null)
                {
                    serviceResult.SetSuccess(data, "Event created successfully!");
                }
                else
                {
                    serviceResult.SetError("Error adding the event!");
                }
            }
            catch (Exception ex)
            {
                // Log exception (logging can be implemented here)
                serviceResult.SetError($"An unexpected error occurred: {ex.Message}");
            }

            return serviceResult;
        }


        public async Task<ServiceResult<List<EventLogResponseModel>>> GetAllEvents(EventLogRequestModel eventLogRequestModel)
        {
            var serviceResult = new ServiceResult<List<EventLogResponseModel>>();

            var data = await _eventRepository.GetAllEventAsync(eventLogRequestModel);
            if (data != null)
            {
                serviceResult.SetSuccess(data, "Get event history successfully!");
            }
            else
            {
                serviceResult.SetError("Error in fetching the event data!");
            }

            return serviceResult;

        }

    }
}
