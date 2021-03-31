import { Component, OnDestroy, OnInit } from "@angular/core";
import { Utility } from "../../../core/utility/utility";
import { utilityConfig } from "../../../core/utility/utility-config";
import { DksService } from "../../../core/_services/dks.service";
import { F340Schedule } from "../../../core/_models/f340-schedule.ts";
import { SF340Schedule } from "../../../core/_models/s_f340-schedule";
import { PaginatedResult } from "../../../core/_models/pagination";

@Component({
  selector: "app-F340",
  templateUrl: "./F340.component.html",
  styleUrls: ["./F340.component.scss"],
})
export class F340Component implements OnInit,OnDestroy   {

  //for sorting ; ASC DESC
  cwaDeadlineS = true;


  sF340Schedule: SF340Schedule = new SF340Schedule();
  result: F340Schedule[] = [];
  bpVerList: string[];
  constructor(public utility: Utility, private dksService: DksService) {}

  ngOnInit() {
    console.log((<HTMLInputElement>document.getElementById("idDataType")));
    this.sF340Schedule.loginUser = this.utility.getAccount();
    //this.sF340Schedule.cwaDateS = this.utility.datepiper.transform(new Date(), 'yyyy-MM-dd');

    console.log("Hello Init hello() !!!");
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
    if(this.sF340Schedule.dataType =="FHO"&&this.sF340Schedule.bpVer ==""){
      this.utility.alertify.confirm(
        "Sweet Alert",
        "Please choose Buy Plan before click !",
        () => {}
      );
      return;
    }
    this.utility.spinner.show();
    this.dksService.searchF340Process(this.sF340Schedule).subscribe(
      (res: PaginatedResult<F340Schedule[]>) => {
        this.result = res.result;
        this.sF340Schedule.setPagination(res.pagination);
        this.utility.spinner.hide();
        if (res.result.length < 1) {
          this.utility.alertify.confirm(
            "Sweet Alert",
            "No Data in these conditions of search, please try again.",
            () => {}
          );
        }
      },
      (error) => {
        this.utility.spinner.hide();
        this.utility.alertify.confirm(
          "System Notice",
          "Syetem is busy, please try later.",
          () => {}
        );
      }
    );
  }
  export(){
    if(this.sF340Schedule.dataType =="FHO"&&this.sF340Schedule.bpVer ==""){
      this.utility.alertify.confirm(
        "Sweet Alert",
        "Please choose Buy Plan before click !",
        () => {}
      );
      return;
    }
    const url =this.utility.baseUrl +"dks/exportF340_Process";
    this.utility.exportFactory(url,"F340_Schedule",this.sF340Schedule);
  }

  //下拉選單帶出版本
  changeBPVerList() {
    if(this.sF340Schedule.season ==="") return;
    this.utility.spinner.show();
    this.dksService.searchBPVerList(this.sF340Schedule.season,this.sF340Schedule.factory).subscribe(
      (res) => {
        this.utility.spinner.hide();
        this.bpVerList = res;
      },
      (error) => {
        this.utility.spinner.hide();
        this.utility.alertify.confirm(
          "System Notice",
          "Syetem is busy, please try later.",
          () => {}
        );
      }
    );
  }
  cleanBP(){
    this.bpVerList = [];
    this.sF340Schedule.bpVer = "";
  }
  ngOnDestroy(): void {
    
  }
}
