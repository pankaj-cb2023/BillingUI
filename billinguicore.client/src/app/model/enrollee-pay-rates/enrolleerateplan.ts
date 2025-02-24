export class EnrolleeRatePlan {
  RatePlanId: number;
  Description: string;
  IsActive: boolean;

  constructor(RatePlanId: number, Description: string, IsActive: boolean) {
    this.RatePlanId = RatePlanId;
    this.Description = Description;
    this.IsActive = IsActive;
  }
}
