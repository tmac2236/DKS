import { Component, OnInit, ViewChild } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { Utility } from '../../../core/utility/utility';
import { DevSysSet } from '../../../core/_models/dev-sys-set';
import { Pagination } from '../../../core/_models/pagination';
import { SystemService } from '../../../core/_services/system.service';

@Component({
  selector: 'app-eng',
  templateUrl: './eng.component.html',
  styleUrls: ['./eng.component.scss']
})
export class EngComponent implements OnInit {
  @ViewChild('editSysSetModal') public editSysSetModal: ModalDirective;

  result: DevSysSet[] = [];
  userInfo:Pagination = new Pagination();

  constructor(public utility: Utility, private systemService: SystemService) { }
  theEditSysSet: DevSysSet = new DevSysSet(); //onlt use in photoCommentModal

  ngOnInit() {
    this.utility.initUserRole(this.userInfo);
    this.getAllSetting();
  }
  getAllSetting(){
    this.utility.spinner.show();
    this.systemService.findAll().subscribe(
      (res) => {
        this.utility.spinner.hide();
        this.result = res;
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
  editSysSet(model:DevSysSet){
    this.openModal("EditSysSetModal");
    this.theEditSysSet = model;
  }
  openModal(type:string){
    if(type == "EditSysSetModal") this.editSysSetModal.show();
  }
  closeModal(type:string){
    if(type == "EditSysSetModal") this.editSysSetModal.hide();
  }
  saveEditSysSet(model: DevSysSet){
    this.utility.spinner.show();
    model.upusr = this.userInfo.loginUser;
    this.systemService.eidtSysSet(model).subscribe(
      (res) => {
        this.utility.spinner.hide();
        this.utility.alertify.confirm(
          "Sweet Alert",
          "You Updated Comment.",
          () => { this.closeModal("EditSysSetModal") });  
      },
      (error) => {
        this.utility.spinner.hide();
        this.utility.alertify.error(error);
      }
    );
  } 
}
