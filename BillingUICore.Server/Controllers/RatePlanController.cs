using BillingUI.Business.IService;
using BillingUI.Common.Infrastructure;
using BillingUI.Common.Model;
using BillingUICore.Server.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace BillingUICore.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[RequiresPermission("ViewRatePlan")]
    public class RatePlanController : ControllerBase
    {
        private readonly IRatePlanService _ratePlanService;
        private readonly IEventService _eventService;
        private readonly ILogger<RatePlanController> _logger;

        public RatePlanController(IRatePlanService ratePlanService, ILogger<RatePlanController> logger, IEventService eventService)
        {
            _ratePlanService = ratePlanService;
            _eventService = eventService;
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="billingRateSearch"></param>
        /// <returns></returns>

        [HttpPost("searchPlan")]
        public async Task<IActionResult> SearchPlan(BillingRateSearchModel billingRateSearch)
        {

            if (billingRateSearch.PageNumber < 1 || billingRateSearch.PageSize < 1)
            {
                return BadRequest("Invalid pagination parameters.");
            }
            var result = await _ratePlanService.Search(billingRateSearch);

            if (result.HasError)
            {
                return BadRequest(result);
            }
            return result.HasError ? BadRequest(result.Data) : Ok(result.Data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteSearchVM"></param>
        /// <returns></returns>
        [HttpPost("searchSite")]
        public async Task<IActionResult> SearchSite(BillingRateSearchModel siteSearchVM)
        {
            if (siteSearchVM.PageNumber < 1 || siteSearchVM.PageSize < 1)
            {
                return BadRequest("Invalid pagination parameters.");
            }

            var result = await _ratePlanService.GetSitesDataAsync(siteSearchVM);
            return result.HasError ? BadRequest(result) : Ok(result.Data);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="billingRateUpdateModel"></param>
        /// <returns></returns>
        [HttpPut("updateBillingRate")]
        public async Task<IActionResult> UpdateBillingRate([FromBody] BillingRateUpdateModel billingRateUpdateModel)
        {
            var result = await _ratePlanService.UpdateBillingRate(billingRateUpdateModel);
            if (result.Data != null)
            {
                await _eventService.CreateEvent(result.Data, EventTypes.EventTypeUpdate);
            }
            return result.HasError ? BadRequest(result) : Ok(result);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="billingRateAddModel"></param>
        /// <returns></returns>
        [HttpPost("addBillingRate")]
        public async Task<IActionResult> AddBillingRate(BillingRateAddModel billingRateAddModel)
        {
            var result = await _ratePlanService.AddBillingRate(billingRateAddModel);
            if (result != null)
            {
                //await _eventService.CreateEvent(result.Data);
            }
            return result.HasError ? BadRequest(result) : Ok(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteSearchVM"></param>
        /// <returns></returns>
        [HttpPost("addSite")]
        public async Task<IActionResult> AddSite(List<AddSiteVM> siteSearchVM)
        {
            var result = await _ratePlanService.AddSites(siteSearchVM);
            return result.HasError ? BadRequest(result) : Ok(result.Data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="siteName"></param>
        /// <returns></returns>
        [HttpPut("updateSite")]
        public async Task<IActionResult> UpdateSite(string siteId, string siteName)
        {
            var result = await _ratePlanService.UpdateSite(siteId, siteName);
            return result.HasError ? BadRequest(result) : Ok(result.Data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="contractNo"></param>
        /// <returns></returns>
        [HttpGet("getSiteById")]
        public async Task<IActionResult> GetSiteById(string siteId, string contractNo)
        {
            var result = await _ratePlanService.GetSiteById(siteId, contractNo);
            return result.HasError ? BadRequest(result) : Ok(result.Data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("getCommType")]
        public async Task<IActionResult> GetALLCommType()
        {
            var result = await _ratePlanService.GetAllCommType();
            return result.HasError ? BadRequest(result) : Ok(result.Data);
        }

    }
}
