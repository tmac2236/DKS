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
  
  title = "Maintain-RFID";
  result: PrdRfidAlertDto[] = [];
  sReactime: SRfidMaintain = new SRfidMaintain();
  uiControls:any = {
    sendMemoMail: utilityConfig.DtrUnfozen,
  };
  selectedList:PrdRfidAlertDto[] = []; //checkbox用
  isAllCheck = false; //全選checkbox用
  addModal:DevGateLogDataLog = new DevGateLogDataLog();


  constructor(public utility: Utility, private systemService: SystemService, private commonService: CommonService) { }

  ngOnInit() {
    //this.utility.initUserRole(this.sReactime);

  }
  //搜尋
  search() {
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
  if (type == "addRfidModal") this.addRfidModal.show();
  
}
closeModal(type: string) {
  if (type == "addRfidModal") this.addRfidModal.hide();
}
saveModal(){
  this.utility.spinner.show();
  this.systemService.setRfidAlerts(this.selectedList,this.addModal.reason,this.addModal.updater).subscribe(
    (res) => {
      this.utility.spinner.hide();
      this.utility.alertify.confirm(
        "Sweet Alert",
        "You Updated Comment.",
        () => { this.closeModal('addRfidModal') });  
    },
    (error) => {
      this.utility.spinner.hide();
      this.utility.alertify.error(error);
    }
  );
}


}
