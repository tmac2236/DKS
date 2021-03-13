import { DatePipe } from "@angular/common";
import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { JwtHelperService } from "@auth0/angular-jwt";
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
  //getUserName
  jwtHelper = new JwtHelperService();

  constructor(
    public http: HttpClient,
    public alertify: AlertifyService,
    public spinner: NgxSpinnerService,
    public datepiper: DatePipe,
    public languageService: LanguageService
  ) {}

  logout() {
    localStorage.removeItem("token");
    this.alertify.message("logged out");
  }
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
  //取得目前語言
  getCurrentLang(){
    return this.languageService.translate.currentLang ;
  }
  //設定語言
  useLanguage(language: string) {
    this.languageService.setLang(language);
  }
    //設定是否分頁
    setPagination(bo:boolean, objS : Pagination){

      let powerStr = 'on';
      if (!bo) powerStr ='off';
      this.alertify.confirm(
        "Sweet Alert",
        "You just turned "+ powerStr + " the pagination mode.",
        () => {
          objS.isPaging = bo;
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

  getAccount() {
    const jwtTtoken = localStorage.getItem("token");
    if (jwtTtoken) {
      return  this.jwtHelper.decodeToken(jwtTtoken)["unique_name"];
    }
    return "";
  }
}
