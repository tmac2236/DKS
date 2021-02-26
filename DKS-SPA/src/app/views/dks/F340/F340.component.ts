import { Component, OnInit } from "@angular/core";
import { Utility } from "../../../core/utility/utility";
import { DksService } from "../../../core/_services/dks.service";
import { JwtHelperService } from "@auth0/angular-jwt";
import { F340Schedule } from "../../../core/_models/f340-schedule.ts";
import { SF340Schedule } from "../../../core/_models/s_f340-schedule";
import { PaginatedResult } from "../../../core/_models/pagination";

@Component({
  selector: "app-F340",
  templateUrl: "./F340.component.html",
  styleUrls: ["./F340.component.scss"],
})
export class F340Component implements OnInit {
  //for sorting ; ASC DESC
  cwaDeadlineS = true;
  //
  loginUser: string;
  
  sF340Schedule: SF340Schedule = new SF340Schedule();
  result: F340Schedule[];
  bpVerList: string[];

  jwtHelper = new JwtHelperService();
  constructor(private utility: Utility, private dksService: DksService) {}

  ngOnInit() {
    const jwtTtoken = localStorage.getItem("token");
    if (jwtTtoken) {
      this.loginUser = this.jwtHelper.decodeToken(jwtTtoken)["unique_name"];
    }
    //init javascript start
    (function hello() {
      console.log('Hello Init hello() !!!');
    })()
    //init javascript start
  }
  search() {
    this.utility.spinner.show();
    this.dksService.searchF340Processs(this.sF340Schedule).subscribe(
      (res: PaginatedResult<F340Schedule[]>) => {
        this.result = res.result;
        this.sF340Schedule.setPagination(res.pagination);
        this.utility.spinner.hide();
      },
      (error) => {
        this.utility.spinner.hide();
        this.utility.alertify.error(error);
      }
    );
  }
  //分頁按鈕
  pageChangeds(event: any): void {
    this.sF340Schedule.currentPage = event.page;
    this.search();
  }
  //排序按鈕
  sort(e) {
    //console.log(e);
    let headStr = e.target.innerHTML;

    if (this.cwaDeadlineS) {
      //ASC
      this.result = this.result.sort((a, b) =>
        a[headStr].localeCompare(b[headStr])
      );
    } else {
      //DESC
      this.result = this.result.sort((a, b) =>
        b[headStr].localeCompare(a[headStr])
      );
    }
    this.cwaDeadlineS = !this.cwaDeadlineS;

    //type = type =="ASC"?"DESC":"ASC";
  }
  //匯出
  export() {
    this.utility.spinner.show();
    this.utility.http
      .get(
        this.utility.baseUrl +
          "dks/exportF340_Process?season=" +
          this.sF340Schedule.season +
          "&bpVer=" +
          this.sF340Schedule.bpVer,
        { responseType: "blob" }
      )
      .subscribe((result: Blob) => {
        if (result.type !== "application/xlsx") {
          alert(result.type);
          this.utility.spinner.hide();
        }
        const blob = new Blob([result]);
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement("a");
        const currentTime = new Date();
        const filename =
          "F340_Schedule" +
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
        this.utility.spinner.hide();
      });
  }
  //下拉選單帶出版本
  changeBPVerList(){
    this.dksService.searchBPVerList(this.sF340Schedule.season).subscribe(
      (res) => {
        this.bpVerList = res;
      },
      (error) => {
        this.utility.alertify.error(error);
      }
    );
  }
  //設定語言
  useLanguage(language: string) {
    this.utility.languageService.setLang(language);
  }
}
