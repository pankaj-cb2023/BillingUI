import { Component, EventEmitter, input, Input, Output } from '@angular/core';
import { PlanRates } from '../../../model/rate-plan/planrates';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-siteselection',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './site-selection.component.html',
  styleUrl: './site-selection.component.css'
})
export class SiteSelectionComponent {

  scurrentPage: number = 1;
  srecordsPerPage: number = 10;

  @Input() plansSiteList: PlanRates[] = [];
  @Input() siteTotalCount: number = 0;
  @Output() paginationChanged = new EventEmitter<any>();
  @Output() itemClicked = new EventEmitter<{ type: string; value: string }>();


  ngOnInit() {    
  }

  handleClick(type: 'contract' | 'site', value: string, event: Event): void {
    event.preventDefault();    
    this.itemClicked.emit({ type, value });
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
