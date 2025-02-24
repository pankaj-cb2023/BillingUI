import { CommonModule } from '@angular/common';
import { Component, EventEmitter, input, Input, Output, SimpleChanges } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PlanRates, searchValue } from '../../../model/rate-plan/planrates';
import { VideoConnectRateSharedService } from '../../../shared/video-connect-rate-shared-service';

@Component({
  selector: 'app-siteresult',
  standalone: true,
  templateUrl: './site-result.component.html',
  styleUrl: './site-result.component.css',
  imports: [CommonModule,
    FormsModule, ReactiveFormsModule],
})
export class SiteResultComponent {

  scurrentPage: number = 1;
  srecordsPerPage: number = 10;
  contractNoForModal: string = '';
  hasReadOnlyPermission: boolean = false;

  @Input() plansSiteList: PlanRates[] = [];
  @Input() planRateList: PlanRates[] = [];
  @Input() searchData!: searchValue;
  @Input() siteTotalCount: number = 0;
  @Input() gridErrorMessage: string = '';
  @Output() paginationChanged = new EventEmitter<any>();
  @Output() editClicked = new EventEmitter<any>();
  @Output() contractSelected = new EventEmitter<string>();
  @Output() openModalEvent = new EventEmitter<void>();

  constructor(private sharedService: VideoConnectRateSharedService) { }

  ngOnInit() {
    this.sharedService.readOnlyPermission$.subscribe((status) => {
      this.hasReadOnlyPermission = status;
    });
  }

  openAddModal() {
    this.openModalEvent.emit();
  }


  onContractClick(event: Event) {
    event.preventDefault();
    if (this.searchData?.contractNo) {
      this.contractSelected.emit(this.searchData.contractNo);
    }
  }

  ngOnChanges(changes: SimpleChanges) {
    this.gridErrorMessage;
    if (changes['searchData'] || changes['plansSiteList']) {
      this.updateSearchData();
    }
  }

  private updateSearchData() {
    if (this.searchData && this.planRateList.length > 0) {
      const matchingSite = this.planRateList.find(site => site.siteId === this.searchData.siteId);

      if (matchingSite) {
        // Update siteName only if searchData.contractNo is 0
        if (this.searchData.contractNo == '0') {
          this.searchData.contractNo = matchingSite.contractNo;
          this.searchData.siteName = matchingSite.siteName;

        }
        // Otherwise, update siteName only if it's empty or undefined
        else if (!this.searchData.siteName) {
          this.searchData.siteName = matchingSite.siteName;
          this.searchData.state = matchingSite.state;
        }
      }
      else {
        if (this.planRateList.length > 0) {
          this.searchData.contractNo = this.planRateList[0].contractNo;
          this.searchData.isSearchBySiteIdOrName = false;
        }
      }
    }
  }


  // Now use these functions to set the values
  getDisplaySiteId(): string {
    if (this.searchData?.siteId) {
      return this.searchData.siteId;  // Return the siteId from searchData
    } else if (this.searchData?.siteName) {
      return this.getSiteIdFromSiteName(this.searchData.siteName) || 'N/A';
    }
    return 'N/A';
  }

  getDisplaySiteName(): string {
    if (this.searchData?.siteName) {
      return this.searchData.siteName;  // Return the siteName from searchData
    } else if (this.searchData?.siteId) {
      return this.getSiteNameFromSiteId(this.searchData.siteId) || 'N/A';
    }
    return 'N/A';
  }

  getSiteNameFromSiteId(siteId: string): string | undefined {
    const site = this.plansSiteList.find(plan => plan.siteId === siteId);
    return site ? site.siteName : undefined;
  }

  getSiteIdFromSiteName(siteName: string): string | undefined {
    const site = this.plansSiteList.find(plan => plan.siteName === siteName);
    return site ? site.siteId : undefined;
  }


  onEditClick(rate: any): void {
    this.editClicked.emit(rate);
  }

  getPageNumberssite(): (number | null)[] {
    const totalPages = Math.ceil(this.siteTotalCount / this.srecordsPerPage);
    const pageNumbers: (number | null)[] = [];
    const visiblePages = 3;

    for (let i = 1; i <= totalPages; i++) {
      if (
        i === 1 ||
        i === totalPages ||
        (i >= this.scurrentPage - visiblePages && i <= this.scurrentPage + visiblePages)
      ) {
        pageNumbers.push(i);
      } else if (pageNumbers[pageNumbers.length - 1] !== null) {
        pageNumbers.push(null);
      }
    }
    return pageNumbers;
  }

  changeRecordsPerPagesite(event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    this.srecordsPerPage = +selectElement.value;

    this.scurrentPage = 1;
    this.updatePaginatedListsite(this.scurrentPage);
  }

  prevPagesite(): void {
    if (this.scurrentPage > 1) {
      this.updatePaginatedListsite(this.scurrentPage - 1);
    }
  }

  nextPagesite(): void {
    if (this.scurrentPage < Math.ceil(this.siteTotalCount / this.srecordsPerPage)) {
      this.updatePaginatedListsite(this.scurrentPage + 1);
    }
  }

  updatePaginatedListsite(page: number): void {
    if (page < 1 || page > Math.ceil(this.siteTotalCount / this.srecordsPerPage)) {
      return;
    }
    this.scurrentPage = page;
    this.paginationChanged.emit({
      scurrentPage: this.scurrentPage,
      srecordsPerPage: this.srecordsPerPage,
    });
  }

}
