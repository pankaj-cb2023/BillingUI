import { Component, EventEmitter, Input } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { VideoConnectRatesService } from '../../../services/video-connect-rates/video-connect-rates.service';
import { PlanRates, searchValue } from '../../../model/rate-plan/planrates';
import { Output } from '@angular/core';
import { debounceTime } from 'rxjs';
import { VideoConnectRateSharedService } from '../../../shared/video-connect-rate-shared-service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-searchfilter',
  standalone: true,
  templateUrl: './search-filter.component.html',
  styleUrls: ['./search-filter.component.css'],
  imports: [ReactiveFormsModule, CommonModule]
})
export class SearchFilterComponent {

  @Output() searchResults = new EventEmitter<{ planRateList: PlanRates[], dataTotalCount: number, plansSiteList: PlanRates[], siteTotalCount: number, isTrue: boolean, searchData: searchValue, gridErrorMessage: string }>();
  @Output() searchFormData = new EventEmitter<any>();
  @Input() selectedContractNo: string = '';
  @Input() selectedData: { type: string; value: string } | null = null;

  hasReadOnlyPermission: boolean = false;
  hasData: boolean = true;
  noDataMessage: string = '';
  currentPage: number = 1;
  recordsPerPage: number = 10;
  dataTotalCount: number = 0;
  siteTotalCount: number = 0;
  scurrentPage: number = 1;
  srecordsPerPage: number = 10;
  sortOrder: string = 'ASC';
  contractNoMismatch: boolean = false;
  siteIdMismatch: boolean = false;
  isSaveBtn: boolean = true;
  gridErrorMessage: string = '';
  searchContractNo: number = 0;
  searchSiteId: number = 0;
  searchSiteName: string = '';
  planRateList: PlanRates[] = [];
  plansSiteList: PlanRates[] = [];
  searchData!: searchValue;
  SearchRatePlanForm!: FormGroup;
  state: string = '';

  constructor(private router: Router, private videoSrv: VideoConnectRatesService, private searchRatePlan: FormBuilder, private sharedService: VideoConnectRateSharedService) { }

  ngOnInit(): void {
    this.sharedService.readOnlyPermission$.subscribe((status) => {
      this.hasReadOnlyPermission = status;
    });

    this.SearchRatePlanForm = this.searchRatePlan.group({
      siteId: ['', [Validators.pattern('^[0-9]*$')]],
      contractNo: [''],
      siteName: [''],
      state: [''],
      callTypeCd: ['VDR'],
      startDt: [null],
      pending: [1],
      historical: [0],
      active: [1]
    });    
    this.hasData = true;
  }

  validateNumber(event: KeyboardEvent) {
    const charCode = event.which ? event.which : event.keyCode;
    if (charCode < 48 || charCode > 57) {
      event.preventDefault();
    }
  }

  emitSearchFormData(): void {
    const contractNo = this.SearchRatePlanForm.get('contractNo')?.value || 0;
    const callTypeCd = this.SearchRatePlanForm.get('callTypeCd')?.value || '';
    this.searchFormData.emit({ contractNo, callTypeCd });
  }

  onSearch(updatedPageNumber?: number, updatedPageSize?: number, updatedSPageNumber?: number, updatedSPageSize?: number, outerContractNo?: string, outerSiteId?: string, isSiteClicked: boolean = false): void {
    this.noDataMessage = '';
    if (updatedPageNumber !== undefined) {
      this.currentPage = updatedPageNumber;
    }
    if (updatedPageSize !== undefined) {
      this.recordsPerPage = updatedPageSize;
    }

    if (updatedSPageNumber !== undefined) {
      this.scurrentPage = updatedSPageNumber;
    }
    if (updatedSPageSize !== undefined) {
      this.srecordsPerPage = updatedSPageSize;
    }
    if (this.SearchRatePlanForm.valid) {
      const formData = this.SearchRatePlanForm.value;
      formData.active = formData.active == '1' ? true : false;
      formData.pageNumber = this.currentPage;
      formData.pageSize = this.recordsPerPage;
      formData.sortColumn = "SiteId";
      formData.sortOrder = this.sortOrder;
      formData.spageNumber = this.scurrentPage;
      formData.spageSize = this.srecordsPerPage;
      formData.pending = formData.pending == '1' ? true : false;
      formData.historical = formData.historical == '1' ? true : false;
      if (formData.startDt == '') {
        formData.startDt = null;
      }
      if (outerContractNo != undefined) {
        formData.contractNo = outerContractNo;
        formData.siteId = '';
      }
      if (outerSiteId != undefined) {
        formData.siteId = outerSiteId;
      }

      this.videoSrv.searchPlan(formData).subscribe(
        (response: { data: PlanRates[], site: PlanRates[], totalCount: number, siteTotalCount: number }) => {
          if (response && Array.isArray(response.data) && Array.isArray(response.site)) {
            this.dataTotalCount = response.totalCount;
            this.siteTotalCount = response.siteTotalCount;

            this.planRateList = this.mapPlanRates(response.data);
            this.plansSiteList = this.mapPlanRates(response.site);
            //this.hasData = true; 
            // Determine if the search is based on contractNo, siteId, or siteName
            const isContractSearch = !!formData.contractNo;
            const searchBySiteIdOrName = !!formData.siteId || !!formData.siteName;

            this.searchData = this.extractSearchValues(formData, isSiteClicked);

            // Check if siteName or state is not null/empty
            this.emitSearchResults();
          }
        },
        (error) => {
          this.noDataMessage = 'An error occurred while fetching data';
          this.hasData = false;
        }
      );
    } else {
      this.noDataMessage = 'An error occurred while fetching data';
      this.hasData = false;
    }
  }

  private mapPlanRates(data: PlanRates[]): PlanRates[] {
    return data.map((item) =>
      new PlanRates(
        item.billingRateId || 0,
        item.siteId || "",
        item.contractNo || "",
        item.state || "",
        item.siteName || "",
        item.securusRatePerMinAmt || "",
        item.commRatePerMinAmt || "",
        item.agencyRatePerMinAmt || "",
        item.includeAgencyRateInTotalFl || 0,
        item.commTypeDs || 0,
        item.roundingThresholdNr || 0,
        item.startDt,
        item.endDt,
        item.createdBy || "",
        item.createDt,
        item.auditNoteTxt || "",
        item.callTypeCd || '',
        item.active
      )
    );
  }
  private extractSearchValues(formData: any, isSiteClicked: boolean): searchValue {
    const isContractSearch = !!formData.contractNo;
    const searchBySiteIdOrName = !!formData.siteId || !!formData.siteName;

    let updatedIsSiteClicked = isSiteClicked;
    if (formData.siteId) {
      updatedIsSiteClicked = true;
    } else if (formData.contractNo) {
      updatedIsSiteClicked = false;
    }

    return new searchValue(
      formData.siteId || '',
      formData.contractNo || 0,
      formData.siteName || '',
      isContractSearch,
      searchBySiteIdOrName,
      updatedIsSiteClicked,
      this.state
    );
  }
  private emitSearchResults(): void {
    const isTrue = !!(this.SearchRatePlanForm.value.siteName?.trim() || this.SearchRatePlanForm.value.state?.trim()) && this.plansSiteList.length > 1;

    if (this.planRateList.length > 0 || this.plansSiteList.length > 0) {
      this.searchResults.emit({
        planRateList: this.planRateList,
        dataTotalCount: this.dataTotalCount,
        plansSiteList: this.plansSiteList,
        siteTotalCount: this.siteTotalCount,
        isTrue: !!isTrue,
        searchData: this.searchData,
        gridErrorMessage: this.gridErrorMessage
      });
      this.hasData = true;
    } else {
      this.hasData = false;
      this.noDataMessage = 'No results were found';
      this.searchResults.emit({
        planRateList: [],
        dataTotalCount: 0,
        plansSiteList: [],
        siteTotalCount: 0,
        isTrue: false,
        searchData: this.searchData,
        gridErrorMessage: this.noDataMessage
      });
    }
  }


  onReset(): void {
    this.SearchRatePlanForm.patchValue({
      siteId: '',
      contractNo: '',
      siteName: '',
      state: ''
    });
    this.noDataMessage = '';
    this.hasData = true;
  }

}
