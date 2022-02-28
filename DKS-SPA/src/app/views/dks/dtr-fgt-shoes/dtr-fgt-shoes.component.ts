import { Component, OnInit } from '@angular/core';
import { Utility } from '../../../core/utility/utility';
import { utilityConfig } from '../../../core/utility/utility-config';
import { BasicCodeDto } from '../../../core/_models/basic-code-dto';
import { DtrFgtEtdDto } from '../../../core/_models/dtr-fgt-etd-dto';
import { DtrFgtShoes } from '../../../core/_models/dtr-fgt-shoes';
import { PaginatedResult } from '../../../core/_models/pagination';
import { SampleTrackReportDto } from '../../../core/_models/sample-track-report-dto';
import { SDtrFgtShoes } from '../../../core/_models/s_dtr_fgt_shoes';
import { CommonService } from '../../../core/_services/common.service';
import { DtrService } from '../../../core/_services/dtr.service';

@Component({
  selector: 'app-dtr-fgt-shoes',
  templateUrl: './dtr-fgt-shoes.component.html',
  styleUrls: ['./dtr-fgt-shoes.component.scss']
})
export class DtrFgtShoesComponent implements OnInit {
  deadlineNow = new Date(new Date().getTime()); // now minus three months(- 7776000000)
  memoBtn = true;
  title = "DTR-FGT-Shoes";
  sDtrFgtShoes: SDtrFgtShoes = new SDtrFgtShoes();
  result: DtrFgtEtdDto[] = [];
  oriResult: DtrFgtEtdDto[] = [];
  uiControls:any = {
    editMemo: utilityConfig.RoleFgtLabReport,
    editMemo1: utilityConfig.DtrQcSup
  };
  code066: BasicCodeDto[] = []; //066 QC測試報告ETD的REMARK

  constructor(public utility: Utility, private dtrService: DtrService, private commonService: CommonService) {}

  async ngOnInit() {
    await this.getBasicCodeDto();
    this.utility.initUserRole(this.sDtrFgtShoes);
    this.search();
    //this.sF340PpdSchedule.loginUser = this.utility.getToken("unique_name");
  }
  //分頁按鈕
  pageChangeds(event: any): void {
    this.sDtrFgtShoes.currentPage = event.page;
    this.search();
  }
  changePageSize(event: any){
    
    this.search();
  }
  //搜尋
  search() {
    this.utility.spinner.show();
    
    this.dtrService.getDtrFgtEtdDto(this.sDtrFgtShoes).subscribe(
      (res: PaginatedResult<DtrFgtEtdDto[]>) => {
        this.result = res.result;
        this.result.map((x) => {
          //check is need warn
          if( new Date(x.qcEtd).getTime() < 
            new Date(this.deadlineNow).getTime()
          ){
            x.isWarn = true;
          }else{
            x.isWarn = false;
          }
        });
        this.oriResult = this.result;          
        this.sDtrFgtShoes.setPagination(res.pagination);
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
    const url = this.utility.baseUrl + "dtr/exportDtrFgtEtdDto";
    this.utility.exportFactory(url, "DtrFgtEtdReport", this.sDtrFgtShoes);
  }
  editMemo(){
    this.memoBtn = !this.memoBtn;
  }
  saveMemo(type:string){
    
    this.memoBtn = !this.memoBtn;
    if(type == "cancel"){
      this.search();
      return;
    }

    this.utility.spinner.show();

    
    this.dtrService.editDtrFgtEtds(this.result,this.sDtrFgtShoes.loginUser).subscribe(
      (res) => {
        this.utility.spinner.hide();
        this.utility.alertify.success("Save success !");
        this.search(); 
      },
      (error) => {
        this.utility.spinner.hide();
        this.utility.alertify.error(error);
      }
    );
     
  }
  filterResult(){
    if(this.sDtrFgtShoes.article.trim() == ""){
      this.result = this.oriResult;
    }else{
      this.result = this.oriResult.filter(x=>x.article.includes(this.sDtrFgtShoes.article) );
    }

  }
    async getBasicCodeDto() {
      if(!this.utility.checkIsNullorEmpty(this.code066)) return;
      await this.commonService
        .getBasicCodeDto(
          '066'
        )
        .then(
          (res) => {
            this.code066 = res
          },
          (error) => {
            this.utility.alertify.confirm(
              "System Notice",
              "Syetem is busy, please try later.",
              () => {}
            );
          }
        );
    }
}
