import { Component, OnInit, ViewChild } from "@angular/core";
import { Utility } from "../../../core/utility/utility";
import { Router } from "@angular/router";
import { CommonService } from "../../../core/_services/common.service";
import { DevDtrVsList } from "../../../core/_models/dev-dtr-vs-list";
import { BasicCodeDto } from "../../../core/_models/basic-code-dto";
import { PaginatedResult } from "../../../core/_models/pagination";
import { environment } from "../../../../environments/environment";
import { DevBomFile } from "../../../core/_models/dev-bom-file";
import { DevBomFileDetailDto } from "../../../core/_models/dev-bom-file-detail-dto";
import { SDevBomFile } from "../../../core/_models/s-dev-bom-file";
import { DksService } from "../../../core/_services/dks.service";
import { ModalDirective } from "ngx-bootstrap/modal";
import { DevTeamByLoginDto } from "../../../core/_models/dev-team-by-login-dto";

@Component({
  selector: "app-bom-manage",
  templateUrl: "./bom-manage.component.html",
  styleUrls: ["./bom-manage.component.scss"],
})
export class BomManageComponent implements OnInit {
  @ViewChild("Modal1") public Modal1: ModalDirective;
  @ViewChild("Modal2") public Modal2: ModalDirective;
  @ViewChild("Modal3") public Modal3: ModalDirective;
  constructor(
    public utility: Utility,
    private route: Router,
    private dksService: DksService,
    private commonService: CommonService
  ) {}
  //for hint
  hintMsg: any = {
    uploadPdf: "Please upload .xlsx file and size cannot over 2 Mb.",
  };
  addAModel: DevBomFile = new DevBomFile();
  bufferFile: File | null = null; // upload

  sDevBomFile: SDevBomFile = new SDevBomFile();
  result: DevBomFileDetailDto[] = [];
  code017: BasicCodeDto[] = [];
  loginTeam :string[] = [];
  ngOnInit() {
    this.utility.initUserRole(this.sDevBomFile);

    if(!this.utility.checkIsNullorEmpty(this.sDevBomFile.loginUser)){
      this.dksService.getDevTeamByLoginDto(this.sDevBomFile.loginUser).subscribe(
        (res: DevTeamByLoginDto[]) => {
          if (res) {
            this.loginTeam = res.map(
              (devTeam: DevTeamByLoginDto) => devTeam.devTeamNo
            );
            console.log("this.loginTeam");
            console.log(this.loginTeam);
          } 
        },
        (error) => {
          console.log(error);
        }
      );      
    }

    this.getBasicCodeDto();
  }
  openModal(type: string) {
    this.cleanModel();
    if (type == "Modal1") this.Modal1.show();
    if (type == "Modal2") this.Modal2.show();
    if (type == "Modal3") this.Modal3.show();
  }
  closeModal(type: string) {
    if (type == "Modal1") this.Modal1.hide();
    if (type == "Modal2") this.Modal2.hide();
    if (type == "Modal3") this.Modal3.hide();
  }
  cleanModel() {
    this.addAModel = new DevBomFile();
    this.bufferFile = null;
  }
  //搜尋
  async search() {
    //article modleNo modelName at least one field is required
    if (
      !this.utility.checkIsNullorEmpty(this.sDevBomFile.season) ||
      !this.utility.checkIsNullorEmpty(this.sDevBomFile.modelNo) ||
      !this.utility.checkIsNullorEmpty(this.sDevBomFile.modelName) ||
      !this.utility.checkIsNullorEmpty(this.sDevBomFile.article) ||
      !this.utility.checkIsNullorEmpty(this.sDevBomFile.team)
    ) {
    } else {
      this.utility.alertify.error(
        "At least one field is required before Query !!!!"
      );
      return;
    }

    this.result = [];

    this.utility.spinner.show();

    this.dksService.getDevBomFile(this.sDevBomFile).subscribe(
      (res: PaginatedResult<DevBomFileDetailDto[]>) => {
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
        //this.sDevBomFile.setPagination(res.pagination);
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
  viewRecord(model: DevDtrVsList) {
    //let dataUrl =
    //environment.spaUrl + '/#/DTR-Vs-Maintain?homeParam=' + model.season + "$" + model.article + '$' + this.sDevDtrVsList.factoryId;
    //window.open(dataUrl);
  }

  //check is the season article is exist in Article、Model
  async getBasicCodeDto() {
    if (!this.utility.checkIsNullorEmpty(this.code017)) return;
    await this.commonService.getBasicCodeDto("017").then(
      (res) => {
        this.code017 = res;
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
  //save the file to memory
  handleFileInput(files: FileList) {
    //"application/pdf" "image/jpeg"
    if (
      !this.utility.checkFileMaxMultiFormat(files.item(0), 1128659 * 2, [
        "application/vnd.ms-excel",
        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
      ])
    ) {
      this.utility.alertify.confirm(
        "System Alert",
        this.hintMsg.uploadPdf,
        () => {}
      );
      return; //exit function
    }
    this.bufferFile = files.item(0);
  }

  saveUploadBtn() {
    /*
    if (this.utility.checkIsNullorEmpty(this.addAModel.remark)) {
      this.utility.alertify.error("You must type some remark !!!!");
      return;
    }
    */
    if (!this.bufferFile) {
      this.utility.alertify.error("Please upload a bom file file !!!!");
      return;
    }
    var formData = new FormData();

    formData.append("factoryId", this.addAModel.factory);
    formData.append("team", this.addAModel.devTeamId);
    formData.append("season", this.addAModel.season);
    formData.append("modelNo", this.addAModel.modelNo);
    formData.append("modelName", this.addAModel.modelName);

    formData.append("article", this.addAModel.article);
    formData.append("stage", this.addAModel.stage);
    formData.append("ver", this.addAModel.ver.toString());
    formData.append("remark", this.addAModel.remark);
    formData.append("updateUser", this.addAModel.upUsr);
    formData.append("file", this.bufferFile);

    this.utility.spinner.show();

    this.dksService.addBOMfile(formData).subscribe(
      (res) => {
        this.utility.spinner.hide();
        this.closeModal("Modal1");
        //refresh the page
        this.search();
        this.utility.alertify.success("Save success !");
      },
      (error) => {
        this.utility.spinner.hide();
        this.utility.alertify.error("Add failed !!!!");
      }
    );
  }
  saveApplyBtn() {
    this.utility.alertify.confirm(
      "System Alert",
      "Are you sure you want to apply?",
      () => {
        var formData = new FormData();
        formData.append("factoryId", this.addAModel.factory);
        formData.append("article", this.addAModel.article);
        formData.append("ver", this.addAModel.ver.toString());
        formData.append("remark", this.addAModel.remark);
        formData.append("loginUser", this.sDevBomFile.loginUser);
        this.utility.spinner.show();
        this.dksService.applyBOMfile(formData).subscribe(
          (res) => {
            this.utility.spinner.hide();
            this.closeModal("Modal3");
            //refresh the page
            this.search();
            this.utility.alertify.success("Apply success!");
          },
          (error) => {
            this.utility.spinner.hide();
            this.utility.alertify.error("Add file failed !!!!");
          }
        );
      }
    );
    

  }

  //開窗 Start
  applyBtn(model: DevBomFileDetailDto) {
    if(this.loginTeam.includes('99')){
      this.utility.alertify.error(`Normal User can not operate it !!`);
      return;
    }
    if(this.loginTeam.includes(model.devTeamId)){
      this.openModal("Modal3");
      this.copyToAddAModel(model);
    }else{
      this.utility.alertify.error(`You don't have permission to do ${model.teamName}!!`);
      return;
    }

  }
  remarkBtn(model: DevBomFileDetailDto) {
    this.openModal("Modal2");
    this.copyToAddAModel(model);
  }  
  uploadBtn(model: DevBomFileDetailDto) {
    if(this.loginTeam.includes('99')){
      this.utility.alertify.error(`Normal User can not operate it !!`);
      return;
    }
    this.openModal("Modal1");
    this.copyToAddAModel(model);
    this.addAModel.ver += 1;
  }
  //開窗 End  

  downloadBtn(model: DevBomFileDetailDto) {
    let theKey2Download = false;
    if(this.loginTeam.includes('99')){
      console.log("the user is belong team 99.");
      const gp = this.result
      .filter(i => i.factoryId === model.factoryId && i.article === model.article && i.apply === 'Y' );
      const maxVer = gp.reduce((max,curr)=>{
        return curr.ver > max.ver ? curr: max
      });
      if(maxVer.ver > model.ver){
        this.utility.alertify.error(`Normal User only can download the newest BOM (Ver:${maxVer.ver}) !!`);
      }else{
        theKey2Download = true;
      }
    }else{
      theKey2Download = true;
    }

    if(theKey2Download){
      let dataUrl =
      "../assets/F340PpdPic/ArticleBoms/" + model.season + "/" + model.article + "/" + model.fileName;
        window.open(dataUrl);
    }
    
  }

  copyToAddAModel(model: DevBomFileDetailDto) {
    this.addAModel.factory = model.factoryId;
    this.addAModel.devTeamId = model.devTeamId;
    this.addAModel.season = model.season;
    this.addAModel.modelNo = model.modelNo;
    this.addAModel.modelName = model.modelName;
    this.addAModel.article = model.article;
    this.addAModel.stage = model.stage;
    this.addAModel.ver = model.ver;
    this.addAModel.fileName = model.fileName;
    this.addAModel.remark = model.remark;
    this.addAModel.apply = model.apply;
    this.addAModel.upUsr = this.sDevBomFile.loginUser;
  }
}
