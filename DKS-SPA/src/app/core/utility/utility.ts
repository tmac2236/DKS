import { DatePipe } from "@angular/common";
import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { NgxSpinnerService } from "ngx-spinner";
import { environment } from "../../../environments/environment";
import { Pagination } from "../_models/pagination";
import { AlertifyService } from "../_services/alertify.service";
import { LanguageService } from "../_services/language.service";

@Injectable({
  providedIn: "root",
})
export class Utility {
  baseUrl = environment.apiUrl;
  //http = this.cHttp;
  //alertify = this.cAlertify;
  //spinner = this.cSpinner;
  //datepiper = this.datepipe;
  //activeRouter = this.acRouter;
  //router = this.route;
  constructor(
    public http: HttpClient,
    public alertify: AlertifyService,
    public spinner: NgxSpinnerService,
    public datepiper: DatePipe,
    public activeRouter: ActivatedRoute,
    public route: Router,
    public languageService: LanguageService
  ) {}

    //匯出
    exportFactory(url: string, nameParam: string, params: Pagination) {
      this.spinner.show();
      this.http
        .post( url,params,
          { responseType: "blob" }
        )
        .subscribe((result: Blob) => {
          if (result.type !== "application/xlsx") {
            alert(result.type);
            this.spinner.hide();
          }
          const blob = new Blob([result]);
          const url = window.URL.createObjectURL(blob);
          const link = document.createElement("a");
          const currentTime = new Date();
          const filename =
          nameParam +
            currentTime.getFullYear().toString() +
            (currentTime.getMonth() + 1) +
            currentTime.getDate() +
            currentTime
              .toLocaleTimeString()
              .replace(/[ ]|[,]|[:]/g, "")
              .trim() +
            ".xlsx";
          link.href = url;
          link.setAttribute("download", filename);
          document.body.appendChild(link);
          link.click();
          this.spinner.hide();
        });
    }

  getToDay() {
    const toDay =
      new Date().getFullYear().toString() +
      "/" +
      (new Date().getMonth() + 1).toString() +
      "/" +
      new Date().getDate().toString();
    return toDay;
  }

  getDateFormat(day: Date) {
    const dateFormat =
      day.getFullYear().toString() +
      "/" +
      (day.getMonth() + 1).toString() +
      "/" +
      day.getDate().toString();
    return dateFormat;
  }
}
