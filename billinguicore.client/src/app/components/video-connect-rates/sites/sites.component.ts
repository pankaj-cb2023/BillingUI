import { Component, ElementRef, ViewChild } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import { VideoConnectTabsComponent } from '../video-connect-tabs/video-connect-tabs.component';
import { CommonModule } from '@angular/common';
import { Sites } from '../../../model/site/sites';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { VideoConnectRatesService } from '../../../services/video-connect-rates/video-connect-rates.service';

@Component({
  selector: 'app-sites',
  standalone: true,
  imports: [RouterLink, RouterOutlet, VideoConnectTabsComponent, CommonModule, ReactiveFormsModule],
  templateUrl: './sites.component.html',
  styleUrl: './sites.component.css'
})
export class SitesComponent {
  @ViewChild('groupmodel') groupmodel: ElementRef | undefined;
  @ViewChild('editmodel') editmodel: ElementRef | undefined;
  isLoading = false;
  currentPage: number = 1;
  recordsPerPage: number = 10;
  totalCount: number = 0;
  isEditModalOpen = false;
  sitesList: Sites[] = [];
  SiteForm: FormGroup = new FormGroup({});
  editForm: FormGroup = new FormGroup({});

  constructor(private fb: FormBuilder, private videoSrv: VideoConnectRatesService) {
    this.SiteForm = this.fb.group({
      SiteId: [''],
      SiteName: [''],
      State: [''],
      ContractNo: ['']
    });

    this.editForm = this.fb.group({
      siteId: [''],
      siteName: ['']
    });
  }

  onSearch() {
    this.isLoading = true;
    const formValues = { ...this.SiteForm.value };
    formValues.pageNumber = this.currentPage;
    formValues.pageSize = this.recordsPerPage;

    // Call the service to get site data
    this.videoSrv.getAllSite(formValues).subscribe(
      (res: any) => {
        this.totalCount = res && typeof res.totalCount === 'number' ? res.totalCount : 0;
        if (res && Array.isArray(res.data) && res.data.length > 0) {
          this.sitesList = res.data.map((item: any) => ({
            siteId: item.siteId,
            siteName: item.siteName,
            contractNo: item.contractNo,
            state: item.state,
          }));
        } else {
          this.sitesList = [];
        }
        // End loading state
        this.isLoading = false;
      },
      (error) => {
        // Handle errors and reset state
        console.error('Error fetching site data:', error);
        this.totalCount = 0;
        this.sitesList = [];
        this.isLoading = false;
      }
    );
  }

  resetForm() {
    // Reset the form to its initial state
    this.SiteForm.reset();
  }

  changeRecordsPerPage(event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    this.recordsPerPage = +selectElement.value;

    // Reset to the first page after changing the records per page
    this.currentPage = 1;
    this.updatePaginatedList(this.currentPage);
  }

  updatePaginatedList(page: number): void {
    if (page < 1 || page > Math.ceil(this.totalCount / this.recordsPerPage)) {
      return;
    }
    this.currentPage = page;
    this.onSearch();
  }

  getPageNumbers(): (number | null)[] {
    const totalPages = Math.ceil(this.totalCount / this.recordsPerPage);
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
        pageNumbers.push(null); // Use null instead of '...'
      }
    }
    return pageNumbers;
  }

  updateSite() {
    if (this.editForm.valid) {
      const updatedData = this.editForm.value;

      this.videoSrv.updateSite(updatedData.siteId, updatedData.siteName).subscribe(
        response => {

          this.closeEditModal();

          this.onSearch();
        },
        error => {
          // Handle error
          alert('Failed to update site');
        }
      );
    } else {
      console.error('Edit form is invalid');
    }
  }


  openEditModal(site: Sites) {
    this.isEditModalOpen = true;

    this.editForm.setValue({
      siteId: site.siteId || '',
      siteName: site.siteName || ''
    });
  }


  openGroupModal() {
    if (this.groupmodel) {
      this.groupmodel.nativeElement.classList.add('show');
    }
  }

  closeGroupModal() {
    if (this.groupmodel) {
      this.groupmodel.nativeElement.classList.remove('show');
    }
  }


  closeEditModal() {
    this.isEditModalOpen = false;
  }
}
