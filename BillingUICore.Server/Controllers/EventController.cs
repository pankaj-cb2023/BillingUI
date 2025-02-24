using BillingUI.Business.IService;
using BillingUI.Common.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BillingUICore.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly ILogger<EventController> _logger;
        public EventController(IEventService eventService, ILogger<EventController> logger)
        {
            _eventService = eventService;
            _logger = logger;

        }

        [HttpPost("getEventsHistory")]
        public async Task<IActionResult> GetAllEvents(EventLogRequestModel eventLogRequestModel)
        {
            var result = await _eventService.GetAllEvents(eventLogRequestModel);
            return result.HasError ? BadRequest(result) : Ok(result.Data);
        }


    }
}
