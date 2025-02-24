import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";


@Injectable({
  providedIn: 'root',
})
export class VideoConnectRateSharedService {

    private selectedOptionSource = new BehaviorSubject<string>(''); // Default empty
  selectedOption$ = this.selectedOptionSource.asObservable();

  setSelectedOption(option: string) {
    this.selectedOptionSource.next(option);
  }


  private readOnlyPermissionSubject = new BehaviorSubject<boolean>(false);
  readOnlyPermission$ = this.readOnlyPermissionSubject.asObservable();

  setReadOnlyPermission(status: boolean) {
    this.readOnlyPermissionSubject.next(status);
  }


  
}
