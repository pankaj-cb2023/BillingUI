import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, RouterLink, RouterOutlet } from '@angular/router';
import { VideoConnectTabsComponent } from "../../video-connect-tabs/video-connect-tabs.component";
import { CommonModule } from '@angular/common';
//import { RatePlansInnerTabSummaryComponent } from '../rate-plans-inner-tab/rate-plans-inner-tab-summary/rate-plans-inner-tab-summary/rate-plans-inner-tab-summary.component';
//import { RatePlansInnerTabSitesComponent } from '../rate-plans-inner-tab/rate-plans-inner-tab-sites/rate-plans-inner-tab-sites/rate-plans-inner-tab-sites.component';
//import { RatePlansInnerTabRatesComponent } from '../rate-plans-inner-tab/rate-plans-inner-tab-rates/rate-plans-inner-tab-rates/rate-plans-inner-tab-rates.component';
//import { RatePlansInnerTabHistoryComponent } from '../rate-plans-inner-tab/rate-plans-inner-tab-history/rate-plans-inner-tab-history/rate-plans-inner-tab-history.component';
import { Plan } from '../../../../model/rate-plan/plan';


@Component({
  selector: 'app-rate-plans-details',
  standalone: true,
  imports: [RouterLink, RouterOutlet, CommonModule, VideoConnectTabsComponent],
  templateUrl: './rate-plans-details.component.html',
  styleUrl: './rate-plans-details.component.css'
})
export class RatePlansDetailsComponent implements OnInit {

  planDetail: Plan = {
    billingRateId: 0,
    siteName: '',
    siteId: '',   
    securusRate: '',
    agencyRate: '',
    commonType: 0,
    roundingThresholdNr: '',
    startDate: new Date,
    endDate: new Date
  };

  constructor(private activatedRoute: ActivatedRoute) {

  }

  ngOnInit(): void {
    const navigationState = history.state;
    if (navigationState) {
      this.planDetail = navigationState;
    }
  }


  @ViewChild('model') model: ElementRef | undefined;

  openModal() {
    if (this.model) {
      this.model.nativeElement.classList.add('show');
    }
  }

  closeModal() {
    if (this.model) {
      this.model.nativeElement.classList.remove('show');
    }
  }

 

 

  activeTab: string = 'summary';  // Default tab

  setActiveTab(tab: string) {
    this.activeTab = tab;
  }

  showRatesComponent: boolean = true;  // Controls visibility of the rates component

  // Toggle visibility to show the edit component
  openEditComponent() {
    this.showRatesComponent = false;
  }

}
