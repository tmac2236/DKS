import { Component, OnInit } from "@angular/core";
import { Utility } from "../../../core/utility/utility";
import { DksService } from "../../../core/_services/dks.service";
import { JwtHelperService } from "@auth0/angular-jwt";

@Component({
  selector: "app-F340",
  templateUrl: "./F340.component.html",
  styleUrls: ["./F340.component.scss"],
})
export class F340Component implements OnInit {
  //for sorting ; ASC DESC
  cwaDeadlineS = true;
  //
  loginUser: string;
  season: string;
  bpVer = "";
  result: object[];
  jwtHelper = new JwtHelperService();
  constructor(private utility: Utility, private dksService: DksService) {}

  ngOnInit() {
    const jwtTtoken = localStorage.getItem("token");
    if (jwtTtoken) {
      this.loginUser = this.jwtHelper.decodeToken(jwtTtoken)["unique_name"];
    }
  }
  search() {
    this.utility.spinner.show();
    this.dksService.searchF340Process(this.season, this.bpVer).subscribe(
      (res) => {
        this.result = res;
        this.utility.spinner.hide();
      },
      (error) => {
        this.utility.spinner.hide();
        this.utility.alertify.error(error);
      }
    );
  }
  sort(e) {
    //console.log(e);
    let headStr = e.target.innerHTML;

    switch (headStr) {
      case "cwaDeadline":
        if (this.cwaDeadlineS) {
          //ASC
          this.result = this.result.sort((a, b) =>
            a["cwaDeadline"].localeCompare(b["cwaDeadline"])
          );
        } else {
          //DESC
          this.result = this.result.sort((a, b) =>
            b["cwaDeadline"].localeCompare(a["cwaDeadline"])
          );
        }
        this.cwaDeadlineS = !this.cwaDeadlineS;
        break;
      default:
        alert("Hello Default");
        break;
    }
    //type = type =="ASC"?"DESC":"ASC";
  }
  export() {
    this.utility.spinner.show();
    this.utility.http
      .get(
        this.utility.baseUrl +
          "dks/exportF340_Process?season=" +
          this.season +
          "&bpVer=" +
          this.bpVer,
        { responseType: "blob" }
      )
      .subscribe((result: Blob) => {
        if (result.type !== "application/xlsx") {
          alert(result.type);
          this.utility.spinner.hide();
        }
        const blob = new Blob([result]);
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement("a");
        const currentTime = new Date();
        const filename =
          "F340_Schedule" +
          currentTime.getFullYear().toString() +
          (currentTime.getMonth() + 1) +
          currentTime.getDate() +
          currentTime
            .toLocaleTimeString()
            .replace(/[ ]|[,]|[:]/g, "")
            .trim() +
          ".xlsx";
        link.href = url;
        link.setAttribute("download", filename);
        document.body.appendChild(link);
        link.click();
        this.utility.spinner.hide();
      });
  }
  useLanguage(language: string) {
    this.utility.languageService.setLang(language);
  }
}
