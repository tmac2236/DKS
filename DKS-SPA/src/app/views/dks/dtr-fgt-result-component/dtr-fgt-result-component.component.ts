import { Component, OnInit, ViewChild } from "@angular/core";
import { ModalDirective } from "ngx-bootstrap/modal";
import { Utility } from "../../../core/utility/utility";
import { utilityConfig } from "../../../core/utility/utility-config";
import { DevDtrFgtResult } from "../../../core/_models/dev-dtr-fgt-result";
import { DevDtrFgtResultDto } from "../../../core/_models/dev-dtr-fgt-result-dto";
import { PaginatedResult } from "../../../core/_models/pagination";
import { SDevDtrFgtResult } from "../../../core/_models/s-dev-dtr-fgt-result";
import { DtrService } from "../../../core/_services/dtr.service";

@Component({
  selector: "app-dtr-fgt-result-component",
  templateUrl: "./dtr-fgt-result-component.component.html",
  styleUrls: ["./dtr-fgt-result-component.component.scss"],
})
export class DtrFgtResultComponentComponent implements OnInit {
  @ViewChild("addFgtResultModal") public addFgtResultModal: ModalDirective;
  //for hint
  hintMsg: any = {
    uploadPdf: "Please upload jpg file and size cannot over 2 Mb.",
  };
  uiControls: any = {
    uploadPicF340Ppd: utilityConfig.RolePpdPic,
  };
  sDevDtrFgtResult: SDevDtrFgtResult = new SDevDtrFgtResult();
  result: DevDtrFgtResultDto[] = [];
  articleList: object[]; //ArticleModelNameDto
  partNameList: object[]; //F340PartNoTreatmemtDto
  addAModel: DevDtrFgtResult = new DevDtrFgtResult(); //use in addFgtResultModal
  addAModelTreatment: string = ""; //only let user see not save to db
  constructor(
    public utility: Utility,
    private dtrService: DtrService
  ) {}

  ngOnInit() {
    this.utility.initUserRole(this.sDevDtrFgtResult);
    this.addAModel.upusr = this.sDevDtrFgtResult.loginUser;
  }

  //搜尋
  async search() {
    //modelNo 或 Article 至少任一需輸入等於五個字元
    //if(!(this.sDevDtrFgtResult.modelNo.trim().length == 5 || this.sDevDtrFgtResult.article.trim().length == 6)) return;
    this.cleanAll();
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
    await this.dtrService
      .getArticle4Fgt(
        this.sDevDtrFgtResult.modelNo,
        this.sDevDtrFgtResult.article,
        this.sDevDtrFgtResult.modelName
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
    this.partNameList = [];//clear
    this.addAModel.partName = ""; //防呆
    if (this.addAModel.stage == "SMS" || this.addAModel.stage == "MCS") {
      this.utility.spinner.show();
      await this.dtrService
        .getPartName4DtrFgt(this.addAModel.article, this.addAModel.stage)
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
  //上傳pdf
  uploadPdfDtrFgtResult(files: FileList, model: DevDtrFgtResult) {
    console.log(model);
    //"application/pdf" "image/jpeg"
    if (
      !this.utility.checkFileMaxFormat(
        files.item(0),
        1128659 * 2,
        "application/pdf"
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
    formData.append("fileName", model.fileName);
    formData.append("loginUser", this.sDevDtrFgtResult.loginUser);
    this.utility.spinner.show();
    this.dtrService.editPdfDevDtrFgtResult(formData).subscribe(
      (res) => {
        this.utility.spinner.hide();
        //找到該筆model 把資料回填
        console.log("res dpf" + res.fileName);
        model.fileName = res.fileName;
      },
      (error) => {
        this.utility.spinner.hide();
        this.utility.alertify.error("Add failed !!!!");
      }
    );
  }
  viewPdf(item: DevDtrFgtResultDto) {
    let dataUrl =
      "../assets/F340PpdPic/QCTestResult/" + item.article + "/" + item.fileName;
    window.open(dataUrl);
  }
  //Add a result of fgt
  openAddFgtResultModal() {
    this.openModal("addFgtResult");
  }
  openModal(type: string) {
    this.addAModel = new DevDtrFgtResult();
    if (type == "addFgtResult") this.addFgtResultModal.show();
  }
  closeModal(type: string) {
    if (type == "addFgtResult") this.addFgtResultModal.hide();
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

    if (this.partNameList != null) {
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
      //Step 2: check is labNo valid
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
        //Step 3: call api save to db

        this.utility.spinner.show();
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
  selectArticle() {
    this.addAModel.stage = ""; //防呆
    var model = this.articleList.find(
      (x) => x["article"] == this.addAModel.article
    );
    this.addAModel.modelNo = model["modelNo"];
    this.addAModel.modelName = model["modelName"];
  }

  cleanAll() {
    this.result = [];
    this.articleList = [];
    this.addAModel = new DevDtrFgtResult();
    this.addAModelTreatment = "";
  }
  deleteDevDtrFgtResult(model:DevDtrFgtResult) {
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
}
