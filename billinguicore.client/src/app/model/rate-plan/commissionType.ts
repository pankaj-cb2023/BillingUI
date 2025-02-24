export class CommissionType {
  commTypeId: number;
  commTypeDs: string;
  CreateDt: string;
  CreatedBy: string;
  ModifiedDt: Date;
  ModifiedBy:string

  constructor(commTypeId: number, commTypeDs: string, CreateDt: string, CreatedBy: string, ModifiedDt: Date, ModifiedBy: string) {
    this.commTypeId = commTypeId;
    this.commTypeDs = commTypeDs;
    this.CreateDt = CreateDt;
    this.CreatedBy = CreatedBy;
    this.ModifiedDt = ModifiedDt;
    this.ModifiedBy = ModifiedBy;
  }
}
