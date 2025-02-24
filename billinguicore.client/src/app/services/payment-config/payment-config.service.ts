import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Constant } from '../../constants/Constant';

@Injectable({
  providedIn: 'root'
})
export class PaymentConfigService {

  constructor(private http: HttpClient) {

  }

  paymentConfig() {
    return this.http.get(Constant.PAYMENT_CONFIG + Constant.PAYMENT_CONFIG.PAYMENT_CONFIG_RATE);
  }
}
