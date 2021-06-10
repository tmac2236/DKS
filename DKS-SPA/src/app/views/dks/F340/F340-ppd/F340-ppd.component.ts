import { Component, OnInit, ViewChild } from "@angular/core";
import { Utility } from "../../../../core/utility/utility";
import { utilityConfig } from "../../../../core/utility/utility-config";
import { DksService } from "../../../../core/_services/dks.service";
import { PaginatedResult } from "../../../../core/_models/pagination";
import { SF340PpdSchedule } from "../../../../core/_models/s_f340-ppd-schedule";
import { F340SchedulePpd } from "../../../../core/_models/f340-schedule-ppd";
import { ModalDirective } from "ngx-bootstrap/modal";
import { environment } from "../../../../../environments/environment";

@Component({
  selector: "app-F340",
  templateUrl: "./F340-ppd.component.html",
  styleUrls: ["./F340-ppd.component.scss"],
})
export class F340PpdComponent implements OnInit {
  @ViewChild('photoCommentModal') public photoCommentModal: ModalDirective;
  @ViewChild('ppdRemarkModal') public ppdRemarkModal: ModalDirective;
  
  //for sorting ; ASC DESC
  cwaDeadlineS = true;

  sF340PpdSchedule: SF340PpdSchedule = new SF340PpdSchedule();
  result: F340SchedulePpd[] = [];
  bpVerList: string[];
  memoBtn = true;
  uiControls:any = {
    uploadPicF340Ppd: utilityConfig.DevPreAssist,
    editMemo: utilityConfig.DevPreAssist
  };
  editModel: F340SchedulePpd = new F340SchedulePpd(); //onlt use in photoCommentModal

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
  changePageSize(event: any){
    
    if(this.sF340PpdSchedule.season.length < 1){
      return;
    }
    this.search();
  }
  //搜尋
  search() {
    this.utility.spinner.show();
    this.dksService.searchF340PpdProcess(this.sF340PpdSchedule).subscribe(
      (res: PaginatedResult<F340SchedulePpd[]>) => {
        this.result = res.result;
        this.result.forEach(m=>{
          if(m.releaseType =="CWA" && m.treatMent.length > 1 && m.partName.length > 1 ){
            m.isDisplay = utilityConfig.YesNumber;
          }else{
            m.isDisplay = utilityConfig.NoNumber;
          }
          
        })
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
  //上傳PPD圖片
  uploadPicF340Ppd(files: FileList, model: F340SchedulePpd) {
    console.log(model);
    //"application/pdf" "image/jpeg"
    if (!this.utility.checkFileMaxFormat(files.item(0), (1128659 * 2 ),"image/jpeg")) {
      this.utility.alertify.confirm(
        "Sweet Alert",
        "Please upload jpg file and size cannot over 2 Mb.",
        () => {}
      );
      return; //exit function
    }
    var formData = new FormData();
    formData.append("file", files.item(0));
    formData.append("sampleNo",model.sampleNo);
    formData.append("treatMent",model.treatMent);
    formData.append("partName",model.partName);
    formData.append("article",model.article);
    formData.append("devSeason",model.devSeason);
    formData.append("loginUser", this.sF340PpdSchedule.loginUser);
    
    this.utility.spinner.show();
    this.dksService.editPicF340Ppd(formData).subscribe(
      (res) => {
        this.utility.spinner.hide();
        //找到該筆model 把資料回填
        model.photo = res.photo
      },
      (error) => {
        this.utility.spinner.hide();
        this.utility.alertify.error("Add failed !!!!");
      }
    );
  }
  //上傳pdf
  uploadPdfF340Ppd(files: FileList, model: F340SchedulePpd) {
    console.log(model);
    //"application/pdf" "image/jpeg"
    if (!this.utility.checkFileMaxFormat(files.item(0), (1128659 * 2 ),"application/pdf")) {
      this.utility.alertify.confirm(
        "Sweet Alert",
        "Please upload pdf file and size cannot over 2 Mb.",
        () => {}
      );
      return; //exit function
    }
    var formData = new FormData();
    formData.append("file", files.item(0));
    formData.append("sampleNo",model.sampleNo);
    formData.append("treatMent",model.treatMent);
    formData.append("partName",model.partName);
    formData.append("article",model.article);
    formData.append("devSeason",model.devSeason);
    this.utility.spinner.show();
    this.dksService.editPdfF340Ppd(formData).subscribe(
      (res) => {
        this.utility.spinner.hide();
        //找到該筆model 把資料回填
        model.photo = res.photo
      },
      (error) => {
        this.utility.spinner.hide();
        this.utility.alertify.error("Add failed !!!!");
      }
    );
  }
  //刪除F340 ppd 圖片  
  removePicF340Ppd(model: F340SchedulePpd) {
    var formData = new FormData(); 
    formData.append("file", null);  // upload null present delete
    formData.append("sampleNo",model.sampleNo);
    formData.append("treatMent",model.treatMent);
    formData.append("partName",model.partName);
    formData.append("article",model.article);
    formData.append("devSeason",model.devSeason);
    formData.append("photo",model.photo); 
    formData.append("loginUser", this.sF340PpdSchedule.loginUser);
    this.utility.alertify.confirm(
      "Sweet Alert",
      "Are you sure to Delete this picture ?",
      () => {
        this.utility.spinner.show();
        this.dksService.editPicF340Ppd(formData).subscribe(
          () => {
            this.utility.spinner.hide();
            this.utility.alertify.success("Delete succeed!");
            model.photo =''; // make the url of photo to blank
          },
          (error) => {
            this.utility.spinner.hide();
            this.utility.alertify.error("Delete failed !!!!");
          }
        );
      }
    );
  }
   //刪除F340 ppd Pdf  
   removePdfF340Ppd(model: F340SchedulePpd) {
    var formData = new FormData(); 
    formData.append("file", null);  // upload null present delete
    formData.append("sampleNo",model.sampleNo);
    formData.append("treatMent",model.treatMent);
    formData.append("partName",model.partName);
    formData.append("article",model.article);
    formData.append("devSeason",model.devSeason);
    formData.append("pdf",model.pdf); 
    this.utility.alertify.confirm(
      "Sweet Alert",
      "Are you sure to Delete this pdf ?",
      () => {
        this.utility.spinner.show();
        this.dksService.editPdfF340Ppd(formData).subscribe(
          () => {
            this.utility.spinner.hide();
            this.utility.alertify.success("Delete succeed!");
            model.pdf =''; 
          },
          (error) => {
            this.utility.spinner.hide();
            this.utility.alertify.error("Delete failed !!!!");
          }
        );
      }
    );
  }
  viewPic(model: F340SchedulePpd){
    let serverIp = "";
    switch(this.sF340PpdSchedule.factory) { 
      case 'C': { 
        serverIp = environment.mpsSpaC;
         break; 
      } 
      case 'U': { 
        serverIp = environment.mpsSpaU;
         break; 
      } 
      case 'E': { 
        serverIp = environment.mpsSpaE; 
        break; 
      } 
      case 'D': { 
        serverIp = environment.mpsSpaD;
         break; 
      } 
   } 
    let dataUrl = 'http://' + serverIp +'/assets/F340PpdPic/' + model.devSeason +  "/" + model.article + "/" + model.photo;
    window.open(dataUrl);
  }
  viewPDF(model: F340SchedulePpd){
    let serverIp = "";
    switch(this.sF340PpdSchedule.factory) { 
      case 'C': { 
        serverIp = environment.mpsSpaC;
         break; 
      } 
      case 'U': { 
        serverIp = environment.mpsSpaU;
         break; 
      } 
      case 'E': { 
        serverIp = environment.mpsSpaE; 
        break; 
      } 
      case 'D': { 
        serverIp = environment.mpsSpaD;
         break; 
      } 
   } 
    let dataUrl = 'http://' + serverIp +'/assets/F340PpdPic/' + model.devSeason +  "/" + model.article + "/" + model.pdf;
    window.open(dataUrl);
  }
  editMemo(){
    this.memoBtn = !this.memoBtn;
  }
  saveMemo(){
    this.utility.spinner.show();
    this.memoBtn = !this.memoBtn;
    this.dksService.editF340Ppds(this.result).subscribe(
      (res) => {
        this.utility.spinner.hide();
        this.utility.alertify.confirm(
          "Sweet Alert",
          "You Updated PPD Remark.",
          () => { });  
      },
      (error) => {
        this.utility.spinner.hide();
        this.utility.alertify.error(error);
      }
    );
  }
  openModal(type:string){
    if(type == "PhotoComment") this.photoCommentModal.show();
    if(type == "PpdRemark") this.ppdRemarkModal.show();

  }
  closeModal(type:string){
    if(type == "PhotoComment") this.photoCommentModal.hide();
    if(type == "PpdRemark")this.ppdRemarkModal.hide();
  }
  editPhotoComment(model: F340SchedulePpd){
    this.openModal("PhotoComment");
    this.editModel = model;
  }
  savePhotComment(type:string){
    this.utility.spinner.show();
    this.dksService.editF340Ppd(this.editModel,type).subscribe(
      (res) => {
        this.utility.spinner.hide();
        this.utility.alertify.confirm(
          "Sweet Alert",
          "You Updated Comment.",
          () => { this.closeModal(type) });  
      },
      (error) => {
        this.utility.spinner.hide();
        this.utility.alertify.error(error);
      }
    );
  }
  editPpdRemark(model: F340SchedulePpd){
    this.openModal("PpdRemark");
    this.editModel = model;
  }
  sendMail(){
    if(this.sF340PpdSchedule.article.length < 1) {
      alert( "請先輸入Article" );
      return;
    } 
    this.utility.spinner.show();
    debugger;
    this.dksService.sentMailF340PpdByArticle(this.sF340PpdSchedule).subscribe(
      (res) => {
        this.utility.spinner.hide();
        this.utility.alertify.confirm(
          "Sweet Alert",
          "You sent Mail.",
          () => { });  
      },
      (error) => {
        this.utility.spinner.hide();
        this.utility.alertify.error(error);
      }
    );
  }
}
