export class RateTextConnect {
  ratePlanId: number;
  description: string;
  contractNo: string;
  contractName: string;
  defaultSiteId: string;
  id: number;
  reloadBalance: number;
  maxPerPeriod: number;
  expireDays: number;
  displayName: string;
  displayDescription: string;
  rate: number;
  agencyCharge: number;
  startDate: Date;


  constructor(ratePlanId: number, description: string, contractNo: string, contractName: string, defaultSiteId: string, id: number, reloadBalance: number, maxPerPeriod: number, expireDays: number, displayName: string, displayDescription: string, rate: number, agencyCharge: number, startDate: Date,) {
    this.ratePlanId = ratePlanId,
      this.description = description;
    this.contractNo = contractNo;
    this.contractName = contractName;
    this.defaultSiteId = defaultSiteId;
    this.id = id;
    this.reloadBalance = reloadBalance;
    this.maxPerPeriod = maxPerPeriod;
    this.expireDays = expireDays;
    this.displayName = displayName;
    this.displayDescription = displayDescription;
    this.rate = rate;
    this.agencyCharge = agencyCharge;
    this.startDate = startDate;
  }

}

export class TextConnectHistory {
  eventDate: Date;
  eventType: string;
  user: string;
  notes: string;
  eventDetails: string;

  constructor(eventDate: Date, eventType: string, user: string, notes: string, eventDetails: string) {

    this.eventDate = eventDate;
    this.eventType = eventType;
    this.user = user;
    this.notes = notes;
    this.eventDetails=eventDetails

  }
}
