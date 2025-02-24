export class Fees {
  Accounttype: string;
  Applicationtype: string;
  Paymenttype: string;
  Amount: number;
  MinAmount: number;
  MaxAmount: number;
  CardProcessingFees: string;

  constructor(
    Accounttype: string,
    Applicationtype: string,
    Paymenttype: string,
    Amount: number,
    MinAmount: number,
    MaxAmount: number,
    CardProcessingFees: string,

  ) {
    this.Accounttype = Accounttype;
    this.Applicationtype = Applicationtype;
    this.Paymenttype = Paymenttype;
    this.Amount = Amount;
    this.MinAmount = MinAmount;
    this.MaxAmount = MaxAmount;
    this.CardProcessingFees = CardProcessingFees;
  }
}
