import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { environment } from "../../../../environments/environment";
import { Utility } from "../../../core/utility/utility";
import { BasicCodeDto } from "../../../core/_models/basic-code-dto";
import { DevDtrVsList } from "../../../core/_models/dev-dtr-vs-list";
import { PaginatedResult } from "../../../core/_models/pagination";
import { SDevDtrVsList } from "../../../core/_models/s-dev-dtr-vs-list";
import { CommonService } from "../../../core/_services/common.service";
import { DtrService } from "../../../core/_services/dtr.service";

@Component({
  selector: "app-dtr-v-standard-list",
  templateUrl: "./dtr-v-standard-list.component.html",
  styleUrls: ["./dtr-v-standard-list.component.scss"],
})
export class DtrVStandardListComponent implements OnInit {
  constructor(public utility: Utility, private route: Router, private dtrService: DtrService, private commonService: CommonService) {}

  sDevDtrVsList: SDevDtrVsList = new SDevDtrVsList();
  result: DevDtrVsList[] = [];
  code017: BasicCodeDto[] = [];
  
  ngOnInit() {
    this.utility.initUserRole(this.sDevDtrVsList);
    this.getBasicCodeDto();
  }
  //搜尋
  async search() {
    if (this.utility.checkIsNullorEmpty(this.sDevDtrVsList.season)) {
      this.utility.alertify.error("Season is required !!!!");
      return;
    }
    //article modleNo modelName at least one field is required
    if (
      !this.utility.checkIsNullorEmpty(this.sDevDtrVsList.modelNo) ||
      !this.utility.checkIsNullorEmpty(this.sDevDtrVsList.modelName) ||
      !this.utility.checkIsNullorEmpty(this.sDevDtrVsList.article)
    ) {
    } else {
      this.utility.alertify.error("Article、ModleNo、ModelName at least one field is required !!!!");
      return;
    }

    if (this.sDevDtrVsList.season.trim().length < 4) {
      this.utility.alertify.error("Season must be 4 characters  !!!!");
      return;
    }
    if ( !this.utility.checkIsNullorEmpty(this.sDevDtrVsList.article) && this.sDevDtrVsList.article.trim().length < 6) {
      this.utility.alertify.error("Artilce must be 6 characters  !!!!");
      return;
    }
    this.result = [];

    this.utility.spinner.show();
    this.dtrService.getDevDtrList(this.sDevDtrVsList).subscribe(
      (res: PaginatedResult<DevDtrVsList[]>) => {

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
        this.sDevDtrVsList.setPagination(res.pagination);
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
  viewRecord( model: DevDtrVsList){
    let dataUrl =
    environment.spaUrl + '/#/DTR-Vs-Maintain?season=' + model.season + "&article=" + model.article;
    window.open(dataUrl);
  }

    //check is the season article is exist in Article、Model
    async getBasicCodeDto() {
      if(!this.utility.checkIsNullorEmpty(this.code017)) return;
      await this.commonService
        .getBasicCodeDto(
          '017'
        )
        .then(
          (res) => {
            this.code017 = res
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
