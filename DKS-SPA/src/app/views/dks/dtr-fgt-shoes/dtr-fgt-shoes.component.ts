import { Component, OnInit } from '@angular/core';
import { Utility } from '../../../core/utility/utility';
import { utilityConfig } from '../../../core/utility/utility-config';
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

  memoBtn = true;
  title = "DTR-FGT-Shoes";
  sDtrFgtShoes: SDtrFgtShoes = new SDtrFgtShoes();
  result: DtrFgtEtdDto[] = [];

  uiControls:any = {
    editMemo: utilityConfig.RoleSysAdm,
  };

  constructor(public utility: Utility, private dtrService: DtrService, private commonService: CommonService) {}

  ngOnInit() {
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

    
    this.dtrService.editDtrFgtEtds(this.result).subscribe(
      (res) => {
        this.utility.spinner.hide();
        this.utility.alertify.confirm(
          "System Alert",
          "Updated Successed.",
          () => { });  
      },
      (error) => {
        this.utility.spinner.hide();
        this.utility.alertify.error(error);
      }
    );
     
  }

}
