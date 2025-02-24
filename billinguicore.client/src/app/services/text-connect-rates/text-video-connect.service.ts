import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Constant } from '../../constants/Constant';

@Injectable({
  providedIn: 'root'
})
export class TextVideoConnectService {

  constructor(private http: HttpClient) {

  }

  getAllTextRate() {
    return this.http.get(Constant.COMMON_API_URL + Constant.TEXT_VIDEO_CONNECT.GET_ALL_TEXT_RATE);

  }
}
