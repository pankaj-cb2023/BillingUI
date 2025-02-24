import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Constant } from '../../constants/Constant';
import { User, UserRequest, UserRoleResponse, UserSearchResponse } from '../../model/user/User';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient) { }

  getUser(): Observable<User> {
    const url = Constant.COMMON_API_URL + Constant.AUTH_USER.GET_USER;
    return this.http.get<User>(url, { withCredentials: true });
  }

  searchUser(formData: User): Observable<UserSearchResponse> {
    const url = Constant.COMMON_API_URL + Constant.AUTH_USER.SEARCH_USER;
    return this.http.post<UserSearchResponse>(url, formData, { withCredentials: true });
  }

  getRoles(roleId?: number): Observable<UserRoleResponse> {
    const url = `${Constant.COMMON_API_URL}${Constant.AUTH_USER.GET_ROLES}?roleId=${roleId}`;
    return this.http.get<UserRoleResponse>(url, { withCredentials: true });
  }

  addUser(formData: UserRequest): Observable<User> {
    const url = Constant.COMMON_API_URL + Constant.AUTH_USER.ADD_USER;
    return this.http.post<User>(url, formData, { withCredentials: true });
  }

  deleteUser(userId: number): Observable<boolean> {
    const url = `${Constant.COMMON_API_URL}${Constant.AUTH_USER.DELETE_USER}?userId=${userId}`;
    return this.http.delete<boolean>(url, { withCredentials: true });
  }

}
