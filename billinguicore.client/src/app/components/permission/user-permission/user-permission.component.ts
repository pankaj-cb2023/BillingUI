import { Component, ElementRef, ViewChild, AfterViewInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink, RouterOutlet } from '@angular/router';
import { AuthService } from '../../../services/auth/auth.service';
import { User, UserSearchResponse, UserRole, UserRoleResponse } from '../../../model/user/User';
import { CommonModule } from '@angular/common';
import { debounceTime } from 'rxjs/internal/operators/debounceTime';
import { distinctUntilChanged, firstValueFrom, lastValueFrom } from 'rxjs';

@Component({
  selector: 'app-user-permission',
  standalone: true,
  imports: [RouterLink, RouterOutlet, ReactiveFormsModule, CommonModule, FormsModule],
  templateUrl: './user-permission.component.html',
  styleUrl: './user-permission.component.css'
})
export class UserPermissionComponent {

  SearchUserForm!: FormGroup;
  addUserForm!: FormGroup;
  addRoleForm!: FormGroup;
  noDataMessage: string = '';
  paginatedUsersList: User[] = [];
  paginatedRoleList: UserRole[] = [];
  currentPage: number = 1;
  recordsPerPage: number = 5;
  totalRecords: number = 0;
  activeTab: string = 'user';
  modalTitle: string = '';
  isEditMode: boolean = false;
  roleId?: number = 0;

  @ViewChild('addUserModal') addUserModal!: ElementRef;
  @ViewChild('addRoleModal') addRoleModal!: ElementRef;
  @ViewChild('editRoleAccessModal') editRoleAccessModal!: ElementRef;



/*  newRole: any = { roleName: '' };*/


  

  constructor(private authService: AuthService, private SearchUser: FormBuilder, private addUser: FormBuilder, private addRole: FormBuilder, private router: Router) { }

  ngOnInit(): void {
    this.initSearchForm();
    this.initUserForm();
    this.initRoleForm();
    this.getUsers();
  }

  // Initialize search form with debounce
  initSearchForm() {
    this.SearchUserForm = this.SearchUser.group({
      searchQuery: ['']
    });

    this.SearchUserForm.get('searchQuery')?.valueChanges
      .pipe(
        debounceTime(3000),
        distinctUntilChanged()
      )
      .subscribe(() => {
        this.onSearch();
      });
  }


  initUserForm() {
    this.addUserForm = this.addUser.group({
      username: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      role: ['', Validators.required]
    });
  }

  initRoleForm() {
    this.addRoleForm = this.addRole.group({
      roleName: ['', [Validators.required, Validators.required]]
    });
  }


  openAddUserModal(): void {
    if (this.addUserModal.nativeElement) {
      this.addUserModal.nativeElement.classList.add('show');
      this.modalTitle = 'Add User';
      this.isEditMode = false;
      // Enable fields for Add Mode
      this.addUserForm.get('username')?.enable();
      this.addUserForm.get('email')?.enable();
      this.getRoles(this.roleId);
      this.addUserForm.reset();
    }
  }

  closeUserModal(): void {
    if (this.addUserModal) {
      this.addUserModal.nativeElement.classList.remove('show');
    }
  }

  openEditUserModal(user: User): void {
    if (this.addUserModal.nativeElement) {      
      this.getRoles(this.roleId);
      this.addUserModal.nativeElement.classList.add('show');
      this.modalTitle = 'Edit User';
      this.isEditMode = true;

      this.addUserForm.get('username')?.disable();
      this.addUserForm.get('email')?.disable();

      const selectedRole = this.paginatedRoleList.find(role => role.roleName === user.roleName);
      this.addUserForm.patchValue({
        username: user.userName,
        email: user.email,
        role: selectedRole ? selectedRole.roleId : null
      });
    }
  }

  openAddRoleModal(): void {
    if (this.editRoleAccessModal.nativeElement) {
      this.editRoleAccessModal.nativeElement.classList.add('show');
      this.modalTitle = 'Add Role Access';
      //this.getRoles();
      //this.addUserForm.reset();
    }
  }


  //openEditRoleModal(role: number): void {
  //  if (this.addRoleModal.nativeElement) {
  //    this.addRoleModal.nativeElement.classList.add('show');
  //    this.modalTitle = 'Edit Role';
  //  }
  //}


  closeRoleModal(): void {
    if (this.addRoleModal) {
      this.addRoleModal.nativeElement.classList.remove('show');
    }
  }




  deleteUserRole(user: any) {
    const id = user.id;
    this.authService.deleteUser(id).subscribe(
      () => {

      },
      () => {
        this.noDataMessage = 'An error occurred while adding the user.';
      }
    );
  }

  modules = [
    { name: 'Video Connect Rate', read: false, write: false },
    { name: 'Text Connect Rate', read: false, write: false },
    { name: 'Payment Config', read: false, write: false },
    { name: 'Enroll Pay Rate', read: false, write: false }
  ];

  roleName: string = '';
  module: any[] = [];
  editRole: any[]=[];

  openEditRoleModal(roleId?: number): void {
    this.modalTitle = 'Edit Role';
    this.noDataMessage = '';

    this.authService.getRoles(roleId).subscribe(
      (response: any) => {
        if (response && response.data && response.data.length > 0) {
          // Flatten and map API response to a lookup object
          const apiModules = response.data
            .flatMap((role: any) => role.modules || [])
            .reduce((acc: any, module: any) => {
              acc[module.name] = { read: module.read, write: module.write };
              return acc;
            }, {});

          // Merge predefined modules with API response
          this.modules = this.modules.map((module) => ({
            name: module.name,
            read: apiModules[module.name]?.read ?? false, 
            write: apiModules[module.name]?.write ?? false
          }));

          this.editRole = response.data;
          console.log(this.editRole, 'editRole');
          this.showModal();
        } else {
          this.noDataMessage = 'No role data found.';
        }
      },
      (error) => {
        this.noDataMessage = 'An error occurred while fetching the role data.';
      }
    );
  }



  showModal(): void {
    console.log(this.editRole, 'within-modal');
    if (this.addRoleModal?.nativeElement) {
      this.addRoleModal.nativeElement.classList.add('show');
    }
    this.modules.forEach(module => {
      const role = this.editRole.find(item => module.name.includes(item.moduleName));
      if (role) {
        module.read = role.canRead;
        module.write = role.canWrite;
      } else {
        module.read = false;
        module.write = false;
      }
    });
  }


  saveUser(): void {
    if (this.addUserForm.valid) {
      const formData = this.addUserForm.getRawValue();
      this.authService.addUser(formData).subscribe(
        (response) => {
          if (response != null) {
            this.noDataMessage = '';
            this.getUsers();
            this.addUserForm.reset();
            this.addUserModal.nativeElement.classList.remove('show');
          } else {
            this.noDataMessage = 'Failed to add user.';
          }
        },
        (error) => {
          this.noDataMessage = 'An error occurred while adding the user.';
        }
      );
    } else {
      this.noDataMessage = 'Please fill in all required fields correctly.';
    }
  }

  saveRole() {
  }


  deleteUser(userId: number): void {
    const confirmDelete = window.confirm("Are you sure you want to delete this user?");
    if (confirmDelete) {
      this.authService.deleteUser(userId).subscribe({
        next: () => {
          alert("User has been deleted successfully.");
          this.getUsers(); // Refresh user list after deletion
        },
        error: (err) => {
          alert("Failed to delete user. Please try again.");
        }
      });
    }
  }

  //openModal(isEdit: boolean, role?: any) {
  //  if (isEdit && role) {
  //    this.modalTitle = 'Edit Role';
  //    this.newRole = { ...role }; // Populate role details for editing
  //  } else {
  //    this.modalTitle = 'Add Role';
  //    this.newRole = { roleName: '' }; // Reset role data
  //  }
  //}

  onTabChange(tab: string) {
    this.activeTab = tab;
    let roleId: number = 0;

    if (tab === 'role' && this.paginatedRoleList.length === 0) {
      this.getRoles(roleId);
    }
  }

  getUsers() {
    const formData = this.SearchUserForm.value;
    formData.pageNumber = this.currentPage;
    formData.pagesize = this.recordsPerPage;
    this.authService.searchUser(formData).subscribe(
      (response: UserSearchResponse) => {
        if (response && Array.isArray(response.data)) {
          this.paginatedUsersList = response.data;
          this.totalRecords = response.totalCount;
        } else {
          this.noDataMessage = 'No users found.';
        }
      },
      (error) => {
        this.noDataMessage = 'An error occurred while fetching data';
      }
    );
  }

  onSearch(): void {
    if (this.SearchUserForm.valid) {
      const formData = this.SearchUserForm.value;
      formData.currentPage = this.currentPage;
      formData.recordsPerPage = this.recordsPerPage;
      this.authService.searchUser(formData).subscribe(
        (response: UserSearchResponse) => {
          if (response && response.data && response.data.length > 0) {
            this.paginatedUsersList = response.data;
            this.noDataMessage = ''; // Reset the no data message if data is found
          } else {
            this.paginatedUsersList = [];
            this.noDataMessage = 'No users found.'; // Set the no data message if no users are returned
          }
        },
        (error) => {
          this.paginatedUsersList = [];
          this.noDataMessage = 'An error occurred while fetching data'; // Set the error message
        }
      );
    } else {
      this.noDataMessage = 'Please enter a valid search query'; // Validation error
    }
  }

  //getRoles(roleId?: number): Promise<void> {
  //  return lastValueFrom(this.authService.getRoles(roleId)).then(
  //    (response: UserRoleResponse) => {
  //      if (response && Array.isArray(response.data)) {
  //        this.paginatedRoleList = response.data;
  //      } else {
  //        this.noDataMessage = 'No roles found.';
  //      }
  //    }
  //  ).catch(() => {
  //    this.noDataMessage = 'An error occurred while fetching data';
  //  });
  //}



  getRoles(roleId?: number) {
    this.authService.getRoles(roleId).subscribe(
      (response: UserRoleResponse) => {
        if (response && Array.isArray(response.data)) {
          this.paginatedRoleList = response.data;
        } else {
          this.noDataMessage = 'No users found.';
        }
      },
      (error) => {
        this.noDataMessage = 'An error occurred while fetching data';
      }
    );
  }


  // #region pagination start
  getPageNumbers(): (number | null)[] {
    const totalPages = Math.ceil(this.totalRecords / this.recordsPerPage);
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
        pageNumbers.push(null);
      }
    }
    return pageNumbers;
  }


  changeRecordsPerPage(event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    this.recordsPerPage = +selectElement.value;

    // Reset to the first page after changing the records per page
    this.currentPage = 1;
    this.updatePaginatedList(this.currentPage);
  }


  prevPage(): void {
    if (this.currentPage > 1) {
      this.updatePaginatedList(this.currentPage - 1);
    }
  }

  nextPage(): void {
    if (this.currentPage < Math.ceil(this.totalRecords / this.recordsPerPage)) {
      this.updatePaginatedList(this.currentPage + 1);
    }
  }

  updatePaginatedList(page: number): void {
    if (page < 1 || page > Math.ceil(this.totalRecords / this.recordsPerPage)) {
      return;
    }
    this.currentPage = page;
    this.getUsers();
  }


  // #endregion pagination end

}


