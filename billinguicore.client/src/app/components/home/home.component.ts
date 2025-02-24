import { Component } from '@angular/core';
import { RouterLink, RouterOutlet, Router } from '@angular/router';
import { VideoConnectRateSharedService } from '../../shared/video-connect-rate-shared-service';
import { AuthService } from '../../services/auth/auth.service';
import { User } from '../../model/user/User';
import { CommonModule } from '@angular/common';
import { Constant } from '../../constants/Constant';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RouterLink, RouterOutlet, CommonModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {

  user: User | null = null;
  showUserPermissionButton: boolean = false;
  allowedModules: string[] = [];
  hasReadOnlyPermission: boolean = false;

  constructor(private sharedService: VideoConnectRateSharedService, private authService: AuthService, private router: Router ) { }


  ngOnInit(): void {
    // Avoid redirect loop if we are already on the access-denied route
    const currentUrl = this.router.url;
    if (currentUrl !== '/access-denied') {
      this.checkUserStatus();
    }
  }

  checkUserStatus(): void {
    this.authService.getUser().subscribe(
      (response: User) => {
        if (response) {
          this.user = response;

          // Check if the user has read-only permission
          //this.hasReadOnlyPermission = response.roleId === 5 && response.roleName === Constant.ROLE_PERMISSION.READONLY;
          this.hasReadOnlyPermission = response.permissions[0].canWrite 
          this.sharedService.setReadOnlyPermission(this.hasReadOnlyPermission);
          this.showUserPermissionButton = response.roleId == 5 && response.roleName == Constant.ROLE_PERMISSION.ADMINISTRATOR;

          this.router.navigate(['/']); 
        } else {
          this.router.navigate(['/access-denied']); 
        }
      },
      (error) => {
        this.router.navigate(['/access-denied']); 
      }
    );
  }


  setDropdownOption(option: string) {
    this.sharedService.setSelectedOption(option);
  }

}
