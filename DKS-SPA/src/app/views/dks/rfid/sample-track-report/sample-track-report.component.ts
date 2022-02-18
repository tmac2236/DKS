import { Component, OnInit, ViewChild } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { environment } from '../../../../../environments/environment';
import { Utility } from '../../../../core/utility/utility';
import { utilityConfig } from '../../../../core/utility/utility-config';
import { F340SchedulePpd } from '../../../../core/_models/f340-schedule-ppd';
import { PaginatedResult } from '../../../../core/_models/pagination';
import { SampleTrackReportDto } from '../../../../core/_models/sample-track-report-dto';
import { SSampleTrackReport } from '../../../../core/_models/s_sample-track-report';
import { CommonService } from '../../../../core/_services/common.service';
import { DtrService } from '../../../../core/_services/dtr.service';

@Component({
  selector: 'app-sample-track-report',
  templateUrl: './sample-track-report.component.html',
  styleUrls: ['./sample-track-report.component.scss']
})
export class SampleTrackReportComponent implements OnInit {
  title = "Sample-Track-Report";
  sSampleTrackReport: SSampleTrackReport = new SSampleTrackReport();
  result: SampleTrackReportDto[] = [];

  uiControls:any = {
    uploadPicF340Ppd: utilityConfig.RolePpdPic,
    uploadPdfF340Ppd: utilityConfig.RolePpdPic,
    editMemo: utilityConfig.RoleSysAdm,
    sendMemoMail: utilityConfig.RoleSysAdm,
    upBottomMaintain: utilityConfig.RolePpdPic,
  };

  constructor(public utility: Utility, private dtrService: DtrService, private commonService: CommonService) {}

  ngOnInit() {
    this.utility.initUserRole(this.sSampleTrackReport);
    if(this.sSampleTrackReport.factoryId =='2'){
      this.sSampleTrackReport.factory = utilityConfig.factory; //事業部帳號預設翔鴻程
    }else{
      this.sSampleTrackReport.factory = this.sSampleTrackReport.factoryId;
    }
    this.search();
    //this.sF340PpdSchedule.loginUser = this.utility.getToken("unique_name");
  }
  //分頁按鈕
  pageChangeds(event: any): void {
    this.sSampleTrackReport.currentPage = event.page;
    this.search();
  }
  changePageSize(event: any){
    
    this.search();
  }
  //搜尋
  search() {
    this.utility.spinner.show();
    
    this.dtrService.getSampleTrackDto(this.sSampleTrackReport).subscribe(
      (res: PaginatedResult<SampleTrackReportDto[]>) => {

        this.result = res.result;
        this.sSampleTrackReport.setPagination(res.pagination);
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
    const url = this.utility.baseUrl + "dtr/exportSampleTrackDto";
    this.utility.exportFactory(url, "SampleTrackReport", this.sSampleTrackReport);
  }

  sendMail(event: any){
    this.utility.alertify.confirm(
      "System Alert",
      "Are you sure to Send Email to PIC.    It may take 10 minutes to precess.     Please DO NOT send Mail frequently.",
      () => {
        //this.utility.spinner.show();
        event.target.disabled = true;
        this.dtrService.sentMailSampleTrack().subscribe(
          (res) => {
            //this.utility.spinner.hide();
            var str = res.join(", "); 
            event.target.disabled = false;
            this.utility.alertify.confirm(
              "Sweet Alert",
              "You'd sent mail to: " + str,
              () => { });  
          },
          (error) => {
            //this.utility.spinner.hide();
            event.target.disabled = false;
            this.utility.alertify.error(error);
          }
        );        
      }
    );

  }
}
