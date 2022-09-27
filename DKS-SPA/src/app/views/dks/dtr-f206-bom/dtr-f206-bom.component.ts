import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { Utility } from "../../../core/utility/utility";
import { DtrF206Bom } from "../../../core/_models/dtr-f206-bom";
import { PaginatedResult } from "../../../core/_models/pagination";
import { SDtrF206Bom } from "../../../core/_models/s-dtr-206-bom";
import { DtrService } from "../../../core/_services/dtr.service";

@Component({
  selector: "app-dtr-f206-bom",
  templateUrl: "./dtr-f206-bom.component.html",
  styleUrls: ["./dtr-f206-bom.component.scss"],
})
export class DtrF206BomComponent implements OnInit {

  sDtrF206Bom: SDtrF206Bom = new SDtrF206Bom();
  title = "Dtr F206 Bom";
  result: DtrF206Bom[] = [];

  constructor(
    public utility: Utility,
    private activeRouter: ActivatedRoute,
    private dtrService: DtrService,
  ) {
    this.activeRouter.queryParams.subscribe((params) => {
      if (params.homeParam !== undefined) {
        //from aven excel
        this.sDtrF206Bom.article = params.homeParam;
      }
    });
  }

  ngOnInit() {
    this.search();
  }
  //搜尋
  search() {
    this.utility.spinner.show();
    this.dtrService.searchDtrF206Bom(this.sDtrF206Bom).subscribe(
      (res: PaginatedResult<DtrF206Bom[]>) => {
        this.result = res.result;
        this.sDtrF206Bom.setPagination(res.pagination);
        this.utility.spinner.hide();
        if (res.result.length < 1) {
          this.utility.alertify.confirm(
            "Sweet Alert",
            "No Data in these conditions of search, please try again.",
            () => {}
          );
        }
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
  export(){
    const url =this.utility.baseUrl +"dtr/exportDtrF206Bom";
    this.utility.exportFactory(url,"DtrF206Bom",this.sDtrF206Bom);
  }
}
