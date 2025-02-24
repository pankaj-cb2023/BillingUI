export class Sites {
  contractNo: string;
  siteId: string;
  siteName: string;
  state: string;
  isContractSearch;
  isSearchBySiteIdOrName;
  isSiteClicked;



  constructor(contractNo: string, siteId: string, siteName: string, state: string, isContractSearch: boolean, isSearchBySiteIdOrName: boolean, isSiteClicked: boolean) {
    this.contractNo = contractNo;
    this.siteId = siteId;   
    this.siteName = siteName;
    this.state = state;
    this.isContractSearch = isContractSearch;
    this.isSearchBySiteIdOrName = isSearchBySiteIdOrName;
    this.isSiteClicked = isSiteClicked;


  }
}
