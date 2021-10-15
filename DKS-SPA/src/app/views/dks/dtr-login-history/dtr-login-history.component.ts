import { Component, OnInit } from '@angular/core';
import { Utility } from '../../../core/utility/utility';
import { F340Schedule } from '../../../core/_models/f340-schedule.ts';
import { PaginatedResult } from '../../../core/_models/pagination';
import { SF340Schedule } from '../../../core/_models/s_f340-schedule';
import { CommonService } from '../../../core/_services/common.service';
import { DksService } from '../../../core/_services/dks.service';

@Component({
  selector: 'app-dtr-login-history',
  templateUrl: './dtr-login-history.component.html',
  styleUrls: ['./dtr-login-history.component.scss']
})
export class DtrLoginHistoryComponent implements OnInit {

  title = "Dtr Login History";
  sF340Schedule: SF340Schedule = new SF340Schedule();
  result: F340Schedule[] = [];
  constructor(public utility: Utility,
               private dksService: DksService, private commonService: CommonService
               ) {}

  ngOnInit() {
    this.utility.initUserRole(this.sF340Schedule);
  }
  //分頁按鈕
  pageChangeds(event: any): void {
    this.sF340Schedule.currentPage = event.page;
    this.search();
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

}
