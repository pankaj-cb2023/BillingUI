using BillingUI.Business.IService;
using BillingUI.Common.Infrastructure;
using BillingUI.Common.Model;
using BillingUI.Data.Entites;
using BillingUI.Data.IRepository;


namespace BillingUI.Business.Services
{
    public class RatePlanService : IRatePlanService
    {
        private readonly IRatePlanRepository _ratePlanRepository;

        public RatePlanService(IRatePlanRepository ratePlanRepository)
        {
            _ratePlanRepository = ratePlanRepository;
        }

        public async Task<ServiceResult<BillingRateReponseModel>> Search(BillingRateSearchModel billingRateSearch)
        {
            var serviceResult = new ServiceResult<BillingRateReponseModel>();
            if (!string.IsNullOrEmpty(billingRateSearch.SiteId) || !string.IsNullOrEmpty(billingRateSearch.ContractNo)) {
                billingRateSearch.SiteName = null;
                billingRateSearch.State = null;  
            }       
            var site = await _ratePlanRepository.SearchSite(billingRateSearch);
            var data = await _ratePlanRepository.Search(billingRateSearch);

            if (site != null && site.Data != null || data != null && data.Data != null)
            {
                var result = new BillingRateReponseModel
                {
                    Data = data.Data,
                    Site = site.Data,
                    TotalCount = data.TotalCount,
                    SiteTotalCount = site.TotalCount,
                };
                serviceResult.SetSuccess(result, "Get plan Successfully!");
            }
            else
            {               
                serviceResult.SetError("Error to get plan! Site or Data is null.");
            }
            return serviceResult;
        }


        public async Task<ServiceResult<BillingRateSite>> GetSitesDataAsync(BillingRateSearchModel siteSearchVM)
        {
            var serviceResult = new ServiceResult<BillingRateSite>();
            var data = await _ratePlanRepository.GetSitesDataAsync(siteSearchVM);
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


        public async Task<ServiceResult<BillingRateModel>> UpdateBillingRate(BillingRateUpdateModel billingRateUpdateModel)
        {
            var serviceResult = new ServiceResult<BillingRateModel>();
            var data = await _ratePlanRepository.UpdateBillingRate(billingRateUpdateModel);
            if (data != null)
            {
                serviceResult.SetSuccess(data, "Updated plan Successfully!");
            }
            else
            {
                serviceResult.SetError("Error to update plan!");
            }
            return serviceResult;
        }

        public async Task<ServiceResult<BillingRateModel>> AddBillingRate(BillingRateAddModel billingRateAddModel)
        {
            var serviceResult = new ServiceResult<BillingRateModel>();
            var data = await _ratePlanRepository.AddBillingRate(billingRateAddModel);
            if (data != null)
            {
                serviceResult.SetSuccess(data, "Paln added Successfully!");
            }
            else
            {
                serviceResult.SetError("Error to add plan!");
            }
            return serviceResult;
        }
  

        public async Task<ServiceResult<bool>> AddSites(List<AddSiteVM> siteSearchVM)
        {
            var serviceResult = new ServiceResult<bool>();
            var data = await _ratePlanRepository.AddSites(siteSearchVM);
            if (data)
            {
                serviceResult.SetSuccess(data, "Get site Successfully!");
            }
            else
            {
                serviceResult.SetError("Error to get site!");
            }
            return serviceResult;
        }

        public async Task<ServiceResult<bool>> UpdateSite(string siteId, string SiteName)
        {
            var serviceResult = new ServiceResult<bool>();
            var data = await _ratePlanRepository.UpdateSite(siteId, SiteName);
            if (data)
            {
                serviceResult.SetSuccess(data, "Update site Successfully!");
            }
            else
            {
                serviceResult.SetError("Error to update site!");
            }
            return serviceResult;
        }

        public async Task<ServiceResult<GetSiteDetailVM>> GetSiteById(string siteId, string contractNo)
        {
            var serviceResult = new ServiceResult<GetSiteDetailVM>();
            var data = await _ratePlanRepository.GetSiteById(siteId, contractNo);
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

        public async Task<ServiceResult<List<CommTypeModel>>> GetAllCommType()
        {
            var serviceResult = new ServiceResult<List<CommTypeModel>>();
            var data = await _ratePlanRepository.GetAllCommType();
            if (data != null)
            {
                serviceResult.SetSuccess(data, "Get commtype Successfully!");
            }
            else
            {
                serviceResult.SetError("Error to get comm type!");
            }
            return serviceResult;
        }
    }
}
