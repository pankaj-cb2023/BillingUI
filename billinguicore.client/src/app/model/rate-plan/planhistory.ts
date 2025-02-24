export class PlanHistory {
  constructor(
    public logDt: Date,
    public eventType: string,
    public logUser: string,
    public notes: string,
    public eventDetails: string
  ) { }
}



export class EventLogRequest {
  EventSource: string = "PMUI_RATES"; 
  EventType: string | null = null;;
  RecordID: number = 0;
  AltRecordID?: number | null = null;

  ShowUpdates: boolean = true; 
  ShowLocks: boolean = false;
  PageSize: number = 10; 
  CurrentPage: number = 1; 

  StartEventDate?: Date | null = null;
  EndEventDate?: Date | null = null;
  TotalRows: number = 0; 
  TotalPages: number = 0; 
}
