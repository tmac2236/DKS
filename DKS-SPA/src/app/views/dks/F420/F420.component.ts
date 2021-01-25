import { Component, OnInit } from "@angular/core";
import { Utility } from "../../../core/utility/utility";
import { DksService } from "../../../core/_services/dks.service";

@Component({
  selector: "app-F420",
  templateUrl: "./F420.component.html",
  styleUrls: ["./F420.component.scss"],
})
export class F420Component implements OnInit {
  constructor(private dksService: DksService, private utility: Utility) {}
  article = "";
  ngOnInit() {}
  handleFileInput(files: FileList) {
    this.utility.spinner.show();
    var formData = new FormData(); //共用請小心
    formData.append("file", files.item(0));
    this.save(formData);
  }
  reset(e) {
    console.log("log", e);
    e.target.value = "";
  }
  save(data: FormData) {
    this.dksService.checkF420Valid(data).subscribe(
      (res: Array<Object>) => {
        this.utility.spinner.hide();
        if (res.length > 0) {
          var str = "";
          for (var i = 0; i < res.length; i++) {
            str += res[i]["proordno"];
            str += "(";
            str += res[i]["needqty"];
            str += ")";
            str += "\r\n"; //換行
          }
          //alert(str);
          this.utility.alertify.confirm(
            "The Qty of these Order# exceed the required qty, Please check your excel then upload again .\r\n" +
              str,
            () => {} //doNothing
          );
        }else{
          this.utility.alertify.confirm(
            "You can upload this file to DKS Sysyem !\r\n",
            () => {} //doNothing
          );
        }
      },
      (error) => {
        this.utility.spinner.hide();
        this.utility.alertify.error("failed please check your excel !!!!");
      }
    );
  }
}
