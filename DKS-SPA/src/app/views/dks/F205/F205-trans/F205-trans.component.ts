import { Component, OnInit, ViewChild } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { ModalDirective } from "ngx-bootstrap/modal";
import { Utility } from "../../../../core/utility/utility";
import { ArticleSeason } from "../../../../core/_models/article-season";
import { DevDtrFgtResult } from "../../../../core/_models/dev-dtr-fgt-result";
import { SF205Trans } from "../../../../core/_models/s-f205-trans";
import { CommonService } from "../../../../core/_services/common.service";
import { DtrService } from "../../../../core/_services/dtr.service";

@Component({
  selector: "app-F205-trans",
  templateUrl: "./F205-trans.component.html",
  styleUrls: ["./F205-trans.component.scss"],
})
export class F205TransComponent implements OnInit {
  @ViewChild("transitModal") public transitModal: ModalDirective;
  
  constructor(
    public utility: Utility,
    private activeRouter: ActivatedRoute,
    private dtrService: DtrService,
    private commonService: CommonService
  ) {
    this.activeRouter.queryParams.subscribe((params) => {
      
      if(params.homeParam !== undefined){ //from DKS redirect
        //M19346$M19346$C
        let paramArray = params.homeParam.split("$");
        this.sF205Trans.article = paramArray[0];
        this.sF205Trans.stage = paramArray[1];
        this.factoryIdUrl = paramArray[2];
      }
      this.sF205Trans.factoryId = this.factoryIdUrl;
      if(!this.utility.checkIsNullorEmpty(this.sF205Trans.article)) this.search();

    });
  }
  factoryIdUrl: string; 
  sF205Trans: SF205Trans = new SF205Trans();
  result: ArticleSeason[] = [];
  transitModel: DevDtrFgtResult = new DevDtrFgtResult(); //use in upgradeModelModal
  stageList: { id: number, name: string, code: string}[] =[];
  oStageList: { id: number, name: string, code: string }[] = [
    { "id": 1, "name": "C","code":"SHC" },
    { "id": 2, "name": "U","code":"TSH" },
    { "id": 3, "name": "D","code":"SPC" },
    { "id": 4, "name": "E","code":"CB" },
  ];

  ngOnInit() {
    this.utility.initUserRole(this.sF205Trans);
  }
  //搜尋
  async search() {
    if (
      !this.utility.checkIsNullorEmpty(this.sF205Trans.article) &&
      this.sF205Trans.article.trim().length < 6
    ) {
      this.utility.alertify.error("Artilce must be 6 characters  !!!!");
      return;
    }
    if (
      !this.utility.checkIsNullorEmpty(this.sF205Trans.stage) &&
      this.sF205Trans.stage.trim().length < 3
    ) {
      this.utility.alertify.error("Stage must be 3 characters  !!!!");
      return;
    }
    this.utility.spinner.show();
    await this.commonService
      .getArticleSeason('',this.sF205Trans.article,'')
      .then(
        (res) => {
          this.utility.spinner.hide();
          this.result = res.filter( (x)=> x.stage == this.sF205Trans.stage );
          if (this.result.length < 1) {
            this.utility.alertify.confirm(
              "Sweet Alert",
              "No Data in these conditions of search, please try again.",
              () => {
                return;
              }
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
  openModal(type: string) {
    if (type == "transit") this.transitModal.show();
  }
  closeModal(type: string) {
    if (type == "transit") this.transitModal.hide();
  } 
  cleanTransit(){
    this.transitModel = new DevDtrFgtResult();
    this.transitModel.upusr = this.sF205Trans.loginUser;
  } 
  //Transit article
  transit() {
    if (false) {
      this.utility.alertify.error(
        "Can not upgrade stage from CS3!"
      );
      return;
    }

    this.cleanTransit();

    //let a = this.oStageList.find( (x)=> x.name == this.sF205Trans.factoryId );
    let usedfacArray = this.result.map(x => x.factoryId);
    this.stageList = this.oStageList.filter( (x)=> !usedfacArray.includes(x.name) );

    this.openModal("transit");
  }
  transitArticle(){
    alert("pending.");
  }  
}
