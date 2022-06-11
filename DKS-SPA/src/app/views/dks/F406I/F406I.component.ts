import { Component, OnInit } from "@angular/core";
import { Utility } from "../../../core/utility/utility";
import { utilityConfig } from "../../../core/utility/utility-config";
import { BasicCodeDto } from "../../../core/_models/basic-code-dto";
import { DtrFgtEtdDto } from "../../../core/_models/dtr-fgt-etd-dto";
import { F406iDto } from "../../../core/_models/f406i-dto";
import { F434Dto } from "../../../core/_models/f434-dto";
import { PaginatedResult } from "../../../core/_models/pagination";
import { SDtrFgtShoes } from "../../../core/_models/s_dtr_fgt_shoes";
import { SF406i } from "../../../core/_models/s_f406i";
import { CommonService } from "../../../core/_services/common.service";
import { DtrService } from "../../../core/_services/dtr.service";
import { WarehouseService } from "../../../core/_services/warehouse.service";

@Component({
  selector: "app-F406I",
  templateUrl: "./F406I.component.html",
  styleUrls: ["./F406I.component.scss"],
})
export class F406IComponent implements OnInit {
  title = "F406I";
  sF406i: SF406i = new SF406i();
  result: F406iDto[] = [];
  resultM: F434Dto[] = []; //Materail No : GetF434Dto

  constructor(
    public utility: Utility,
    private warehouseService: WarehouseService,
  ) {}

  async ngOnInit() {
    this.utility.initUserRole(this.sF406i);
  }
  //搜尋
  search(type: Type) {
    this.utility.spinner.show();

    if (type == Type.stockNo) {
      this.warehouseService.searchF406i(this.sF406i).subscribe(
        (res: PaginatedResult<F406iDto[]>) => {
          this.result = res.result;
          this.sF406i.setPagination(res.pagination);
          this.sF406i.stockNo = ""; //clear
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
    } else if (type == Type.materialNo){
      this.warehouseService.searchF434(this.sF406i).subscribe(
        (res: PaginatedResult<F434Dto[]>) => {
          this.resultM = res.result;
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
  }
  scanStockNo() {
    if (this.sF406i.stockNo.length == 8) {
      this.search(Type.stockNo);
    }
  }
  scanMaterialNo() {
    if (this.sF406i.materialNo.length == 8) {
      this.search(Type.materialNo);
    }
  }

}

export enum Type {
  stockNo,
  materialNo,
}
