import { Component, OnInit, ViewChild } from "@angular/core";
import { ModalDirective } from "ngx-bootstrap/modal";
import { Utility } from "../../../core/utility/utility";
import { utilityConfig } from "../../../core/utility/utility-config";
import { DevDtrFgtResult } from "../../../core/_models/dev-dtr-fgt-result";
import { DevDtrFgtResultDto } from "../../../core/_models/dev-dtr-fgt-result-dto";
import { DtrLoginHistoryDto } from "../../../core/_models/dtr-login-history-dto";
import { PaginatedResult } from "../../../core/_models/pagination";
import { SDevDtrFgtResult } from "../../../core/_models/s-dev-dtr-fgt-result";
import { CommonService } from "../../../core/_services/common.service";
import { DtrService } from "../../../core/_services/dtr.service";

@Component({
  selector: "app-dtr-fgt-result-component",
  templateUrl: "./dtr-fgt-result-component.component.html",
  styleUrls: ["./dtr-fgt-result-component.component.scss"],
})
export class DtrFgtResultComponentComponent implements OnInit {
  @ViewChild("addFgtResultModal") public addFgtResultModal: ModalDirective;
  @ViewChild("editFgtResultModal") public editFgtResultModal: ModalDirective;
  @ViewChild("upgradeModal") public upgradeModal: ModalDirective;
  //for hint
  hintMsg: any = {
    uploadPdf: "Please upload pdf or excel file and size cannot over 2 Mb.",
  };
  uiControls: any = {
    editModel: utilityConfig.RoleSysAdm,
  };
  title = "LAB Test Report Maintain";
  sDevDtrFgtResult: SDevDtrFgtResult = new SDevDtrFgtResult();
  result: DevDtrFgtResultDto[] = [];
  articleList: object[]; //ArticleModelNameDto
  partNameList: object[]; //F340PartNoTreatmemtDto
  stageList: { id: number, name: string }[] =[];
  oStageList: { id: number, name: string }[] = [
    { "id": 1, "name": "CR2" },
    { "id": 2, "name": "SMS" },
    { "id": 3, "name": "CS1" },
    { "id": 4, "name": "CS2" },
    { "id": 5, "name": "CS3" }
];

  addAModel: DevDtrFgtResult = new DevDtrFgtResult(); //use in addFgtResultModal、editFgtResultModal
  addAModelTreatment: string = ""; //only let user see not save to db
  isValidUpload: boolean = false; //卡控新增畫面的上傳按鈕(Add和Upgrade都會用到)
  upgradeModel: DevDtrFgtResult = new DevDtrFgtResult(); //use in upgradeModelModal
  isUploadable: boolean = false;  //只用於外層的上傳按鈕，為了卡控用而已

  constructor(
    public utility: Utility,
    private dtrService: DtrService,
    private commonService: CommonService
  ) {}

  ngOnInit() {
    this.utility.initUserRole(this.sDevDtrFgtResult);
    this.addAModel.upusr = this.sDevDtrFgtResult.loginUser;
    this.loginReord();
  }

  //搜尋
  async search() {
    //modelNo 或 Article 至少任一需輸入等於五個字元
    //if(!(this.sDevDtrFgtResult.modelNo.trim().length == 5 || this.sDevDtrFgtResult.article.trim().length == 6)) return;
    this.result = [];
    this.articleList = [];
    await this.getArticleVerList();
    if (this.articleList == undefined || this.articleList.length == 0) {
      this.utility.alertify.confirm(
        "Sweet Alert",
        "These conditions didn't exist in F205 Article in DKS.",
        () => {
          this.utility.spinner.hide();
          return;
        }
      );
    } else {
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
  }
  /*
  export() {
    const url = this.utility.baseUrl + "dks/exportF340_ProcessPpd";
    this.utility.exportFactory(url, "F340_PPD", this.sDevDtrFgtResult);
  }
  */

  //下拉選單帶出Article 
  async getArticleVerList() {
    await this.commonService
      .getArticle(
        this.sDevDtrFgtResult.modelNo,
        this.sDevDtrFgtResult.article,
        this.sDevDtrFgtResult.modelName,
        ''//this.sDevDtrFgtResult.factoryId  10/6:Aven表示下拉的Article要不分廠別
      )
      .then(
        (res) => {
          this.articleList = res;
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
  //下拉選單帶出PartName
  async getPartName4DtrFgt() {
    this.partNameList = []; //clear
    this.addAModel.partName = ""; //防呆
    //有選Component Test時, 才需要去檢查有沒有PartName
    if (
      this.addAModel.kind == "CT" &&
      (this.addAModel.stage == "SMS" || this.addAModel.stage == "CS1")
    ) {
      this.utility.spinner.show();
      await this.commonService
        .getPartName(this.addAModel.article, this.addAModel.stage)
        .then(
          (res) => {
            this.utility.spinner.hide();
            this.partNameList = res;
            if (
              this.partNameList == undefined ||
              this.partNameList.length == 0
            ) {
              this.utility.alertify.confirm(
                "Sweet Alert",
                "The article didn't have any partName in F340 PPD in DKS.",
                () => {
                  return;
                }
              );
            }
          },
          (error) => {
            this.utility.alertify.confirm(
              "System Notice",
              "Syetem is busy, please try later.",
              () => {}
            );
          }
        );
    }else if (this.checkKind4Input(this.addAModel)){ //v1.2 labNo = factory + Article
      this.addAModel.labNo =  this.addAModel.article
    }
  }
  showPartNameDetail() {
    var model = this.partNameList.find(
      (x) => x["partName"] == this.addAModel.partName
    );
    let treatmentCode = model["treatmentCode"];
    let treatmentEn = model["treatmentEn"];
    let treatmentZh = model["treatmentZh"];

    this.addAModelTreatment = `${treatmentCode} ${treatmentZh} (${treatmentEn})`;
    this.addAModel.partNo = model["partNo"];
  }
  //上傳pdf、excel step1
  isUploadableBtn(model: DevDtrFgtResult){
    //"fileUpload{{ model.article }}{{ model.stage }}{{ model.labNo }}"
    let id = `fileUpload${model.article}${model.stage}${model.labNo}`; 
    console.log(id);
    if(model.result == 'PASS'){
      this.utility.alertify.confirm(
        `Article: ${model.article}  Stage: ${model.stage}  LabNo: ${model.labNo}`,
        "The status of the labNo is PASS , are you sure to overwrite the file in server, the previous will permanent disappear !",
        () => {
          document.getElementById(id).click();
        }
      );
    }else{
      document.getElementById(id).click();
    }

  }
  //上傳pdf、excel step2
  uploadPdfDtrFgtResult(files: FileList, model: DevDtrFgtResult) {
    console.log(model);
    //accept pdf, xls , xlsx and below 2Mb
    if (
      !this.utility.checkFileMaxMultiFormat(
        files.item(0),
        1128659 * 2,
        ["application/pdf","application/vnd.ms-excel","application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"]
      )
    ) {
      this.utility.alertify.confirm(
        "Sweet Alert",
        this.hintMsg.uploadPdf,
        () => {}
      );
      return; //exit function
    }
    var formData = new FormData();
    formData.append("file", files.item(0));
    formData.append("article", model.article);
    formData.append("modelNo", model.modelNo);
    formData.append("modelName", model.modelName);
    formData.append("labNo", model.labNo);
    formData.append("stage", model.stage);
    formData.append("loginUser", this.sDevDtrFgtResult.loginUser);
    this.utility.spinner.show();
    this.dtrService.editPdfDevDtrFgtResult(formData).subscribe(
      (res) => {
        this.utility.spinner.hide();
        //找到該筆model 把資料回填
        this.utility.alertify.success("Add file success!");
        this.closeModal("addFgtResult"); //新增一筆record後上傳pdf成功關閉modal
        // refresh the page
        this.search();
      },
      (error) => {
        this.utility.spinner.hide();
        this.utility.alertify.error("Add file failed !!!!");
      }
    );
  }
  viewPdf(item: DevDtrFgtResultDto) {
    let dataUrl =
      "../assets/F340PpdPic/QCTestResult/" + item.article + "/" + item.fileName;
    window.open(dataUrl);
  }
  //Add or update a result of fgt
 openAddFgtResultModal(type: string, editModel?: DevDtrFgtResultDto) {
    this.cleanModel();

    if (type == "addFgtResult") {
      this.addAModel.type = "Article"; // default is Article
      this.openModal("addFgtResult");
    } else if (type == "editFgtResult") {
      this.addAModel.article = editModel.article; //PK
      this.addAModel.modelNo = editModel.modelNo; //PK
      this.addAModel.modelName = editModel.modelName; //PK
      this.addAModel.labNo = editModel.labNo; //PK
      this.addAModel.result = editModel.result;   //Can edit 1
      this.addAModel.remark = editModel.remark;   //Can edit 2
      this.openModal("editFgtResult");
    }
  }
  openModal(type: string) {
    if (type == "addFgtResult") this.addFgtResultModal.show();
    if (type == "editFgtResult") this.editFgtResultModal.show();
    if (type == "upgrade") this.upgradeModal.show();
  }
  closeModal(type: string) {
    if (type == "addFgtResult") this.addFgtResultModal.hide();
    if (type == "editFgtResult") this.editFgtResultModal.hide();
    if (type == "upgrade") this.upgradeModal.hide();
  }

  saveAFgtResult() {
    let isAlert = false;
    let alertStr = "Please select these (";
    if (this.utility.checkIsNullorEmpty(this.addAModel.modelName)) {
      isAlert = true;
      alertStr += " Model Name、";
    }
    if (this.utility.checkIsNullorEmpty(this.addAModel.modelNo)) {
      isAlert = true;
      alertStr += " Model No、";
    }
    if (this.utility.checkIsNullorEmpty(this.addAModel.article)) {
      isAlert = true;
      alertStr += " Article、";
    }
    if (this.utility.checkIsNullorEmpty(this.addAModel.stage)) {
      isAlert = true;
      alertStr += " Stage、";
    }
    if (this.utility.checkIsNullorEmpty(this.addAModel.kind)) {
      isAlert = true;
      alertStr += " Kind、";
    }
    if (this.utility.checkIsNullorEmpty(this.addAModel.type)) {
      isAlert = true;
      alertStr += " Type、";
    }
    if (this.utility.checkIsNullorEmpty(this.addAModel.labNo)) {
      isAlert = true;
      alertStr += " Lab No、";
    }
    if (this.utility.checkIsNullorEmpty(this.addAModel.result)) {
      isAlert = true;
      alertStr += " Test Result、";
    }

    if (this.partNameList?.length > 0) {
      if (this.utility.checkIsNullorEmpty(this.addAModel.partName)) {
        isAlert = true;
        alertStr += " Part Name、";
      }
    }
    //Step 1: check required input
    if (isAlert) {
      alertStr = alertStr.slice(0, -1);
      alertStr += ")";
      this.utility.alertify.confirm("Sweet Alert", alertStr, () => {
        return;
      });
    } else {
      debugger;
      //Step 2: check is labNo length is 6 and the value is number
      let isGoodLength = this.addAModel.labNo.length == 6;

      let nowyear2 = new Date().getFullYear().toString(); 
      let lastyear2 = (new Date().getFullYear()-1).toString();

      let isNoValid = nowyear2.slice(2, 4) == this.addAModel.labNo.slice(0, 2); // get 2021 --> 21 == the first 2 char of labNo
      if(!isNoValid) isNoValid = lastyear2.slice(2, 4) == this.addAModel.labNo.slice(0, 2); // get 2020 --> 20 == the first 2 char of labNo

      let isNumber = this.utility.checkIsNumber(this.addAModel.labNo);
      if (!isGoodLength) {
        this.utility.alertify.error("The length of Lab No have to be 6 !");
        return;
      }
      if((!this.checkKind4Input(this.addAModel))){  // if test(kind) is fit or wear, labNo don't check
        if (!isNumber) {
          this.utility.alertify.error("The Lab No have to be a number !");
          return;
        }
        if (!isNoValid) {
          this.utility.alertify.error(
            "The first 2 char of Lab No have to be " + nowyear2.slice(2, 4) + " or " + lastyear2.slice(2, 4) + " !"
          );
          return;
        }
      }  
      //Step 3: check is labNo valid
      let model = this.result.find(
        (x) => x["labNo"] == this.addAModel.labNo.trim()
      );
      if (model) {
        this.utility.alertify.confirm(
          "Sweet Alert",
          "The Lab No is exist please use another one!",
          () => {
            return;
          }
        );
      } else {
        //Step 4: call api save to db

        this.utility.spinner.show();
        //v1.2 labNo = Factory + labNo
        this.addAModel.labNo =  this.sDevDtrFgtResult.factoryId + this.addAModel.labNo
        this.dtrService.addDevDtrFgtResult(this.addAModel).subscribe(
          (res: boolean) => {
            this.utility.spinner.hide();
            if (!res) {
              this.utility.alertify.confirm(
                "Sweet Alert",
                "Save fault please refresh browser and try again!",
                () => {}
              );
            } else {
              //Step 4: refresh the page
              this.search();
              this.utility.alertify.success("Save success. Please upload pdf or excel!");
              this.isValidUpload = true; //let add pdf button show
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
    }
  }
  updateTheFgtResult(){
    this.dtrService.updateDevDtrFgtResult(this.addAModel).subscribe(
      (res: boolean) => {
        this.utility.spinner.hide();
        if (!res) {
          this.utility.alertify.confirm(
            "Sweet Alert",
            "Update fault please refresh browser and try again!",
            () => {}
          );
        } else {
          // refresh the page
          this.search();
          this.closeModal("editFgtResult"); //關閉modal
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
  selectArticle() {
    this.addAModel.stage = ""; //防呆
    var model = this.articleList.find(
      (x) => x["article"] == this.addAModel.article
    );
    this.addAModel.modelNo = model["modelNo"];
    this.addAModel.modelName = model["modelName"];
  }

  cleanModel() {
    this.addAModel = new DevDtrFgtResult();
    this.addAModel.upusr = this.sDevDtrFgtResult.loginUser;
    this.addAModelTreatment = "";
    this.isValidUpload = false;
  }
  cleanUpgrade(){
    this.upgradeModel = new DevDtrFgtResult();
    this.upgradeModel.upusr = this.sDevDtrFgtResult.loginUser;
  }
  deleteDevDtrFgtResult(model: DevDtrFgtResult) {
    this.utility.alertify.confirm(
      "Sweet Alert",
      "Are you sure to Delete this file of article:" + model.article + ", stage:" + model.stage + ".",
      () => {
        this.utility.spinner.show();
        this.dtrService.deleteDevDtrFgtResult(model).subscribe(
          (res: boolean) => {
            this.utility.spinner.hide();
            if (!res) {
              this.utility.alertify.confirm(
                "Sweet Alert",
                "Delete fault please refresh browser and try again!",
                () => {}
              );
            } else {
              //Step 2: refresh the page
              this.search();
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
    );
  }
  //article modleNo modelName at least one field is required
  checkSearchValid() {
    if (
      !this.utility.checkIsNullorEmpty(this.sDevDtrFgtResult.modelNo) ||
      !this.utility.checkIsNullorEmpty(this.sDevDtrFgtResult.modelName) ||
      !this.utility.checkIsNullorEmpty(this.sDevDtrFgtResult.article)
    ) {
      return false;
    } else {
      return true;
    }
  }
  uploadPdf() {
    let target = "";
    let id = (<HTMLInputElement>document.getElementById("name")).value;
  }
  firePdfUploadInAdd() {
    document.getElementById("pdfUploadInAdd").click();
  }
  //check test(kind) is 'FIT' or 'WEAR'
  checkKind4Input(addAModel:DevDtrFgtResult){
    let result =  false;
    if(addAModel.kind == 'FIT' || addAModel.kind == 'WEAR') result = true;
    return result;
  }
  //upgrade Version
  upgrade(model: DevDtrFgtResultDto) {
    if (model.stage == 'CS3') {
      this.utility.alertify.error(
        "Can not upgrade stage from CS3!"
      );
      return;
    }

    this.cleanUpgrade();
    this.upgradeModel = model;
    this.upgradeModel.upusr = this.sDevDtrFgtResult.loginUser;

    if(model.stage =="MCS"){  //因為舊資料還有MCS
      this.stageList = this.oStageList;
    }else{
      let a = this.oStageList.find( (x)=> x.name == model.stage );
      this.stageList = this.oStageList.filter( (x)=> x.id > a.id );
    }

    this.openModal("upgrade");
  }
  saveAFgtResultByUpgrade(){

    this.utility.spinner.show();
    this.dtrService.addDevDtrFgtResult(this.upgradeModel).subscribe(
      (res: boolean) => {
        this.utility.spinner.hide();
        if (!res) {
          this.utility.alertify.confirm(
            "Sweet Alert",
            "Save fault please refresh browser and try again!",
            () => {}
          );
        } else {
          //Step1: close the modal
          this.closeModal('upgrade');
          //Step2: refresh the page
          this.search();
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
  checkLabNoIsExist(){
    this.utility.spinner.show();
    let labNo =  this.sDevDtrFgtResult.factoryId + this.addAModel.labNo;
    this.dtrService.getAFgtByLabNo(labNo).subscribe(
      (res: DevDtrFgtResult) => {
        this.utility.spinner.hide();
        if (res) {
          this.addAModel.labNo =""; //empty the labNo
          this.utility.alertify.confirm(
            "System Alert",
            "This LabNo is exist in server, using in Article: " + res.article + ", Stage: " + res.stage + ", Update User: "+ res.upusr + ", Udate Time: " + res.upday.toString().split('T')[0],
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
  loginReord(){
    var record = new DtrLoginHistoryDto();
    record.systemName = this.title;
    record.account = this.sDevDtrFgtResult?.loginUser;
    if (this.utility.checkIsNullorEmpty(record.account)){
      return;
    }
    this.dtrService.dtrLoginHistory(record).subscribe(
      (res) => {
        console.log("Add a DtrLoginReord :" + record.account);
      },
      (error) => {
        this.utility.alertify.error(error);
      }
    );
  }
  
}
