import { Component, OnInit, ViewChild } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { Utility } from '../../../core/utility/utility';
import { utilityConfig } from '../../../core/utility/utility-config';
import { DevDtrFgtResult } from '../../../core/_models/dev-dtr-fgt-result';
import { DevDtrFgtResultDto } from '../../../core/_models/dev-dtr-fgt-result-dto';
import { PaginatedResult } from '../../../core/_models/pagination';
import { SDevDtrFgtResult } from '../../../core/_models/s-dev-dtr-fgt-result';
import { DksService } from '../../../core/_services/dks.service';
import { DtrService } from '../../../core/_services/dtr.service';

@Component({
  selector: 'app-dtr-fgt-result-component',
  templateUrl: './dtr-fgt-result-component.component.html',
  styleUrls: ['./dtr-fgt-result-component.component.scss']
})
export class DtrFgtResultComponentComponent implements OnInit {

  @ViewChild('addFgtResultModal') public addFgtResultModal: ModalDirective;
  //for hint
  hintMsg:any = {
    uploadPdf: "Please upload jpg file and size cannot over 2 Mb."
  };

  sDevDtrFgtResult: SDevDtrFgtResult = new SDevDtrFgtResult();
  result: DevDtrFgtResultDto[] = [];
  articleList: string[];
  //memoBtn = true;
  uiControls:any = {
    uploadPicF340Ppd: utilityConfig.RolePpdPic,
    uploadPdfF340Ppd: utilityConfig.RolePpdPic,
    editMemo: utilityConfig.RoleSysAdm,
    sendMemoMail: utilityConfig.RoleSysAdm,
  };
  editModel: DevDtrFgtResult = new DevDtrFgtResult(); //use in addFgtResultModal and upload PDF

  constructor(public utility: Utility, private dksService: DksService, private dtrService: DtrService) {}

  ngOnInit() {
    this.utility.initUserRole(this.sDevDtrFgtResult);
  }

  //搜尋
  search() {
    this.utility.spinner.show();
    this.dtrService.searchDevDtrFgtResult(this.sDevDtrFgtResult).subscribe(
      (res: PaginatedResult<DevDtrFgtResultDto[]>) => {
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
        this.sDevDtrFgtResult.setPagination(res.pagination);
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
  /*
  export() {
    const url = this.utility.baseUrl + "dks/exportF340_ProcessPpd";
    this.utility.exportFactory(url, "F340_PPD", this.sDevDtrFgtResult);
  }
  */

  //下拉選單帶出版本
  changeArticleVerList() {
    if (this.sDevDtrFgtResult.article !== "") {
      this.articleList.push(this.sDevDtrFgtResult.article);
      return;
    }
    this.utility.spinner.show();
    this.dtrService
      .getArticlebyModelNo(
        this.sDevDtrFgtResult.modelNo
      )
      .subscribe(
        (res) => {
          this.utility.spinner.hide();
          this.articleList = res;
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



  //上傳pdf
  uploadPdfDtrFgtResult(files: FileList, model: DevDtrFgtResult) {
    console.log(model);
    //"application/pdf" "image/jpeg"
    if (!this.utility.checkFileMaxFormat(files.item(0), (1128659 * 2 ),"application/pdf")) {
      this.utility.alertify.confirm(
        "Sweet Alert",
        this.hintMsg.uploadPdf,
        () => {}
      );
      return; //exit function
    }
    var formData = new FormData();
    formData.append("file", files.item(0));
    formData.append("article",model.article);
    formData.append("modelNo",model.modelNo);
    formData.append("modelName",model.modelName);
    formData.append("labNo",model.labNo);
    formData.append("loginUser", this.sDevDtrFgtResult.loginUser);
    this.utility.spinner.show();
    this.dtrService.editPdfDevDtrFgtResult(formData).subscribe(
      (res) => {
        this.utility.spinner.hide();
        //找到該筆model 把資料回填
        console.log("res dpf" + res.fileName);
        model.fileName = res.fileName
      },
      (error) => {
        this.utility.spinner.hide();
        this.utility.alertify.error("Add failed !!!!");
      }
    );
  }
    /*
   //刪除F340 ppd Pdf  
   removePdfDtrFgtResult(model: DevDtrFgtResult) {
    var formData = new FormData(); 
    formData.append("file", null);  // upload null present delete
    formData.append("article",model.article);
    formData.append("modelNo",model.modelNo);
    formData.append("modelName",model.modelName);
    formData.append("labNo",model.labNo);
    formData.append("fileName", model.fileName);
    formData.append("loginUser", this.sF340PpdSchedule.loginUser);
    this.utility.alertify.confirm(
      "Sweet Alert",
      "Are you sure to Delete this pdf of article:" + model.article + ", treatment:" + model.treatMent + ", partName:" + model.partName + ".",
      () => {
        this.utility.spinner.show();
        this.dksService.editPdfDevDtrFgtResult(formData).subscribe(
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
  viewPDF(model: F340SchedulePpd){
  
   let param = utilityConfig.encodeStr + model.devSeason +  "$" + model.article + "$" + model.pdf + "$" + this.sF340PpdSchedule.factory + "$" + this.sF340PpdSchedule.loginUser;
   let dataUrl = environment.apiUrl + "dks/getF340PpdPdf?isStanHandsome=" + window.btoa(param);
   window.open(dataUrl);
  }
  
  //Add a result of fgt
  openAddFgtResultModal(){
    this.openModal("addFgtResult");
  }
  openModal(type:string){
    if(type == "addFgtResult") this.addFgtResultModal.show();

  }
  closeModal(type:string){
    if(type == "addFgtResult") this.addFgtResultModal.hide();
  }

  saveAFgtResult(){
    alert("saveAFgtResult");
  }
*/
}
