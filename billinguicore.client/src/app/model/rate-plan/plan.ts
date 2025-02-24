export class Plan {
  billingRateId: number;
  siteId: string;
  siteName: string; 
  securusRate: string;
  agencyRate: string;
  commonType: number;
  roundingThresholdNr: string;
  startDate: Date;
  endDate: Date;


  constructor(billingRateId: number, siteId: string, siteName: string, securusRate: string, agencyRate: string, commonType: number, roundingThresholdNr: string, startDate: Date, endDate: Date) {
    this.billingRateId = billingRateId;
    this.siteId = siteId;
    this.siteName = siteName;   
    this.securusRate = securusRate;
    this.agencyRate = agencyRate;
    this.commonType = commonType;
    this.roundingThresholdNr = roundingThresholdNr;
    this.startDate = startDate;
    this.endDate = endDate;

  }
}


export class AddSite {
  billingRateId: string;
  siteId: string
  contractId: string;

  constructor(billingRateId: string, siteId: string, contractId: string) {
    this.billingRateId = billingRateId;
    this.siteId = siteId;
    this.contractId = contractId;
  }

}




