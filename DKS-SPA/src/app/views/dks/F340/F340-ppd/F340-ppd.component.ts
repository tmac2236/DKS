import { Component, OnInit, ViewChild } from "@angular/core";
import { Utility } from "../../../../core/utility/utility";
import { utilityConfig } from "../../../../core/utility/utility-config";
import { DksService } from "../../../../core/_services/dks.service";
import { PaginatedResult } from "../../../../core/_models/pagination";
import { SF340PpdSchedule } from "../../../../core/_models/s_f340-ppd-schedule";
import { F340SchedulePpd } from "../../../../core/_models/f340-schedule-ppd";
import { ModalDirective } from "ngx-bootstrap/modal";
import { environment } from "../../../../../environments/environment";
import { CommonService } from "../../../../core/_services/common.service";
import { defineLocale } from 'ngx-bootstrap/chronos';
import { zhCnLocale } from "ngx-bootstrap/locale"; //中文
//import { viLocale } from "ngx-bootstrap/locale"; //越文
import { enGbLocale } from "ngx-bootstrap/locale"; //英文
defineLocale("zh", zhCnLocale); //定義local中文
//defineLocale("vn", viLocale);//定義local越文
defineLocale("en", enGbLocale);//定義local英文

@Component({
  selector: "app-F340",
  templateUrl: "./F340-ppd.component.html",
  styleUrls: ["./F340-ppd.component.scss"],
})

export class F340PpdComponent implements OnInit {
  @ViewChild('photoCommentModal') public photoCommentModal: ModalDirective;
  @ViewChild('ppdRemarkModal') public ppdRemarkModal: ModalDirective;
  @ViewChild('uBDateModal') public uBDateModal: ModalDirective;
  //for hint
  hintMsg:any = {
    uploadPic: "Please upload jpg file and size cannot over 10 Mb.",
    uploadPdf: "Please upload pdf file and size cannot over 10 Mb."
  };
  //for sorting ; ASC DESC
  cwaDeadlineS = true;

  sF340PpdSchedule: SF340PpdSchedule = new SF340PpdSchedule();
  result: F340SchedulePpd[] = [];
  bpVerList: string[];
  //memoBtn = true;
  uiControls:any = {
    uploadPicF340Ppd: utilityConfig.RolePpdPic,
    uploadPdfF340Ppd: utilityConfig.RolePpdPic,
    editMemo: utilityConfig.RoleSysAdm,
    sendMemoMail: utilityConfig.RoleSysAdm,
    upBottomMaintain: utilityConfig.RoleSysAdm,
  };
  editModel: F340SchedulePpd = new F340SchedulePpd(); //onlt use in photoCommentModal
  uBDateModel: F340SchedulePpd = new F340SchedulePpd(); // only use in uBDateModal

  constructor(public utility: Utility, private dksService: DksService, private commonService: CommonService) {}

  ngOnInit() {
    this.utility.initUserRole(this.sF340PpdSchedule);
    if(this.sF340PpdSchedule.factoryId =='2'){
      this.sF340PpdSchedule.factory = utilityConfig.factory; //事業部帳號預設翔鴻程
    }else{
      this.sF340PpdSchedule.factory = this.sF340PpdSchedule.factoryId;
    }

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
        /*
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
        */
        this.result.forEach(m=>{
          if(m.releaseType =="CWA" && m.treatMent.length > 1 && m.partName.length > 1 ){
            m.isDisplay = utilityConfig.YesNumber;
          }else{
            m.isDisplay = utilityConfig.NoNumber;
          }
          
        })
        this.sF340PpdSchedule.setPagination(res.pagination);
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
    const url = this.utility.baseUrl + "dks/exportF340_ProcessPpd";
    this.utility.exportFactory(url, "F340_PPD", this.sF340PpdSchedule);
  }

  //下拉選單帶出版本
  async changeBPVerList() {
    if (this.sF340PpdSchedule.season === "") return;
    this.utility.spinner.show();
    await this.commonService
      .searchBPVerList(
        this.sF340PpdSchedule.season,
        this.sF340PpdSchedule.factory
      )
      .then(
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
    if (!this.utility.checkFileMaxFormat(files.item(0), (1128659 * 10 ),"image/jpeg")) {
      this.utility.alertify.confirm(
        "Sweet Alert",
        this.hintMsg.uploadPic,
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
    formData.append("factoryId", this.sF340PpdSchedule.factoryId);

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
    if (!this.utility.checkFileMaxFormat(files.item(0), (1128659 * 10 ),"application/pdf")) {
      this.utility.alertify.confirm(
        "Sweet Alert",
        this.hintMsg.uploadPdf,
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
    formData.append("factoryId", this.sF340PpdSchedule.factoryId);

    this.utility.spinner.show();
    this.dksService.editPdfF340Ppd(formData).subscribe(
      (res) => {
        this.utility.spinner.hide();
        //找到該筆model 把資料回填
        console.log("res dpf" + res.pdf);
        model.pdf = res.pdf
        console.log("model dpf" + model.pdf);
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
    formData.append("factoryId", this.sF340PpdSchedule.factoryId);

    this.utility.alertify.confirm(
      "Sweet Alert",
      "Are you sure to Delete this picture of article:" + model.article + ", treatment:" + model.treatMent + ", partName:" + model.partName + ".",
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
    formData.append("loginUser", this.sF340PpdSchedule.loginUser);
    formData.append("factoryId", this.sF340PpdSchedule.factoryId);

    this.utility.alertify.confirm(
      "Sweet Alert",
      "Are you sure to Delete this pdf of article:" + model.article + ", treatment:" + model.treatMent + ", partName:" + model.partName + ".",
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
    let factoryApi = "";
    switch (this.sF340PpdSchedule.factory) {
      case "C": //SHC
          factoryApi = environment.apiUrl + "dks/getF340PpdPic?isStanHandsome=";
          break;
      case "E": //CB
          factoryApi = "http://10.9.0.35/material/WatermarkAPI/GetF340PpdPic?param=";
          break;
      case "D": //SPC
          factoryApi = "http://10.10.0.21/material/WatermarkAPI/GetF340PpdPic?param=";
          break;
      case "U": //TSH
          factoryApi = "http://10.11.0.22/material/WatermarkAPI/GetF340PpdPic?param=";
          break;                                
      default: { 
          factoryApi = environment.apiUrl + "dks/getF340PpdPic?isStanHandsome=";
          break;
         } 
    }
    let param = utilityConfig.encodeStr + model.devSeason +  "$" + model.article + "$" + model.photo + "$" + this.sF340PpdSchedule.factory + "$" + this.sF340PpdSchedule.loginUser;
    let dataUrl = factoryApi + window.btoa(param);
    window.open(dataUrl);
  }
  viewPDF(model: F340SchedulePpd){
    let factoryApi = "";
    switch (this.sF340PpdSchedule.factory) {
      case "C": //SHC
          factoryApi = environment.apiUrl + "dks/getF340PpdPdf?isStanHandsome=";
          break;
      case "E": //CB
          factoryApi = "http://10.9.0.35/material/WatermarkAPI/GetF340PpdPdf?param=";
          break;
      case "D": //SPC
          factoryApi = "http://10.10.0.21/material/WatermarkAPI/GetF340PpdPdf?param=";
          break;
      case "U": //TSH
          factoryApi = "http://10.11.0.22/material/WatermarkAPI/GetF340PpdPdf?param=";
          break;                                
      default: { 
          factoryApi = environment.apiUrl + "dks/getF340PpdPdf?isStanHandsome=";
          break;
         } 
    }
   let param = utilityConfig.encodeStr + model.devSeason +  "$" + model.article + "$" + model.pdf + "$" + this.sF340PpdSchedule.factory + "$" + this.sF340PpdSchedule.loginUser;
   let dataUrl = factoryApi + window.btoa(param);
   window.open(dataUrl);
  }
  downloadPDF(model: F340SchedulePpd){
    const url = this.utility.baseUrl + "dks/exportF340_ProcessPpd_pdf";
    this.utility.exportPdfFactory(url, model.pdf, model);
  }
  /*
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
  */
  openModal(type:string){
    if(type == "PhotoComment") this.photoCommentModal.show();
    if(type == "PpdRemark") this.ppdRemarkModal.show();
    if(type == "UBDate") this.uBDateModal.show();

  }
  closeModal(type:string){
    if(type == "PhotoComment") this.photoCommentModal.hide();
    if(type == "PpdRemark")this.ppdRemarkModal.hide();
    if(type == "UBDate") this.uBDateModal.hide();
  }
  //viewPic 共用
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
  editUBDate(model: F340SchedulePpd){
    this.uBDateModel = model;
    this.uBDateModel.factory = this.sF340PpdSchedule.factory; //等Aven改Procedure暫時先用登入者的廠別給值
    this.openModal("UBDate");
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
          "sent ail success.",
          () => { });  
      },
      (error) => {
        this.utility.spinner.hide();
        this.utility.alertify.error(error);
      }
    );
  }
  viewFGT(model: F340SchedulePpd){
    //http://10.4.0.39:6970/assets/F340PpdPic/QCTestResult/GV7909/GV7909_CS1_FGT_PASS_001767.pdf

    let factoryApi = "";
    switch (this.sF340PpdSchedule.factory) {
      case "C": //SHC
          factoryApi = environment.spaUrl + "/assets/F340PpdPic/QCTestResult/";
          break;
      case "E": //CB
          factoryApi = "http://10.9.0.35/material/Upload/F340CmptMatPic/QCTestResult/";
          break;
      case "D": //SPC
          factoryApi = "http://10.10.0.21/material/Upload/F340CmptMatPic/QCTestResult/";
          break;
      case "U": //TSH
          factoryApi = "http://10.11.0.22/material/Upload/F340CmptMatPic/QCTestResult/";
          break;                                
      default: { 
          factoryApi = environment.spaUrl + "/assets/F340PpdPic/QCTestResult/";
          break;
         } 
    }
   let dataUrl = factoryApi + model.article + '/' + model.fgtFileName;
   window.open(dataUrl);
  }
  saveUBDate(){
    this.utility.spinner.show();
    this.dksService.saveUBDate(this.uBDateModel).subscribe(
      (res) => {
        this.utility.spinner.hide();
        this.closeModal("UBDate");
        this.utility.alertify.success("Save Success !!");
      },
      (error) => {
        this.utility.spinner.hide();
        this.utility.alertify.error(error);
      }
    );
  }
}
