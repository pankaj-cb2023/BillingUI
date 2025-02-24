export class RatePlanDetails {
  SiteId: string;
  ContractNo: string;
  SiteName: string;
  ContactName: string;
  Status: boolean;

  constructor(SiteId: string, ContractNo: string, SiteName: string, ContactName: string, Status: boolean) {
    this.SiteId = SiteId;
    this.ContractNo = ContractNo;
    this.SiteName = SiteName;
    this.ContactName = ContactName;
    this.Status = Status;

  }
}
