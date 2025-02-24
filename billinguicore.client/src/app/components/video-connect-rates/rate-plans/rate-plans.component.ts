import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import { VideoConnectTabsComponent } from '../video-connect-tabs/video-connect-tabs.component';
import { CommonModule } from '@angular/common';
import { DataTablesModule } from 'angular-datatables';
import { VideoConnectRatesService } from '../../../services/video-connect-rates/video-connect-rates.service';
import { PlanRates, searchValue } from '../../../model/rate-plan/planrates';
import { AgencyResultComponent } from '../agency-result/agency-result.component';
import { SearchFilterComponent } from '../search-filter/search-filter.component';
import { SiteResultComponent } from '../site-result/site-result.component';
import { SiteSelectionComponent } from '../site-selection/site-selection.component';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { Sites } from '../../../model/site/sites';

@Component({
  selector: 'app-rate-plans',
  standalone: true,
  imports: [
    RouterOutlet,
    RouterLink,
    VideoConnectTabsComponent,
    CommonModule,
    DataTablesModule,
    SearchFilterComponent,
    SiteResultComponent,
    AgencyResultComponent,
    SiteSelectionComponent,
    ReactiveFormsModule
  ],
  templateUrl: './rate-plans.component.html',
  styleUrls: ['./rate-plans.component.css']
})

export class RatePlansComponent implements OnInit {

  @ViewChild(SearchFilterComponent) searchFilterComponent!: SearchFilterComponent;
  @ViewChild('addBillingRateModal') addBillingRateModal!: ElementRef;
  @ViewChild('editModel') editModel: ElementRef | undefined;

  //#region "variable declaration start"

  searchResultsData: any;
  contractNoForModal: number = 0;
  callTypeForModal: string = '';
  Math = Math;
  description: string = '';
  siteId: string = '';
  siteName: string = '';
  currentPage: number = 1;
  recordsPerPage: number = 10;
  scurrentPage: number = 1;
  srecordsPerPage: number = 10;
  dataTotalCount: number = 0;
  siteTotalCount: number = 0;
  sortOrder: string = 'ASC';
  hasData: boolean = true;
  contractNoMismatch: boolean = false;
  siteIdMismatch: boolean = false;
  isSaveBtn: boolean = true;
  errorMessage: string = '';
  isTrue: boolean = false;
  isEditMode: boolean = false;
  BillingPlanForm!: FormGroup;
  noDataMessage: string = '';
  plansSiteList: PlanRates[] = [];
  planRateList: PlanRates[] = [];
  commTypes: { commTypeId: number; commTypeDs: string }[] = [];
  searchData!: searchValue;
  selectedContractNo: string = '';
  showResults: boolean = false;
  gridErrorMessage: string = '';

  constructor(private videoSrv: VideoConnectRatesService, private ratePlan: FormBuilder) {
    this.BillingPlanForm = this.ratePlan.group({
      siteId: [''],
      securusRate: [''],
      commRate: [''],
      agencyRate: [''],
      includeAgencyInTotal: [false],
      commType: [''],
      roundingThreshold: ['',],
      activeStatus: [true],
      startDate: [''],
      auditNote: ['']
    });
  }

  ngOnInit(): void {

    this.hasData = true;
    this.loadCommTypes()

  }

  onItemClicked(event: { type: string; value: string }) {
    const contractNo = event.type === 'contract' ? event.value : undefined;
    const siteId = event.type === 'site' ? event.value : undefined;

    // Call the search method with the correct parameters
    this.searchFilterComponent.onSearch(
      this.currentPage,
      this.recordsPerPage,
      this.scurrentPage,
      this.srecordsPerPage,
      contractNo,
      siteId
    );
  }

  onContractSelected(contractNo: string) {
    this.selectedContractNo = contractNo;
    this.searchFilterComponent.onSearch(
      this.currentPage,
      this.recordsPerPage,
      this.scurrentPage,
      this.srecordsPerPage,
      this.selectedContractNo.toString(),
      undefined);
  }

  onSearchResults(results: { planRateList: PlanRates[], dataTotalCount: number, plansSiteList: PlanRates[], siteTotalCount: number, isTrue: boolean, searchData: searchValue, gridErrorMessage: string }): void {
    this.planRateList = results.planRateList;
    this.dataTotalCount = results.dataTotalCount;
    this.plansSiteList = results.plansSiteList;
    this.siteTotalCount = results.siteTotalCount;
    this.isTrue = results.isTrue;
    this.searchData = results.searchData
    this.gridErrorMessage = results.gridErrorMessage;
  }

  onSearchFormData(data: { contractNo: number; callTypeCd: string }): void {
    this.contractNoForModal = data.contractNo;
    this.callTypeForModal = data.callTypeCd;
  }

  // Method to handle pagination changes from SiteResultComponent
  onPaginationChanged(event: { currentPage: number, recordsPerPage: number, scurrentPage: number, srecordsPerPage: number }): void {
    this.searchFilterComponent.onSearch(event.currentPage, event.recordsPerPage, event.scurrentPage, event.srecordsPerPage);
  }

  openAddBillingRateModalWithDetails(rate: PlanRates): void {
    this.isEditMode = true;
    this.populateModal(rate);
    const modalElement = document.getElementById('exampleModal');
    if (modalElement) {
      modalElement.classList.add('show');
    }
  }

  onSiteDataFetched(eventData: Sites) {
    this.searchData.siteId = eventData.siteId;
    this.searchData.siteName = eventData.siteName;
    this.searchData.state = eventData.state;
    this.searchData.contractNo = eventData.contractNo;
    this.searchData.isSearchBySiteIdOrName = eventData.isSearchBySiteIdOrName;
    //this.searchData.isSiteClicked = eventData.isSiteClicked;   
    this.searchFilterComponent.onSearch(
      this.currentPage,
      this.recordsPerPage,
      this.scurrentPage,
      this.srecordsPerPage,
      undefined,
      eventData.siteId,
      eventData.isSiteClicked);
  }

  populateModal(rate: PlanRates): void {
    this.siteName = rate.siteName;
    const formattedStartDate = this.formatDate(rate.startDt);
    this.contractNoForModal = this.searchFilterComponent.SearchRatePlanForm.get('contractNo')?.value || 0;;
    this.callTypeForModal = this.searchFilterComponent.SearchRatePlanForm.get('callTypeCd')?.value || '';;
    this.BillingPlanForm.patchValue({
      siteId: rate.siteId,
      securusRate: rate.securusRatePerMinAmt,
      commRate: rate.commRatePerMinAmt,
      agencyRate: rate.agencyRatePerMinAmt,
      includeAgencyInTotal: rate.includeAgencyRateInTotalFl,
      commType: this.getCommTypeIdFromDescription(rate.commTypeDs.toString()),
      roundingThreshold: rate.roundingThresholdNr,
      startDate: formattedStartDate,
      auditNote: rate.auditNoteTxt,
      activeStatus: rate.active
    });
  }


  openAddBillingRateModal(): void {
    if (this.addBillingRateModal.nativeElement) {
      this.isEditMode = false;
      this.BillingPlanForm.reset(); // Reset the form
      this.BillingPlanForm.patchValue({
        siteId: '',
        securusRate: '',
        commRate: '',
        agencyRate: '',
        includeAgencyInTotal: false,
        commType: '',
        roundingThreshold: '',
        startDate: '',
        auditNote: '',
        activeStatus: true
      });
      this.loadCommTypes();
      this.contractNoForModal = this.searchFilterComponent.SearchRatePlanForm.get('contractNo')?.value || 0;;
      this.callTypeForModal = this.searchFilterComponent.SearchRatePlanForm.get('callTypeCd')?.value || '';;
      this.addBillingRateModal.nativeElement.classList.add('show');
    }
  }

  formatDate(date: Date): string {
    if (!date) return '';
    const d = new Date(date);
    const year = d.getFullYear();
    const month = (d.getMonth() + 1).toString().padStart(2, '0');
    const day = d.getDate().toString().padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  getCommTypeIdFromDescription(description: string): number | null {
    const commType = this.commTypes.find(type => type.commTypeDs === description);
    return commType ? commType.commTypeId : null;
  }

  loadCommTypes(): void {
    this.videoSrv.getCommTypes().subscribe(
      (response) => {
        this.commTypes = response.map(item => ({
          commTypeId: item.commTypeId,
          commTypeDs: item.commTypeDs
        }));
      },
      (error) => {       
        this.commTypes = [];
      }
    );
  }

  saveBillingRate(): void {
    if (this.BillingPlanForm.valid) {
      const formData = this.BillingPlanForm.value;
      formData.contractNoForModal = this.contractNoForModal;
      formData.callTypeForModal = this.callTypeForModal;
      this.videoSrv.addBillingRatePlan(formData).subscribe(
        (response: { data: PlanRates[] }) => {
          this.BillingPlanForm.reset();
          this.closeModal();
          alert("Data saved successfuly!")
        },
        (error) => {
          alert("Data not saved!")
        }
      );
    } else {
      alert("form data is not valid!")
    }
  }

  onSiteIdChange(): void {
    const siteIdControl = this.BillingPlanForm.get('siteId');
    const contractNoControl = this.contractNoForModal;
    if (!siteIdControl) return;

    const siteId = siteIdControl.value?.trim();
    if (!siteId) {
      this.siteName = '';
      return;
    }
    this.videoSrv.getSiteById(siteId, contractNoControl).subscribe(
      (response: { siteName: string, contractNo: number }) => {
        if (response && response.siteName) {
          if (String(response.contractNo).trim() === String(contractNoControl).trim()) {
            this.siteName = response.siteName;
            this.contractNoMismatch = false;
            this.siteIdMismatch = false;
            this.isSaveBtn = true;
          } else {
            this.contractNoMismatch = true;
            this.siteIdMismatch = false;
            this.isSaveBtn = false;
            this.errorMessage = `The SiteId : ${siteId} not belongs to this Contract No : ${this.contractNoForModal}`;

            setTimeout(() => {
              this.errorMessage = '';
              this.contractNoMismatch = false;
              this.isSaveBtn = false;
            }, 3000);
          }
        } else {
          this.siteName = '';
          this.contractNoMismatch = false;
          this.isSaveBtn = false;
        }
      },
      (error) => {
        this.siteIdMismatch = true;
        this.siteName = '';
        this.contractNoMismatch = false;
        this.isSaveBtn = false;
        this.errorMessage = `The SiteID ${siteId} does not exist in the database!`;

        setTimeout(() => {
          this.errorMessage = '';
          this.siteIdMismatch = false;
        }, 3000);
      }
    );
  }

  closeModal(): void {
    if (this.addBillingRateModal) {
      this.addBillingRateModal.nativeElement.classList.remove('show');
    }
  }

  onEditClick(): void {
    if (this.editModel) {
      this.editModel.nativeElement.classList.add('show');
    }
  }

  closeEditModal(): void {
    if (this.editModel) {
      this.editModel.nativeElement.classList.remove('show');
    }
  }
}
