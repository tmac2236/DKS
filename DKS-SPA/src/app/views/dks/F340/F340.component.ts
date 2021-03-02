import { Component, OnInit } from "@angular/core";
import { Utility } from "../../../core/utility/utility";
import { DksService } from "../../../core/_services/dks.service";
import { JwtHelperService } from "@auth0/angular-jwt";
import { F340Schedule } from "../../../core/_models/f340-schedule.ts";
import { SF340Schedule } from "../../../core/_models/s_f340-schedule";
import { PaginatedResult } from "../../../core/_models/pagination";
import { DatePipe } from "@angular/common";

@Component({
  selector: "app-F340",
  templateUrl: "./F340.component.html",
  styleUrls: ["./F340.component.scss"],
})
export class F340Component implements OnInit {
  //getUserName
  jwtHelper = new JwtHelperService();
  loginUser: string;
  //for sorting ; ASC DESC
  cwaDeadlineS = true;


  sF340Schedule: SF340Schedule = new SF340Schedule();
  result: F340Schedule[];
  bpVerList: string[];
  constructor(public utility: Utility, private dksService: DksService) {}

  ngOnInit() {
    this.setAccount();
    this.sF340Schedule.cwaDate = this.utility.datepiper.transform(new Date(), 'yyyy-MM-dd');
    //init javascript start
    (function hello() {
      console.log("Hello Init hello() !!!");
    })();
    //init javascript start
  }
  //取得登入帳號
  setAccount(){
    const jwtTtoken = localStorage.getItem("token");
    if (jwtTtoken) {
      this.loginUser = this.jwtHelper.decodeToken(jwtTtoken)["unique_name"];
    }
  }
  //分頁按鈕
  pageChangeds(event: any): void {
    this.sF340Schedule.currentPage = event.page;
    this.search();
  }
  //排序按鈕 pending......
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
  //搜尋
  search() {
    this.utility.spinner.show();
    this.dksService.searchF340Process(this.sF340Schedule).subscribe(
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
  export(){
    const url =this.utility.baseUrl +"dks/exportF340_Process"
    this.utility.exportFactory(url,"F340_Schedule",this.sF340Schedule);
  }

  //下拉選單帶出版本
  changeBPVerList() {
    this.dksService.searchBPVerList(this.sF340Schedule.season).subscribe(
      (res) => {
        this.bpVerList = res;
      },
      (error) => {
        this.utility.alertify.error(error);
      }
    );
  }
}
