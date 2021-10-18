import { Component, OnInit } from '@angular/core';
import { Utility } from '../../../core/utility/utility';
import { DtrLoginHistory } from '../../../core/_models/dtr-login-history';
import { F340Schedule } from '../../../core/_models/f340-schedule.ts';
import { PaginatedResult } from '../../../core/_models/pagination';
import { SDtrLoginHistory } from '../../../core/_models/s_dtr-login-history';
import { CommonService } from '../../../core/_services/common.service';
import { DksService } from '../../../core/_services/dks.service';
import { DtrService } from '../../../core/_services/dtr.service';

@Component({
  selector: 'app-dtr-login-history',
  templateUrl: './dtr-login-history.component.html',
  styleUrls: ['./dtr-login-history.component.scss']
})
export class DtrLoginHistoryComponent implements OnInit {

  title = "Dtr Login History";
  systemNameList: { id: number, name: string, code: string }[] = [
    { "id": 1, "name": "CWA_LIST","code":"CWA List" },
    { "id": 2, "name": "DTR_LIST","code":"DTR List" },
    { "id": 3, "name": "LAB Test Report Maintain","code":"LAB Test Report Maintain" }
  ]; 
  sCondition: SDtrLoginHistory = new SDtrLoginHistory();
  result: DtrLoginHistory[] = [];
  constructor(public utility: Utility,
               private dksService: DksService,private dtrService: DtrService,
              private commonService: CommonService) {}

  ngOnInit() {
    this.utility.initUserRole(this.sCondition);
  }
  //分頁按鈕
  pageChangeds(event: any): void {
    this.sCondition.currentPage = event.page;
    this.search();
  }
  //搜尋
  search() {
    this.utility.spinner.show();
    this.dtrService.searchDtrLoginHistory(this.sCondition).subscribe(
      (res: PaginatedResult<DtrLoginHistory[]>) => {
        this.result = res.result;
        this.sCondition.setPagination(res.pagination);
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
    const url =this.utility.baseUrl +"dtr/exportDtrLoginHistory";
    this.utility.exportFactory(url,this.title,this.sCondition);
  }

}
