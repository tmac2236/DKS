import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { Utility } from '../../../../core/utility/utility';
import { utilityConfig } from '../../../../core/utility/utility-config';
import { PaginatedResult } from '../../../../core/_models/pagination';
import { PlmPart } from '../../../../core/_models/plm-part';
import { SPlmPart } from '../../../../core/_models/s_plm-part';
import { PlmService } from '../../../../core/_services/plm.service';

@Component({
  selector: 'app-plm-part',
  templateUrl: './plm-part.component.html',
  styleUrls: ['./plm-part.component.scss']
})
export class PlmPartComponent implements OnInit {
  @ViewChild("addModal") public addModal: ModalDirective;
  @ViewChild("editModal") public editModal: ModalDirective;
  
  //for hint
  hintMsg: any = {
    uploadPdf: "Please upload pdf or excel file and size cannot over 2 Mb.",
  };
  uiControls: any = {
    editPassData: utilityConfig.RoleSysAdm,
  };
  title = "PLM Part Maintain";
  searchCondition: SPlmPart = new SPlmPart();
  result: PlmPart[] = [];

  reNameList: { id: number, name: string, code: string }[] = [
    { "id": 0, "name": "Y","code":"Y" },
    { "id": 1, "name": "N","code":"N" },
  ];   

  addAModel: PlmPart = new PlmPart(); //use in add and update

  constructor(public utility: Utility, private activeRouter: ActivatedRoute
    , private plmService: PlmService ) {}

  ngOnInit() {
    this.utility.initUserRole(this.searchCondition);
    this.initModel(); 
    //this.search();
    console.log(new Date().toLocaleString());
  }
  //分頁按鈕
  pageChangeds(event: any): void {
      this.searchCondition.currentPage = event.page;
      this.search();
  }
  //搜尋
  async search() {

    //check
    this.utility.spinner.show();
    this.plmService
      .getPlmPart(this.searchCondition)
      .subscribe(
        (res: PaginatedResult<PlmPart[]>) => {
          if (res.result.length < 1) {
            this.utility.alertify.confirm(
              "Sweet Alert",
              "No Data in these conditions of search.",
              () => {
                this.utility.spinner.hide();
                return;
              }
            );
          }
          this.result = res.result;
          this.searchCondition.setPagination(res.pagination);
          this.utility.spinner.hide();
        },
        (error) => {
          this.utility.spinner.hide();
          this.utility.alertify.confirm(
            "System Notice",
            ` Please scan the screen and steps of your operation as more as possible, then report to Aven, Sorry. Dear: ${this.searchCondition.loginUser}, Time: ${this.utility.datepiper.transform(new Date(), 'yyyy-MM-dd HH:mm:ss')}`,
            () => {}
          );
        }
      );
  }

  export() {
    const url = this.utility.baseUrl + "plm/exportPlmPart";
    this.utility.exportFactory(url, "PLM Part No", this.searchCondition);
  }

  openModal(type: string) {
    if (type == "addModal") this.addModal.show();
    if (type == "editModal") this.editModal.show();
  }
  closeModal(type: string) {
    if (type == "addModal") this.addModal.hide();
    if (type == "editModal") {
      this.search();
      this.editModal.hide();
    }
  }
  //Add or update a result of fgt
  async openAddModal() {
    this.initModel();
    this.openModal("addModal");
  }

  openEditModal(model:PlmPart){
   this.addAModel = model;
   this.addAModel.changeuser = this.searchCondition.loginUser;
   this.openModal("editModal");

  }

  initModel() {
    this.addAModel = new PlmPart();
    this.addAModel.changeuser = this.searchCondition.loginUser;
    this.addAModel.rename = "Y"; //defualt is Yes
  }
  async save(){
    if(this.checkModelValid(this.addAModel)) return;
    let isExist = await this.plmService.checkPartNoIsExist( this.addAModel.partno);
    if (isExist) {
      this.utility.alertify.error("The Part No is exist in the sysyem. Please use another one !");
      return;
    }
    this.addAModel.insertuser = this.searchCondition.loginUser;

    this.utility.spinner.show();
    this.plmService.addPlmPart(this.addAModel).subscribe(
      (res:boolean) => {
        this.utility.spinner.hide();
        this.closeModal("addModal");
        this.search();
        this.utility.alertify.success("Save Success !");
      },
      (error) => {
        this.utility.spinner.hide();
        this.utility.alertify.confirm(
          "System Notice",
          error,
          () => {}
        );
      }
    );

  }
 delete(model: PlmPart) {
    model.changeuser = this.searchCondition.loginUser;
    this.utility.alertify.confirm(
      "Sweet Alert",
      "Are you sure to Delete this Part No :" + model?.partno +  ".",
      () => {
        this.utility.spinner.show();
        this.plmService.deletePlmPart(model).subscribe(
          (res:boolean) => {
            this.utility.spinner.hide();
            this.search();
            this.utility.alertify.success("Delete Success !");
          },
          (error) => {
            this.utility.spinner.hide();
            this.utility.alertify.confirm(
              "System Notice",
              ` Please scan the screen and steps of your operation as more as possible, then report to Aven, Sorry. Dear: ${this.searchCondition.loginUser}, Time: ${this.utility.datepiper.transform(new Date(), 'yyyy-MM-dd HH:mm:ss')}`,
              () => {}
            );
          }
        );
      }
    );
  }
 edit(){
    if(this.checkModelValid(this.addAModel)) return;

    this.utility.spinner.show();
    this.plmService.updatePlmPart(this.addAModel).subscribe(
      (res:boolean) => {
        this.utility.spinner.hide();
        this.closeModal("editModal");
        this.search();
        this.utility.alertify.success("Update Success !");
      },
      (error) => {
        this.utility.spinner.hide();
        this.utility.alertify.confirm(
          "System Notice",
          ` Please scan the screen and steps of your operation as more as possible, then report to Aven, Sorry. Dear: ${this.searchCondition.loginUser}, Time: ${this.utility.datepiper.transform(new Date(), 'yyyy-MM-dd HH:mm:ss')}`,
          () => {}
        );
      }
    );
  }

  checkModelValid(model:PlmPart){
    let isNumber = this.utility.checkIsNumber(this.addAModel.partno);
    if (!isNumber) {
      this.utility.alertify.error("The Part                                                                                                                                                                                                                                                                            No have to be a number !");
      return true;
    }
    if (this.addAModel.partno.length != 4) {
      this.utility.alertify.error("The length of Part No have to be 4 !");
      return true;
    }
    if (this.utility.checkIsNullorEmpty(this.addAModel.location)) {
      this.utility.alertify.error("The Location can not be empty !");
      return true;
    }
    if (this.utility.checkIsNullorEmpty(this.addAModel.partnameen)) {
      this.utility.alertify.error("The Part Name(EN) can not be empty !");
      return true;
    }
    if (this.utility.checkIsNullorEmpty(this.addAModel.partnamecn)) {
      this.utility.alertify.error("The Part Name(CN) can not be empty !");
      return true;
    }
    if (this.utility.checkIsNullorEmpty(this.addAModel.rename)) {
      this.utility.alertify.error("The Rename can not be empty !");
      return true;
    }
    return false;
  }
}
