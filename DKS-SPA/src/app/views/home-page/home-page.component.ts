import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { BsDropdownConfig } from "ngx-bootstrap/dropdown";
import { NgxSpinnerService } from "ngx-spinner";
import { AlertifyService } from "../../core/_services/alertify.service";
import { AuthService } from "../../core/_services/auth.service";
import { Utility } from "../../core/utility/utility";

@Component({
  selector: "app-home-page",
  templateUrl: "./home-page.component.html",
  styleUrls: ["./home-page.component.scss"],
  providers: [
    {
      provide: BsDropdownConfig,
      useValue: { isAnimated: true, autoClose: true },
    },
  ],
})
export class HomePageComponent implements OnInit {
  loginModel: any = {};
  photoUrl: string;
  param1: string; //userID or LOGIN
  param2: string; //Path
  param3: string; //Parameters
  formList: { id: number, name: string,path: string }[] = [
    { "id": 1, "name": "BOM Upload Manage","path":"/BOM-Manage" }
  ];
  formType:string;
  constructor(
    public authService: AuthService,
    private alertify: AlertifyService,
    private router: Router,
    private activeRouter: ActivatedRoute,
    private spinner: NgxSpinnerService,
    public utility: Utility
  ) {
    this.activeRouter.queryParams.subscribe((params) => {

      this.param1 = params.A0Lfn93DlC; //userID or LOGIN
      this.param2 = params.DWgu5gtmmT; //Path
      this.param3 = params.Z7kXu2OaRm; //Parameters
    });
  }

  ngOnInit() {
    if (typeof this.param1 !== "undefined") {
      this.loginByDKS(this.param1,this.param2,this.param3);
    }
    //this.router.navigate(["/F340"], {
    //  queryParams: { param1: this.param1 },
    //  skipLocationChange: false,
    //});
  }

  loginSystem() {
    this.spinner.show();
    this.authService.loginByPage(this.loginModel).subscribe(
      (next) => {
        this.spinner.hide();
        this.alertify.success("Logined in sucessed");
        if(!this.utility.checkIsNullorEmpty(this.formType)){
          this.router.navigate([this.formType]);
        }else{
          this.router.navigate([""]);
        }

      },
      (error) => {
        this.spinner.hide();
        this.alertify.error(error);
        this.router.navigate([""]);
      }
    );
  }
  loginByDKS(userID: string, path: string, parameters?:string) {

    this.spinner.show();
    this.loginModel.account = userID;
    this.authService.login(this.loginModel).subscribe(
      (next) => {
        this.spinner.hide();
        let PathCode = '/' + path; 

        if (typeof this.param3 !== "undefined") { //url have parameters
          var navigationExtras = {
            queryParams: {
              homeParam : parameters
            },
            skipLocationChange: false,
          };
          this.router.navigate([PathCode], navigationExtras);
        }else{

          this.router.navigate([PathCode]);
        }

      },
      (error) => {
        this.spinner.hide();
        this.alertify.error(error);
        this.router.navigate([""]);
      }
    );
  }

}
