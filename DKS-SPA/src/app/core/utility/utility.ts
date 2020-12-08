import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { environment } from '../../../environments/environment';
import { AlertifyService } from '../_services/alertify.service';

@Injectable({
  providedIn: 'root',
})
export class Utility {

  baseUrl = environment.apiUrl;
  http = this.cHttp;
  alertify = this.cAlertify;
  spinner = this.cSpinner;

  constructor(
    private cHttp: HttpClient,
    private cAlertify: AlertifyService,
    private cSpinner: NgxSpinnerService
  ) {}

  getToDay() {
    const toDay =
      new Date().getFullYear().toString() +
      '/' +
      (new Date().getMonth() + 1).toString() +
      '/' +
      new Date().getDate().toString();
    return toDay;
  }

  getDateFormat(day: Date) {
    const dateFormat =
      day.getFullYear().toString() +
      '/' +
      (day.getMonth() + 1).toString() +
      '/' +
      day.getDate().toString();
    return dateFormat;
  }
}
