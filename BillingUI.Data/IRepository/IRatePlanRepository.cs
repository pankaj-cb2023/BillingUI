using BillingUI.Common.Model;

namespace BillingUI.Data.IRepository
{
    public interface IRatePlanRepository
    {
        Task<BillingRateSite> Search(BillingRateSearchModel billingRateSearch);
        Task<BillingRateSite> SearchSite(BillingRateSearchModel billingRateSearch);
        Task<BillingRateSite> GetSitesDataAsync(BillingRateSearchModel siteSearchVM);
        Task<BillingRateModel> UpdateBillingRate(BillingRateUpdateModel billingRateUpdateModel);
        Task<BillingRateModel> AddBillingRate(BillingRateAddModel billingRateAddModel);      
        Task<bool> AddSites(List<AddSiteVM> siteSearchVM);
        Task<bool> UpdateSite(string siteId, string SiteName);
        Task<GetSiteDetailVM> GetSiteById(string siteId, string contractNo);
        Task<List<CommTypeModel>> GetAllCommType();
    }
}
