import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { BsDropdownConfig } from "ngx-bootstrap/dropdown";
import { NgxSpinnerService } from "ngx-spinner";
import { AlertifyService } from "../../core/_services/alertify.service";
import { AuthService } from "../../core/_services/auth.service";

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
  param1: string;
  param2: string;

  constructor(
    public authService: AuthService,
    private alertify: AlertifyService,
    private router: Router,
    private activeRouter: ActivatedRoute,
    private spinner: NgxSpinnerService,
  ) {
    this.activeRouter.queryParams.subscribe((params) => {

      this.param1 = params.A0Lfn93DlC; //userID or LOGIN
      this.param2 = params.DWgu5gtmmT; //Path
    });
  }

  ngOnInit() {
    if (typeof this.param1 !== "undefined") {
      this.loginByDKS(this.param1,this.param2);
    }
    //this.router.navigate(["/F340"], {
    //  queryParams: { param1: this.param1 },
    //  skipLocationChange: false,
    //});
  }

  loginSystem() {
    this.spinner.show();
    this.authService.login(this.loginModel).subscribe(
      (next) => {
        this.spinner.hide();
        this.alertify.success("Logined in sucessed");
        this.router.navigate(["excel/compare"]);
      },
      (error) => {
        this.spinner.hide();
        this.alertify.error(error);
        this.router.navigate([""]);
      }
    );
  }
  loginByDKS(userID: string, path: string) {

    this.spinner.show();
    this.loginModel.account = userID;
    this.authService.login(this.loginModel).subscribe(
      (next) => {
        this.spinner.hide();
        let PathCode = '/' + path;
        //this.alertify.success(PathCode);
        this.router.navigate([PathCode]);
      },
      (error) => {
        this.spinner.hide();
        this.alertify.error(error);
        this.router.navigate([""]);
      }
    );
  }

}
