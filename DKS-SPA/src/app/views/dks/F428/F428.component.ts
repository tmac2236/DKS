import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";

import { Utility } from "../../../core/utility/utility";
import { F428SampleNoDetail } from "../../../core/_models/f428-sample-no-detail";
import { PaginatedResult } from "../../../core/_models/pagination";
import { SF428SampleNoDetail } from "../../../core/_models/s-f428-sample-no-detail";
import { WarehouseService } from "../../../core/_services/warehouse.service";
import { F428Commuter } from "./f428-commuter";

@Component({
  selector: "app-F428",
  templateUrl: "./F428.component.html",
  styleUrls: ["./F428.component.scss"],
})
export class F428Component implements OnInit {

  sF428SampleNoDetail: SF428SampleNoDetail = new SF428SampleNoDetail();
  result: F428SampleNoDetail[]=[];

  //params
  urlParams: F428Commuter;

  constructor(
    public utility: Utility,
    private warehouseService: WarehouseService,
    private activeRouter: ActivatedRoute,
    private route: Router
  ) {    
      this.activeRouter.queryParams.subscribe((params) => {
      this.urlParams = new F428Commuter(params.sampleNo,params.materialNo,params.actionCode);
    });
  }

  ngOnInit() {

    if(this.urlParams.actionCode =='Return'){
      this.sF428SampleNoDetail.sampleNo = this.urlParams.sampleNo;
      this.search();
    } 
    
    this.sF428SampleNoDetail.loginUser = this.utility.getAccount();
  }
  //分頁按鈕
  pageChangeds(event: any): void {
    this.sF428SampleNoDetail.currentPage = event.page;
    this.search();
  }

  search() {
    this.utility.spinner.show();
    this.warehouseService
      .getMaterialNoBySampleNoForWarehouse(this.sF428SampleNoDetail)
      .subscribe(
        (res: PaginatedResult<F428SampleNoDetail[]>) => {
          this.result = res.result;
          this.sF428SampleNoDetail.setPagination(res.pagination);
          this.utility.spinner.hide();
          if(res.result.length < 1){
            this.utility.alertify.confirm(
              "Sweet Alert",
              "No Data in these conditions of search, please try again.",
              () => {});
          }
        },
        (error) => {
          this.utility.spinner.hide();
          this.utility.alertify.error(error);
        }
      );
  }
  edit(model: F428SampleNoDetail) {
    var navigateTo = "/F428-edit";
    var navigationExtras = {
      queryParams: {
        sampleNo: this.sF428SampleNoDetail.sampleNo,
        materialNo: model.materialNo,
        actionCode:"Edit"
      },
      skipLocationChange: true,
    };
    this.route.navigate([navigateTo], navigationExtras);
  }
}
