import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { AppComponent } from '../../../app.component';

@Component({
  selector: 'app-video-connect-tabs',
  standalone: true,
  imports: [RouterOutlet, RouterLink, AppComponent, RouterLinkActive],
  templateUrl: './video-connect-tabs.component.html',
  styleUrl: './video-connect-tabs.component.css'
})
export class VideoConnectTabsComponent {

}
