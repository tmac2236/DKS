import { Component, OnInit } from "@angular/core";
import { Utility } from "../../../../core/utility/utility";
import { ActivatedRoute } from "@angular/router";
import { SF203Copy } from "../../../../core/_models/s-f203-copy";
import { SrfArticleDto } from "../../../../core/_models/srf-article-dto";
import { DksService } from "../../../../core/_services/dks.service";

@Component({
  selector: "app-F203-copy",
  templateUrl: "./F203-copy.component.html",
  styleUrls: ["./F203-copy.component.scss"],
})
export class F203CopyComponent implements OnInit {
  constructor(
    public utility: Utility,
    private activeRouter: ActivatedRoute,
    private dksService: DksService
  ) {
    this.activeRouter.queryParams.subscribe((params) => {
      if (params.homeParam !== undefined) {
        //from DKS redirect
        this.sConditon.srfIdF = params.homeParam;
      }
    });
  }
  title = "F203 SRF Copy";
  sConditon: SF203Copy = new SF203Copy();
  result: SrfArticleDto[] = [];
  selectedList: SrfArticleDto[] = []; //checkbox用
  isAllCheck = false; //全選checkbox用
  stageList: { id: number, name: string, code: string }[] = [
    { "id": 1, "name": "CR2","code":"CR2" },
    { "id": 2, "name": "SMS","code":"SMS" },
    { "id": 3, "name": "MCS","code":"MCS" }]; 

  ngOnInit() {
    this.utility.initUserRole(this.sConditon);
    this.search();
  }
  copy() {
    this.utility.alertify.confirm(
      "System Alert",
      "Are you sure you want to copy this(these) " + this.selectedList.length + " article(s)?",
      () => {
        var formData = new FormData();
        formData.append("f1", JSON.stringify(this.sConditon));
        formData.append("f2", JSON.stringify(this.selectedList));

        this.utility.spinner.show();
        this.dksService.copySrf(formData).subscribe(
          (res) => {
            this.utility.spinner.hide();
            this.utility.alertify.success("Copy success!");
          },
          (error) => {
            this.utility.spinner.hide();
            this.utility.alertify.error(error.replace(/{|}/g, ''));
          }
        );
      }
    );
  }
  //搜尋
  search() {
    this.dksService.getSrfArticleDto(this.sConditon.srfIdF).subscribe(
      (res: SrfArticleDto[]) => {
        this.result = res;
      },
      (error) => {
        console.log(error);
      }
    );
  }
  selectList() {
    this.selectedList = this.result.filter((item) => item.checked);
    console.log(this.selectedList.length);
  }
  checkUncheckAll(e) {
    this.selectedList = []; //先清空

    this.result.forEach((i) => {
      //打勾或取消打勾
      i.checked = e.target.checked;
    });

    if (e.target.checked) {
      //如勾全選
      this.selectedList = this.result;
    }
  }
  checkCopyValid() {
    if (
      !this.utility.checkIsNullorEmpty(this.sConditon.srfIdT) &&
      !this.utility.checkIsNullorEmpty(this.sConditon.stageT) &&
       this.selectedList.length > 0
    ) {
      return false;
    } else {
      return true;
    }
  }
}
