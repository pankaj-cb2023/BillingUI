import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { VideoConnectRateSharedService } from '../../shared/video-connect-rate-shared-service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent  {
  selectedOption: string = '';

  constructor(private router: Router, private sharedService: VideoConnectRateSharedService) { }

  ngOnInit() {
    this.sharedService.selectedOption$.subscribe((option: string) => {
      if (option) {
        this.selectedOption = option;
      }
    });
  }

  onOptionChange() {
    if (this.selectedOption === 'Video Connect Rates') {
      this.router.navigate(['/rate-plans']);
    } else if (this.selectedOption === '6') {
      this.router.navigate(['/enrollee-pay-rate-plans']);
    }
    else if (this.selectedOption === '2') {
      this.router.navigate(['/payment-config-payment-fees']);
    }
    else if (this.selectedOption === '1') {
      this.router.navigate(['/']);
    }
    else if (this.selectedOption === '5') {
      this.router.navigate(['/text-connect-rate-plan']);
    }
  }

}
