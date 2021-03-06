import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { JwtHelperService } from "@auth0/angular-jwt";
import { Utility } from "../../../../core/utility/utility";
import { SF428SampleNoDetail } from "../../../../core/_models/s-f428-sample-no-detail";
import { StockDetailByMaterialNo } from "../../../../core/_models/stock-detail-by-material-no";
import { WarehouseService } from "../../../../core/_services/warehouse.service";
import { F428Commuter } from "../f428-commuter";
@Component({
  selector: "app-F428-edit",
  templateUrl: "./F428-edit.component.html",
  styleUrls: ["./F428-edit.component.scss"],
})
export class F428EditComponent implements OnInit {
    //getUserName
    jwtHelper = new JwtHelperService();
    loginUser: string;
    
    sF428SampleNoDetail: SF428SampleNoDetail = new SF428SampleNoDetail();
    result: StockDetailByMaterialNo[];
    //params
    urlParams:F428Commuter;

  constructor(public utility: Utility, private warehouseService: WarehouseService,private activeRouter: ActivatedRoute,
    private route: Router) { 
    
    this.activeRouter.queryParams.subscribe((params) => {
      
    this.urlParams = new F428Commuter(params.sampleNo,params.materialNo,params.actionCode);
  });}
  ngOnInit() {

    if(this.urlParams.actionCode =='Edit'){
      this.sF428SampleNoDetail.sampleNo  = this.urlParams.sampleNo;
      this.sF428SampleNoDetail.materialNo = this.urlParams.materialNo;
      this.search();
    } 

    this.setAccount();
  }
  //取得登入帳號
  setAccount() {
    const jwtTtoken = localStorage.getItem("token");
    if (jwtTtoken) {
      this.loginUser = this.jwtHelper.decodeToken(jwtTtoken)["unique_name"];
    }
  }
  search(){
    //alert(this.sF428SampleNoDetail.materialNo);
    this.utility.spinner.show();
    this.warehouseService.getStockDetailByMaterialNo(this.sF428SampleNoDetail).subscribe(
      (res) => {
        this.result = res;
        this.utility.spinner.hide();
      },
      (error) => {
        this.utility.spinner.hide();
        this.utility.alertify.error(error);
      }
    );
  }
  previousPage(){
    var navigateTo = "/F428";
    var navigationExtras = {
      queryParams: {
        sampleNo: this.urlParams.sampleNo,
        materialNo: this.urlParams.materialNo,
        actionCode:"Return"
      },
      skipLocationChange: true,
    };
    this.route.navigate([navigateTo], navigationExtras);

  }
}
