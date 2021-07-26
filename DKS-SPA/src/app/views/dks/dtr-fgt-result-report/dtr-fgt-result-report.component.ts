import { Component, OnInit } from '@angular/core';
import { Utility } from '../../../core/utility/utility';
import { utilityConfig } from '../../../core/utility/utility-config';
import { DevDtrFgtResultDto } from '../../../core/_models/dev-dtr-fgt-result-dto';
import { PaginatedResult } from '../../../core/_models/pagination';
import { SDevDtrFgtResultReport } from '../../../core/_models/s-dev-dtr-fgt-result-report';
import { DksService } from '../../../core/_services/dks.service';
import { DtrService } from '../../../core/_services/dtr.service';

@Component({
  selector: 'app-dtr-fgt-result-report',
  templateUrl: './dtr-fgt-result-report.component.html',
  styleUrls: ['./dtr-fgt-result-report.component.scss']
})
export class DtrFgtResultReportComponent implements OnInit {

  uiControls: any = {
    uploadPicF340Ppd: utilityConfig.RolePpdPic,
    uploadPdfF340Ppd: utilityConfig.RolePpdPic,
    editMemo: utilityConfig.RoleSysAdm,
    sendMemoMail: utilityConfig.RoleSysAdm,
  };
  sDevDtrFgtResultReport: SDevDtrFgtResultReport = new SDevDtrFgtResultReport();
  result: DevDtrFgtResultDto[] = [];

  constructor(
    public utility: Utility,
    private dksService: DksService,
    private dtrService: DtrService
  ) {}

  ngOnInit() {
    this.utility.initUserRole(this.sDevDtrFgtResultReport);
  }

  //搜尋
  async search() {

      this.utility.spinner.show();
      this.dtrService.getDevDtrFgtResultReport(this.sDevDtrFgtResultReport).subscribe(
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
  
  export() {
    const url = this.utility.baseUrl + "dtr/exportDevDtrFgtResultReport";
    this.utility.exportFactory(url, "DTR_FGT_Result_Report", this.sDevDtrFgtResultReport);
  }
  

  viewPdf(item: DevDtrFgtResultDto) {
    let dataUrl =
      "../assets/F340PpdPic/QCTestResult/" + item.article + "/" + item.fileName;
    window.open(dataUrl);
  }
}
