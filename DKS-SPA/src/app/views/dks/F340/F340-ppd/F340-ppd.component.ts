import { Component, OnInit } from "@angular/core";
import { Utility } from "../../../../core/utility/utility";
import { utilityConfig } from "../../../../core/utility/utility-config";
import { DksService } from "../../../../core/_services/dks.service";
import { PaginatedResult } from "../../../../core/_models/pagination";
import { SF340PpdSchedule } from "../../../../core/_models/s_f340-ppd-schedule";
import { F340SchedulePpd } from "../../../../core/_models/f340-schedule-ppd";

@Component({
  selector: "app-F340",
  templateUrl: "./F340-ppd.component.html",
  styleUrls: ["./F340-ppd.component.scss"],
})
export class F340PpdComponent implements OnInit {
  //for sorting ; ASC DESC
  cwaDeadlineS = true;

  sF340PpdSchedule: SF340PpdSchedule = new SF340PpdSchedule();
  result: F340SchedulePpd[] = [];
  bpVerList: string[];
  memoBtn = true;
  constructor(public utility: Utility, private dksService: DksService) {}

  ngOnInit() {
    this.utility.initUserRole(this.sF340PpdSchedule);
    //this.sF340PpdSchedule.loginUser = this.utility.getToken("unique_name");
  }
  //分頁按鈕
  pageChangeds(event: any): void {
    this.sF340PpdSchedule.currentPage = event.page;
    this.search();
  }
  //搜尋
  search() {
    this.utility.spinner.show();
    this.dksService.searchF340PpdProcess(this.sF340PpdSchedule).subscribe(
      (res: PaginatedResult<F340SchedulePpd[]>) => {
        this.result = res.result;
        this.sF340PpdSchedule.setPagination(res.pagination);
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
  export() {
    const url = this.utility.baseUrl + "dks/exportF340_ProcessPpd";
    this.utility.exportFactory(url, "F340_PPD", this.sF340PpdSchedule);
  }

  //下拉選單帶出版本
  changeBPVerList() {
    if (this.sF340PpdSchedule.season === "") return;
    this.utility.spinner.show();
    this.dksService
      .searchBPVerList(
        this.sF340PpdSchedule.season,
        this.sF340PpdSchedule.factory
      )
      .subscribe(
        (res) => {
          this.utility.spinner.hide();
          this.bpVerList = res;
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
  cleanBP() {
    this.bpVerList = [];
    this.sF340PpdSchedule.bpVer = "";
  }

  uploadPicF340Ppd(files: FileList, model: F340SchedulePpd) {
    alert("DEMO Add!");
    return ;
    console.log(model);
    if (!this.utility.checkFileMaxFormat(files.item(0), 1128659)) {
      this.utility.alertify.confirm(
        "Sweet Alert",
        "Please upload jpg file and size cannot over 1 Mb.",
        () => {}
      );
      return; //exit function
    }
    var formData = new FormData();
    formData.append("file", files.item(0));
    formData.append("sampleNo",model.sampleNo);
    formData.append("treatMent",model.treatMent);
    formData.append("partName",model.partName);

    this.dksService.editPicF340Ppd(formData).subscribe(
      () => {
        //找到該筆model 把資料回填
      },
      (error) => {
        this.utility.alertify.error("Add failed !!!!");
      },()=>{
        alert("You add one picture !");
      }
    );
  }
  removePicF340Ppd(model: F340SchedulePpd) {
    alert("DEMO Delete!");
    return ;
    var formData = new FormData(); 
    formData.append("file", null);
    formData.append("sampleNo",model.sampleNo);
    formData.append("treatMent",model.treatMent);
    formData.append("partName",model.partName);
    this.utility.alertify.confirm(
      "Sweet Alert",
      "Are you sure to Delete this picture ?",
      () => {
        this.dksService.editPicF340Ppd(formData).subscribe(
          () => {
            location.reload();
            this.utility.alertify.success("Delete succeed!");
          },
          (error) => {
            this.utility.alertify.error("Delete failed !!!!");
          }
        );
      }
    );
  }
  viewPic(model: F340SchedulePpd){
    alert("DEMO click viewPic : " + model.article);
    window.open('http://10.4.0.39:6970/assets/systemImg/exit.png');
  }
  editMemo(){
    this.memoBtn = !this.memoBtn;
  }
  saveMemo(){
    this.memoBtn = !this.memoBtn;
    this.dksService.editF340Ppd(this.result).subscribe(
      (res) => {
        this.utility.spinner.hide();
        this.utility.alertify.confirm(
          "Sweet Alert",
          "Update Success !",
          () => { });  
      },
      (error) => {
        this.utility.spinner.hide();
        this.utility.alertify.error(error);
      }
    );
  }
}
