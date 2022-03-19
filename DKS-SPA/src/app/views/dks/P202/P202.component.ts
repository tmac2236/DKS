import { Component, OnInit } from '@angular/core';
import { Utility } from '../../../core/utility/utility';
import { BasicCodeDto } from '../../../core/_models/basic-code-dto';
import { SP202 } from '../../../core/_models/s_p202';
import { CommonService } from '../../../core/_services/common.service';
@Component({
  selector: 'app-P202',
  templateUrl: './P202.component.html',
  styleUrls: ['./P202.component.scss']
})
export class P202Component implements OnInit {

  title = "P202 BOM Export";
  sCondtion: SP202 = new SP202();
  code034: BasicCodeDto[] = []; //034 BrandCategory
  constructor(public utility: Utility,private commonService: CommonService) {}

  async ngOnInit() {
    await this.getBasicCodeDto();
    this.utility.initUserRole(this.sCondtion);

  }
  isCannotExportable(){
    let flag = true;
    if(!this.utility.checkIsNullorEmpty(this.sCondtion.season)) flag = false;
    if(!this.utility.checkIsNullorEmpty(this.sCondtion.brand)) flag = false;
    if(!this.utility.checkIsNullorEmpty(this.sCondtion.modelName)) flag = false;
    if(!this.utility.checkIsNullorEmpty(this.sCondtion.modelNo)) flag = false;
    if(!this.utility.checkIsNullorEmpty(this.sCondtion.article)) flag = false;
    return flag;
  }
  export() {
    const url = this.utility.baseUrl + "excel/getP202BySeason";
    this.utility.exportFactory(url, "P202Export", this.sCondtion);
  }
  async getBasicCodeDto() {
    await this.commonService
      .getBasicCodeDto(
        '034'
      )
      .then(
        (res) => {
          this.code034 = res
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
