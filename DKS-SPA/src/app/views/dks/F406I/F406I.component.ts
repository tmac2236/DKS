import { Component, OnInit } from '@angular/core';
import { Utility } from '../../../core/utility/utility';
import { utilityConfig } from '../../../core/utility/utility-config';
import { BasicCodeDto } from '../../../core/_models/basic-code-dto';
import { DtrFgtEtdDto } from '../../../core/_models/dtr-fgt-etd-dto';
import { F406iDto } from '../../../core/_models/f406i-dto';
import { PaginatedResult } from '../../../core/_models/pagination';
import { SDtrFgtShoes } from '../../../core/_models/s_dtr_fgt_shoes';
import { SF406i } from '../../../core/_models/s_f406i';
import { CommonService } from '../../../core/_services/common.service';
import { DtrService } from '../../../core/_services/dtr.service';
import { WarehouseService } from '../../../core/_services/warehouse.service';

@Component({
  selector: 'app-F406I',
  templateUrl: './F406I.component.html',
  styleUrls: ['./F406I.component.scss']
})
export class F406IComponent implements OnInit {

  title = "F406I";
  sF406i: SF406i = new SF406i();
  result: F406iDto[] = [];
  uiControls:any = {
    editMemo: utilityConfig.RoleFgtLabReport,
    editMemo1: utilityConfig.DtrQcSup
  };
  
  constructor(public utility: Utility, private dtrService: DtrService, private warehouseService: WarehouseService, private commonService: CommonService) {}

  async ngOnInit() {
    this.utility.initUserRole(this.sF406i);
  }
  //分頁按鈕
  pageChangeds(event: any): void {
    this.sF406i.currentPage = event.page;
    this.search();
  }
  changePageSize(event: any){
    this.search();
  }
  //搜尋
  search() {
    this.utility.spinner.show();
    
    this.warehouseService.searchF406i(this.sF406i).subscribe(
      (res: PaginatedResult<F406iDto[]>) => {
        this.result = res.result;        
        this.sF406i.setPagination(res.pagination);
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
  scanStockNo(){
    if(this.sF406i.stockNo.length == 8){
      this.search() ;
    }else{
      this.utility.alertify.error("The length of Stock# have to be 6 !");
    }

  }
  //export() {
  //  const url = this.utility.baseUrl + "dtr/exportDtrFgtEtdDto";
  //  this.utility.exportFactory(url, "DtrFgtEtdReport", this.sF406i);
  //}

}
