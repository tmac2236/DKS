import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { JwtHelperService } from "@auth0/angular-jwt";
import { TranslateService } from "@ngx-translate/core";
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

    sF428SampleNoDetail: SF428SampleNoDetail = new SF428SampleNoDetail();
    result: StockDetailByMaterialNo[];
    //params
    urlParams:F428Commuter;

    optionMap: Map<string,string>;
    stockNoList:Array<string> = [];

  constructor(public utility: Utility, private warehouseService: WarehouseService,private activeRouter: ActivatedRoute,
    private route: Router,private translate: TranslateService) { 
    
    this.activeRouter.queryParams.subscribe((params) => {
      
    this.urlParams = new F428Commuter(params.sampleNo,params.materialNo,params.actionCode);
  });}
  ngOnInit() {
    //get optionList
     this.translate.get('F428.optionList').subscribe((data:Map<string,string>)=> {
      this.optionMap = data ;
     });

    if(this.urlParams.actionCode =='Edit'){
      this.sF428SampleNoDetail.sampleNo  = this.urlParams.sampleNo;
      this.sF428SampleNoDetail.materialNo = this.urlParams.materialNo;
      this.search();
    } 

    this.sF428SampleNoDetail.loginUser = this.utility.getAccount();
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

  checkElement(e, stockNo) {

    if(e.target.checked){
      this.stockNoList.push(stockNo);
    }else{
      const index = this.stockNoList.indexOf(stockNo, 0);
      if(index > -1){
        this.stockNoList.splice(index,1);
      }

    }
  }
  save(){
    //一定要選狀態才能儲存
    if(this.sF428SampleNoDetail.status == "0" ){
      this.utility.alertify.confirm(
      "Sweet Alert",
      "Please select Check Status !",
      () => { });   
       return ;
    }
    let stockNostring =this.stockNoList.toString();
    stockNostring = stockNostring.replace(",","/");
    //console.log(stockNostring);
    this.sF428SampleNoDetail.chkStockNo = stockNostring;

    this.warehouseService.addStockDetailByMaterialNo(this.sF428SampleNoDetail).subscribe(
      (res) => {
        this.utility.spinner.hide();
        this.utility.alertify.confirm(
          "Sweet Alert",
          "Update Success !",
          () => { });  
      },
      (error) => {
        this.utility.spinner.hide();
        this.utility.alertify.error(error);
      }
    );
  }
}
