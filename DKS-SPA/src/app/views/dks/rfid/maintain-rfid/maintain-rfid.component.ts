import { Component, OnInit,ViewChild } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { Utility } from '../../../../core/utility/utility';
import { utilityConfig } from '../../../../core/utility/utility-config';
import { DevGateLogDataLog } from '../../../../core/_models/dev-gate-log-data-log';
import { PaginatedResult } from '../../../../core/_models/pagination';
import { PrdRfidAlertDto } from '../../../../core/_models/prd-rfid-alert-dto';
import { SRfidMaintain } from '../../../../core/_models/s_rfid_maintain';
import { CommonService } from '../../../../core/_services/common.service';
import { SystemService } from '../../../../core/_services/system.service';

@Component({
  selector: 'app-maintain-rfid',
  templateUrl: './maintain-rfid.component.html',
  styleUrls: ['./maintain-rfid.component.scss']
})
export class MaintainRfidComponent implements OnInit {
  @ViewChild("addRfidModal") public addRfidModal: ModalDirective;
  @ViewChild("loginModal") public loginModal: ModalDirective;
  
  title = "Maintain-RFID";
  result: PrdRfidAlertDto[] = [];
  sReactime: SRfidMaintain = new SRfidMaintain();
  uiControls:any = {
    sendMemoMail: utilityConfig.DtrUnfozen,
  };
  selectedList:PrdRfidAlertDto[] = []; //checkbox用
  isAllCheck = false; //全選checkbox用
  addModal:DevGateLogDataLog = new DevGateLogDataLog();
  loginModel: any = {}; //登入用

  constructor(public utility: Utility, private systemService: SystemService, private commonService: CommonService) { }

  ngOnInit() {

    this.sReactime.loginUser = localStorage.getItem("user");
    this.addModal.updater = this.sReactime.loginUser;

  }
  //搜尋
  search() {
    let timeS = new Date(this.sReactime.recordTimeS).getTime();
    let timeE = new Date(this.sReactime.recordTimeE).getTime();
    let timeRange = timeE - timeS;
    if (timeRange < 0) {
      this.utility.alertify.error("Search Condition value 'From' have to smaller than 'To' datetime !!!!");
      return;
    }

    if (timeRange > 86400000) {
      this.utility.alertify.error("Search Condition can not over 24 hr !!!!");
      return;
    }
    
    this.utility.spinner.show();
    
    this.systemService.getRfidAlert(this.sReactime).subscribe(
      (res: PaginatedResult<PrdRfidAlertDto[]>) => {

        this.result = res.result;
        this.sReactime.setPagination(res.pagination);
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

    let timeS = new Date(this.sReactime.recordTimeS).getTime();
    let timeE = new Date(this.sReactime.recordTimeE).getTime();
    let timeRange = timeE - timeS;
    if (timeRange < 0) {
      this.utility.alertify.error("Search Condition value 'From' have to smaller than 'To' datetime !!!!");
      return;
    }

    const url = this.utility.baseUrl + "system/exportRfidAlert";
    this.utility.exportFactory(url, "Maintain RFID ", this.sReactime);
  }  
  selectList(){
    this.selectedList = this.result.filter((item) => item.checked);
    console.log(this.selectedList.length);

  }
checkUncheckAll(e) {

  this.selectedList = [];         //先清空

  this.result.forEach(i =>{       //打勾或取消打勾
    i.checked = e.target.checked;
  })

  if(e.target.checked){           //如勾全選
    this.selectedList = this.result ;
  }

}
openModal(type: string) {
  if (type == "addRfidModal"){
    if(this.utility.checkIsNullorEmpty(this.sReactime.loginUser)){
      this.utility.alertify.error("Please Login in first !!!!");
      return;
    }
    this.addRfidModal.show();
  }
  

  if (type == "loginModal") this.loginModal.show();
  
}
closeModal(type: string) {
  if (type == "addRfidModal") this.addRfidModal.hide();
  if (type == "loginModal") this.loginModal.hide();
}
saveModal(){
  this.utility.spinner.show();
  this.systemService.setRfidAlerts(this.selectedList,this.addModal.reason,this.addModal.updater).subscribe(
    (res) => {
      this.utility.spinner.hide();
      this.utility.alertify.confirm(
        "Sweet Alert",
        "You Updated RFID Status.",
        () => { this.closeModal('addRfidModal') 
                this.search();
      });  
    },
    (error) => {
      this.utility.spinner.hide();
      this.utility.alertify.error(error);
    }
  );
}
login(){
  this.utility.loginRuRu(this.loginModel.account,this.loginModel.password).subscribe(
    (res: any) => {

      console.log(res);
      if(res){
        localStorage.setItem('user', this.loginModel.account); //非後端給的token，簡易給個名稱
        this.sReactime.loginUser = localStorage.getItem("user");
        this.addModal.updater = this.sReactime.loginUser;

        this.utility.alertify.success("Login Success !!!!");
        this.closeModal("loginModal");
      }else{
        this.utility.alertify.error("Account or Password is wrong !!!!");
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
  )
}
logout(){
  localStorage.removeItem('user');
  this.sReactime.loginUser = null;
  this.addModal.updater = "";
  this.utility.alertify.success("Logout Success !!!!");
}


}
