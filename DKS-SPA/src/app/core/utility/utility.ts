import { DatePipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
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
  datepiper = this.datepipe;
  activeRouter = this.acRouter;
  router = this.route;
  constructor(
    private cHttp: HttpClient,
    private cAlertify: AlertifyService,
    private cSpinner: NgxSpinnerService,
    private datepipe: DatePipe,
    private acRouter: ActivatedRoute,
    private route: Router,
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
