export class PlanSite {
  siteId: string;
  contractNo: string;
  siteName: string;
  state: string;

  constructor(SiteId: string, ContractNo: string, SiteName: string, State: string) {
    this.siteId = SiteId;
    this.contractNo = ContractNo;
    this.siteName = SiteName;
    this.state = State;

  }
}
