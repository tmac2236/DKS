import { Component, OnInit } from "@angular/core";
import { Utility } from "../../../core/utility/utility";
import { utilityConfig } from "../../../core/utility/utility-config";
import { DevDtrFgtResultDto } from "../../../core/_models/dev-dtr-fgt-result-dto";
import { DtrLoginHistoryDto } from "../../../core/_models/dtr-login-history-dto";
import { PaginatedResult } from "../../../core/_models/pagination";
import { SDevDtrFgtResultReport } from "../../../core/_models/s-dev-dtr-fgt-result-report";
import { DtrService } from "../../../core/_services/dtr.service";

@Component({
  selector: "app-dtr-fgt-result-report",
  templateUrl: "./dtr-fgt-result-report.component.html",
  styleUrls: ["./dtr-fgt-result-report.component.scss"],
})
export class DtrFgtResultReportComponent implements OnInit {
  uiControls: any = {
    uploadPicF340Ppd: utilityConfig.RolePpdPic,
  };
  title = "LAB Test Report";
  sDevDtrFgtResultReport: SDevDtrFgtResultReport = new SDevDtrFgtResultReport();
  result: DevDtrFgtResultDto[] = [];

  constructor(public utility: Utility, private dtrService: DtrService) {}

  ngOnInit() {
    this.utility.initUserRole(this.sDevDtrFgtResultReport);
    if (this.sDevDtrFgtResultReport.factoryId == "2") {
      this.sDevDtrFgtResultReport.factory = utilityConfig.factory; //事業部帳號預設翔鴻程
    } else {
      this.sDevDtrFgtResultReport.factory =
        this.sDevDtrFgtResultReport.factoryId;
    }
    this.loginReord();
  }

  //搜尋
  async search() {
    this.utility.spinner.show();
    this.dtrService
      .getDevDtrFgtResultReport(this.sDevDtrFgtResultReport)
      .subscribe(
        (res: PaginatedResult<DevDtrFgtResultDto[]>) => {
          if (res.result.length < 1) {
            this.utility.alertify.confirm(
              "Sweet Alert",
              "No Data in these conditions of search, please try again.",
              () => {
                this.utility.spinner.hide();
                return;
              }
            );
          }
          this.result = res.result;
          this.sDevDtrFgtResultReport.setPagination(res.pagination);
          this.utility.spinner.hide();
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
  //分頁按鈕
  pageChangeds(event: any): void {
    this.sDevDtrFgtResultReport.currentPage = event.page;
    this.search();
  }
  export() {
    const url = this.utility.baseUrl + "dtr/exportDevDtrFgtResultReport";
    let fileName ="error";

    if (this.sDevDtrFgtResultReport.reportType == "Dev") {
      fileName  = "DTR_FGT_Report_Dev"
    } else if (this.sDevDtrFgtResultReport.reportType == "Buy Plan") {
      fileName  = "DTR_FGT_Report_BuyPlan"
    } 
    this.utility.exportFactory(
      url,
      fileName,
      this.sDevDtrFgtResultReport
    );
  }

  viewPdf(item: DevDtrFgtResultDto) {
    let dataUrl =
      "../assets/F340PpdPic/QCTestResult/" + item.article + "/" + item.fileName;
    window.open(dataUrl);
  }
  checkSearchValid() {
    let disable = true;
    if (this.sDevDtrFgtResultReport.reportType == "Dev") {
      if (!this.utility.checkIsNullorEmpty(this.sDevDtrFgtResultReport.devSeason)) {
        disable = false;
      }
    } else if (this.sDevDtrFgtResultReport.reportType == "Buy Plan") {
      if (!this.utility.checkIsNullorEmpty(this.sDevDtrFgtResultReport.buyPlanSeason)) {
        disable = false;
      } 
    } 
    return disable;
  }
  loginReord(){
    var record = new DtrLoginHistoryDto();
    record.systemName = this.title;
    record.account = this.sDevDtrFgtResultReport?.loginUser;
    if (this.utility.checkIsNullorEmpty(record.account)){
      return;
    }
    this.dtrService.dtrLoginHistory(record).subscribe(
      (res) => {
        console.log("Add a DtrLoginReord :" + record.account);
      },
      (error) => {
        this.utility.alertify.error(error);
      }
    );
  }
}
