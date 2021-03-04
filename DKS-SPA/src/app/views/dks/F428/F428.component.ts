import { Component, OnInit } from '@angular/core';
import { JwtHelperService } from "@auth0/angular-jwt";
import { Utility } from '../../../core/utility/utility';
import { F428SampleNoDetail } from '../../../core/_models/f428-sample-no-detail';
import { PaginatedResult } from '../../../core/_models/pagination';
import { SF428SampleNoDetail } from '../../../core/_models/s-f428-sample-no-detail';
import { WarehouseService } from '../../../core/_services/warehouse.service';


@Component({
  selector: 'app-F428',
  templateUrl: './F428.component.html',
  styleUrls: ['./F428.component.scss']
})
export class F428Component implements OnInit {
  //getUserName
  jwtHelper = new JwtHelperService();
  loginUser: string;

  sF428SampleNoDetail: SF428SampleNoDetail = new SF428SampleNoDetail();
  result: F428SampleNoDetail[];

  constructor(public utility: Utility, private warehouseService: WarehouseService) {}

  ngOnInit() {
    this.setAccount();
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
    this.sF428SampleNoDetail.currentPage = event.page;
    this.search();
  }
  
  search(){
    this.utility.spinner.show();
    this.warehouseService.getMaterialNoBySampleNoForWarehouse(this.sF428SampleNoDetail).subscribe(
      (res: PaginatedResult<F428SampleNoDetail[]>) => {
        this.result = res.result;
        this.sF428SampleNoDetail.setPagination(res.pagination);
        this.utility.spinner.hide();
      },
      (error) => {
        this.utility.spinner.hide();
        this.utility.alertify.error(error);
      }
    );
  }
}
