import { DatePipe } from "@angular/common";
import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { JwtHelperService } from "@auth0/angular-jwt";
import { NgxSpinnerService } from "ngx-spinner";
import { timeout } from "rxjs/operators";
import { environment } from "../../../environments/environment";
import { Pagination } from "../_models/pagination";
import { AlertifyService } from "../_services/alertify.service";
import { LanguageService } from "../_services/language.service";
import { utilityConfig } from "./utility-config";

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
      .post(url, params, { responseType: "blob" })
      .pipe(timeout(utilityConfig.httpTimeOut))
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
  getCurrentLang() {
    return this.languageService.translate.currentLang;
  }
  //設定語言
  useLanguage(language: string) {
    this.languageService.setLang(language);
  }
  //設定是否分頁
  setPagination(bo: boolean, objS: Pagination) {
    let powerStr = "on";
    if (!bo) powerStr = "off";
    this.alertify.confirm(
      "Sweet Alert",
      "You just turned " + powerStr + " the pagination mode.",
      () => {
        objS.isPaging = bo;
      }
    );
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

  getToken(key: string) {
    const jwtTtoken = localStorage.getItem("token");
    if (jwtTtoken) {
      return this.jwtHelper.decodeToken(jwtTtoken)[key];
    }
    return "";
  }
  //check max file
  //e.g  maxValue: 1128659 = 1MB
  checkFileMaxFormat(file: File, maxVal: number, type: string) {
    var isLegal = true;
    if (file.type != type) isLegal = false;
    if (file.size >= maxVal) isLegal = false; //最大上傳1MB
    return isLegal;
  }
  initUserRole(s: Pagination) {
    s.loginUser = this.getToken("unique_name");
    s.role = this.getToken("role");
    s.userId = this.getToken("nameid");
    return s;
  }
  //Base64 to Blob
  dataURLToBlob(dataURL) {
    // Code taken from https://github.com/ebidel/filer.js
    const parts = dataURL.split(";base64,");
    const contentType = parts[0].split(":")[1];
    const raw = window.atob(parts[1]);
    const rawLength = raw.length;
    const uInt8Array = new Uint8Array(rawLength);
    for (let i = 0; i < rawLength; ++i) {
      uInt8Array[i] = raw.charCodeAt(i);
    }
    return new Blob([uInt8Array], { type: contentType });
  }
  reset(e) {
    console.log(" utility reset function fire! ", e);
    e.target.value = "";
  }
  checkIsNumber(str: string) {
    if (isNaN(Number(str))) return false;
    return true;
  }
  checkIsDate(str: string) {
    if (this.checkIsNumber(str)) return false;
    let newDate = Date.parse(str);
    if (isNaN(newDate)) return false;
    return true;
  }
}
