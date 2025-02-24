import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { HeaderComponent } from './components/header/header.component';
import { FooterComponent } from './components/footer/footer.component';
import { RatePlansComponent } from './components/video-connect-rates/rate-plans/rate-plans.component';
import { VideoConnectTabsComponent } from './components/video-connect-rates/video-connect-tabs/video-connect-tabs.component';
import { NgxPaginationModule } from 'ngx-pagination';


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, HeaderComponent, FooterComponent, RatePlansComponent, VideoConnectTabsComponent, RouterLink, RouterLinkActive, NgxPaginationModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'BillingUI';
}
