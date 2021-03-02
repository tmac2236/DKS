import { Component, OnInit } from '@angular/core';
import { JwtHelperService } from "@auth0/angular-jwt";
import { Utility } from '../../../core/utility/utility';
import { F428SampleNoDetail } from '../../../core/_models/f428-sample-no-detail';
import { SF428SampleNoDetail } from '../../../core/_models/s-f428-sample-no-detail';
import { DksService } from '../../../core/_services/dks.service';


@Component({
  selector: 'app-F428',
  templateUrl: './F428.component.html',
  styleUrls: ['./F428.component.scss']
})
export class F428Component implements OnInit {
  //getUserName
  jwtHelper = new JwtHelperService();
  loginUser: string;

  sF428SampleNoDetail: SF428SampleNoDetail = new SF428SampleNoDetail();
  result: F428SampleNoDetail[];

  constructor(public utility: Utility, private dksService: DksService) {}

  ngOnInit() {
    this.setAccount();
  }
  //取得登入帳號
  setAccount(){
    const jwtTtoken = localStorage.getItem("token");
    if (jwtTtoken) {
      this.loginUser = this.jwtHelper.decodeToken(jwtTtoken)["unique_name"];
    }
  }
  //設定語言
  useLanguage(language: string) {
    this.utility.languageService.setLang(language);
  }
  pageChangeds(event: any): void {
    this.sF428SampleNoDetail.currentPage = event.page;
    this.search();
  }
  
  search(){
    alert('search');
  }
  export(){
    alert('export');
  }
}
