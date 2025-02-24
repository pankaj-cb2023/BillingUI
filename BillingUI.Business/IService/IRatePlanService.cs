using BillingUI.Common.Model;
using BillingUI.Data.Entites;
using BillingUI.Common.Infrastructure;

namespace BillingUI.Business.IService
{
    public interface IRatePlanService
    {
        Task<ServiceResult<BillingRateReponseModel>> Search(BillingRateSearchModel billingRateSearchModel);
        Task<ServiceResult<BillingRateSite>> GetSitesDataAsync(BillingRateSearchModel siteSearchVM);
        Task<ServiceResult<BillingRateModel>> UpdateBillingRate(BillingRateUpdateModel billingRateUpdateModel);
        Task<ServiceResult<BillingRateModel>> AddBillingRate(BillingRateAddModel  billingRateAddModel);
     
        Task<ServiceResult<bool>> AddSites(List<AddSiteVM> siteSearchVM);
        Task<ServiceResult<bool>> UpdateSite(string siteId, string SiteName);
        Task<ServiceResult<GetSiteDetailVM>> GetSiteById(string siteId, string contractNo);
        Task<ServiceResult<List<CommTypeModel>>> GetAllCommType();


    }
}
