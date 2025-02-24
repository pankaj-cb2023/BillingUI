import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Constant } from '../../constants/Constant';
import { Observable } from 'rxjs';
import { AddSite, Plan } from '../../model/rate-plan/plan';
import { EventLogRequest } from '../../model/rate-plan/planhistory';
import { PlanRates } from '../../model/rate-plan/planrates';
import { CommissionType } from '../../model/rate-plan/commissionType';
import { Sites } from '../../model/site/sites';

@Injectable({
  providedIn: 'root'
})
export class VideoConnectRatesService {
  constructor(private http: HttpClient) {
  }

  searchPlan(formData: Plan): Observable<any> {
    const url = Constant.COMMON_API_URL + Constant.VIDEO_CONNECT_RATE.GET_ALL_PLANS;
    return this.http.post(url, formData, { withCredentials: true });
  }

  searchMultipleSite(formData: Plan): Observable<any> {
    const url = Constant.COMMON_API_URL + Constant.VIDEO_CONNECT_RATE.GET_ALL_PLAN_SITE;
    return this.http.post(url, formData);
  }


  //need to add query param
  getSiteById(siteId: string, contractNo: number): Observable<{ siteName: string, contractNo: number }> {
    const url = `${Constant.COMMON_API_URL}${Constant.VIDEO_CONNECT_RATE.GET_SITE_By_SITE_ID}?siteId=${siteId}&contractNo=${contractNo}`;
    return this.http.get<{ siteName: string, contractNo: number }>(url);
  }

  updateBillingRate(planDetail: Plan): Observable<any> {
    const url = Constant.COMMON_API_URL + Constant.VIDEO_CONNECT_RATE.UPDATE_BILIING_RATE_PLAN;
    return this.http.put(url, planDetail);
  }

  addBillingRatePlan(planData: PlanRates): Observable<any> {
    const url = Constant.COMMON_API_URL + Constant.VIDEO_CONNECT_RATE.ADD_BILLING_RATE_PLAN;
    return this.http.post(url, planData);
  }

  getSiteBySiteId(siteId: string, contractNo: string): Observable<PlanRates> {
    const url = `${Constant.COMMON_API_URL}${Constant.VIDEO_CONNECT_RATE.GET_SITE_By_SITE_ID}?siteId=${siteId}&contractNo=${contractNo}`;
    return this.http.get<PlanRates>(url);
  }


 


  getCommTypes(): Observable<CommissionType[]> {
    const url = Constant.COMMON_API_URL + Constant.VIDEO_CONNECT_RATE.GET_COMM_TYPE;
    return this.http.get<CommissionType[]>(url, { withCredentials: true });
  }

  getAllSite(formData: any): Observable<any> {
    const url = Constant.COMMON_API_URL + Constant.VIDEO_CONNECT_RATE.GET_ALL_PLAN_SITE;
    return this.http.post(url, formData);
  }

  addSite(dataToSend: AddSite[]): Observable<any> {
    const url = Constant.COMMON_API_URL + Constant.VIDEO_CONNECT_RATE.ADD_SITE;
    return this.http.post(url, dataToSend);
  }
  updateSite(siteId: string, siteName: string): Observable<any> {
    const url = `${Constant.COMMON_API_URL}${Constant.VIDEO_CONNECT_RATE.UPDATE_SITE}?siteId=${siteId}&SiteName=${siteName}`;
    return this.http.put(url, null);
  }

  getEventsHistory(requestModel: EventLogRequest): Observable<any> {
    const url = Constant.COMMON_API_URL + Constant.VIDEO_CONNECT_RATE.GET_EVENTS_HISTORY;
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });
    return this.http.post<any>(url, requestModel, { headers });
  }




}


