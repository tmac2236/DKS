import { Component, OnInit, ViewChild } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { ModalDirective } from "ngx-bootstrap/modal";
import { Utility } from "../../../../core/utility/utility";
import { ArticleSeason } from "../../../../core/_models/article-season";
import { BasicCodeDto } from "../../../../core/_models/basic-code-dto";
import { SF205Trans } from "../../../../core/_models/s-f205-trans";
import { TransitArticle } from "../../../../core/_models/transit-article";
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
  title = "Transfer Article";
  factoryIdUrl: string; 
  sF205Trans: SF205Trans = new SF205Trans();
  result: ArticleSeason[] = [];
  oriArticle : ArticleSeason;

  transitModel: TransitArticle = new TransitArticle(); //use in transitModal
  facList: { id: number, name: string, code: string}[] =[];
  oFacList: { id: number, name: string, code: string }[] = [
    { "id": 1, "name": "C","code":"SHC" },
    { "id": 2, "name": "U","code":"TSH" },
    { "id": 3, "name": "D","code":"SPC" },
    { "id": 4, "name": "E","code":"CB" },
  ];
  stageList: { id: number, name: string, code: string}[] =[];
  oStageList: { id: number, name: string, code: string }[] = [
    { "id": 1, "name": "CR2","code":"CR2" },
    { "id": 2, "name": "SMS","code":"SMS" },
    { "id": 3, "name": "MCS","code":"MCS" }
  ];  
  code017: BasicCodeDto[] = [];
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
      .getArticleSeason('',this.sF205Trans.article,'')  //撈全部不分廠別
      .then(
        (res) => {
          this.utility.spinner.hide();
          //this.result = res.filter( (x)=> x.stage == this.sF205Trans.stage ); //篩選同stage
          this.result = res;
          this.oriArticle = this.result.find(
            (x) => x.stage == this.sF205Trans.stage.trim() &&
                   x.factoryId == this.sF205Trans.factoryId.trim()
          );
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
    this.transitModel = new TransitArticle();
    this.transitModel.article = this.oriArticle.article;
    this.transitModel.pkArticle = this.oriArticle.pkArticle;
    this.transitModel.modelNo = this.oriArticle.modelNo;
    this.transitModel.modelNoFrom = this.oriArticle.modelNo;
    this.transitModel.factoryIdFrom = this.oriArticle.factoryId;
    this.transitModel.updateUser = this.sF205Trans.userId;
  } 
  //Transit article
  transit() {
    this.cleanTransit();
    let usedfacArray = this.result.map(x => x.factoryId);
    this.facList = this.oFacList.filter( (x)=> !usedfacArray.includes(x.name) );
    let usedStage = this.oStageList.find((x) => x.name == this.oriArticle.stage);
    this.stageList = this.oStageList.filter( (x)=> x.id >= usedStage.id );
    this.openModal("transit");
  }
  async getNewModelNo(){
    this.transitModel.modelNo = this.transitModelNo(this.oriArticle.factoryId,this.transitModel.factoryId,this.oriArticle.modelNo);
    await this.getBasicCodeDto(this.transitModel.factoryId);
  }

  transitModelNo(from:string,to:string,fromModel:string){
    let toModel = "";
    if(from != "C"){
      fromModel = fromModel.substring(1);
    }
    switch (to) {
      case "C": //SHC
          toModel = '' + fromModel;
          break;
      case "E": //CB
          toModel = to + fromModel;
          break;
      case "D": //SPC
          toModel = to + fromModel;
          break;
      case "U": //TSH
          toModel = to + fromModel;
          break;
    }  
    return toModel;
  }
  //check is the season article is exist in Article、Model
  async getBasicCodeDto(factoryId:string) {

    await this.commonService
      .getBasicCodeDto(
        '017'
      )
      .then(
        (res) => {
          this.code017 = res.filter( (x)=> x.param == factoryId );
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
  transitArticle(){
    if (this.utility.checkIsNullorEmpty(this.transitModel.article)) {
      this.utility.alertify.error("Article is required !!!!");
      return;
    }
    if (this.utility.checkIsNullorEmpty(this.transitModel.stage)) {
      this.utility.alertify.error("Stage is required !!!!");
      return;
    }
    if (this.utility.checkIsNullorEmpty(this.transitModel.factoryId)) {
      this.utility.alertify.error("You have to choose a factory to transit the aritcle !!!!");
      return;
    }
    if (this.utility.checkIsNullorEmpty(this.transitModel.devTeamId)) {
      this.utility.alertify.error("You have to choose a Dev TeamId to transit the aritcle !!!!");
      return;
    }
    this.transitModel.email = this.code017.find((x) => x.key == this.transitModel.devTeamId).memoEn1;
    
    this.utility.spinner.show();
    this.dtrService.transitArticle(this.transitModel).subscribe(
      (res) => {
        this.utility.spinner.hide();
        //找到該筆model 把資料回填
        this.closeModal('transit'); //新增一筆成功後關閉modal
        if(this.utility.checkIsNullorEmpty(res)){
          //refresh the page
          this.search();
          this.utility.alertify.success("Transit success !");
        }else{
          this.utility.alertify.error(res);
          return;
        }

      },
      (error) => {
        this.utility.spinner.hide();
        this.utility.alertify.error("Add failed !!!!");
      }
    );
  } 
}
