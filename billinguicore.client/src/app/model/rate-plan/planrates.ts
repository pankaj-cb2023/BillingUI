export class PlanRates {
  billingRateId: number;
  siteId: string;
  contractNo: string;
  state: string;
  siteName: string
  securusRatePerMinAmt: string;
  commRatePerMinAmt: string;
  agencyRatePerMinAmt: string;
  includeAgencyRateInTotalFl: number;
  commTypeDs: number;
  roundingThresholdNr: number;
  startDt: Date;
  endDt: Date;
  createdBy: string;
  createDt: Date;
  auditNoteTxt: string;
  callTypeCd: string;
  active: boolean;



  constructor(billingRateId: number, siteId: string, contractNo: string, state: string, siteName: string, securusRate: string, commRate: string, agencyRate: string, includeAgencyRateInTotalFl: number, commonType: number, roundingThreshold: number, startDate: Date, endDate: Date,
    auditUser: string, auditDate: Date, auditNoteTxt: string, callTypeCd: string, active: boolean) {
    this.billingRateId = billingRateId;
    this.siteId = siteId;
    this.contractNo = contractNo;
    this.state = state;
    this.siteName = siteName;
    this.securusRatePerMinAmt = securusRate;
    this.commRatePerMinAmt = commRate;
    this.agencyRatePerMinAmt = agencyRate;
    this.includeAgencyRateInTotalFl = includeAgencyRateInTotalFl;
    this.commTypeDs = commonType;
    this.startDt = startDate;
    this.endDt = endDate;
    this.roundingThresholdNr = roundingThreshold;
    this.createdBy = auditUser;
    this.createDt = auditDate;
    this.auditNoteTxt = auditNoteTxt;
    this.callTypeCd = callTypeCd;
    this.active = active;

  }
}

export class searchValue {
  siteId: string;
  contractNo: string;
  siteName: string;
  state: string;
  isContractSearch: boolean;
  isSearchBySiteIdOrName: boolean;
  isSiteClicked: boolean;

  constructor(siteId: string, contractNo: string, siteName: string, isContractSearch: boolean, isSearchBySiteIdOrName: boolean, isSiteClicked: boolean, state: string) {
    this.siteId = siteId;
    this.contractNo = contractNo;
    this.siteName = siteName;
    this.isContractSearch = isContractSearch;
    this.isSearchBySiteIdOrName = isSearchBySiteIdOrName;
    this.isSiteClicked = isSiteClicked;
    this.state = state
  }

  static createEmpty(): searchValue {
    return new searchValue('', '', '', false, false, false, '');
  }
}
