import { Component, EventEmitter, Input, Output } from '@angular/core';
import { PlanRates } from '../../../model/rate-plan/planrates';
import { RouterLink, RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Sites } from '../../../model/site/sites';
import { VideoConnectRatesService } from '../../../services/video-connect-rates/video-connect-rates.service';

@Component({
  selector: 'app-agencyresult',
  standalone: true,
  templateUrl: './agency-result.component.html',
  styleUrl: './agency-result.component.css',
  imports: [
    RouterOutlet,
    RouterLink,
    CommonModule,
    FormsModule,
    ReactiveFormsModule
  ]
})
export class AgencyResultComponent {

  currentPage: number = 1;
  recordsPerPage: number = 10;

  @Input() planRateList: PlanRates[] = [];
  @Input() dataTotalCount: number = 0;
  @Output() paginationChanged = new EventEmitter<any>();
  @Output() editClicked = new EventEmitter<any>();
  @Input() gridErrorMessage: string = '';
  @Output() siteDataFetched = new EventEmitter<Sites>();
  constructor(private videoSrv: VideoConnectRatesService) {
  }

  ngOnInit() {
  }

  onEditClick(rate: any): void {
    this.editClicked.emit(rate);
  }


  getSiteData(siteId: string, siteName: string, contractNo: string, state: string) {
    const siteData: Sites = {
      siteId: siteId,
      siteName: siteName,
      contractNo: contractNo,
      state: state,
      isSearchBySiteIdOrName: true,
      isContractSearch: false,
      isSiteClicked: true

    };
    this.siteDataFetched.emit(siteData);
  }

  getPageNumbers(): (number | null)[] {
    const totalPages = Math.ceil(this.dataTotalCount / this.recordsPerPage);
    const pageNumbers: (number | null)[] = [];
    const visiblePages = 3;

    for (let i = 1; i <= totalPages; i++) {
      if (
        i === 1 ||
        i === totalPages ||
        (i >= this.currentPage - visiblePages && i <= this.currentPage + visiblePages)
      ) {
        pageNumbers.push(i);
      } else if (pageNumbers[pageNumbers.length - 1] !== null) {
        pageNumbers.push(null);
      }
    }

    return pageNumbers;
  }

  changeRecordsPerPage(event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    this.recordsPerPage = +selectElement.value;

    // Reset to the first page after changing the records per page
    this.currentPage = 1;
    this.updatePaginatedList(this.currentPage);
  }

  prevPage(): void {
    if (this.currentPage > 1) {
      this.updatePaginatedList(this.currentPage - 1);
    }
  }

  nextPage(): void {
    if (this.currentPage < Math.ceil(this.dataTotalCount / this.recordsPerPage)) {
      this.updatePaginatedList(this.currentPage + 1);
    }
  }

  updatePaginatedList(page: number): void {
    if (page < 1 || page > Math.ceil(this.dataTotalCount / this.recordsPerPage)) {
      return;
    }
    this.currentPage = page;
    this.paginationChanged.emit({
      currentPage: this.currentPage,
      recordsPerPage: this.recordsPerPage,
    });
  }


  //#region "grid coloring start"

  isFutureDate(startDt: Date): boolean {
    const currentDate = new Date();
    const startDate = new Date(startDt);
    return startDate > currentDate;
  }

  isFutureDateDeactive(startDt: Date, active: boolean): boolean {
    const currentDate = new Date();
    const startDate = new Date(startDt);
    return active == false && startDate > currentDate;
  }

  isPastDate(endDt: Date): boolean {
    const today = new Date();
    const endtDate = new Date(endDt);
    return endtDate < today;
  }

  isPastDateDeactive(endDt: Date, active: boolean): boolean {
    const today = new Date();
    const endtDate = new Date(endDt);
    return active == false && endtDate < today;
  }

  isPresentDate(date: string | Date): boolean {
    const today = new Date();
    const inputDate = new Date(date);
    return inputDate.toDateString() === today.toDateString();
  }

  //#endregion "grid coloring end"


}
