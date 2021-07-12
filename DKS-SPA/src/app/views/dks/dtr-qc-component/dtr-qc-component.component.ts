import { Component, OnInit, ViewChild } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { environment } from "../../../../environments/environment";
import { Utility } from "../../../core/utility/utility";
import { utilityConfig } from "../../../core/utility/utility-config";
import { DtrFgt } from "../../../core/_models/dtr-fgt";
import { DksService } from "../../../core/_services/dks.service";

@Component({
  selector: "app-dtr-qc-component",
  templateUrl: "./dtr-qc-component.component.html",
  styleUrls: ["./dtr-qc-component.component.scss"],
})
export class DtrQcComponentComponent implements OnInit {
  //for hint
  hintMsg: any = {
    uploadPdf: "Please upload pdf file and size cannot over 2 Mb.",
  };

  model: DtrFgt = new DtrFgt();
  //memoBtn = true;
  uiControls: any = {
    uploadPicF340Ppd: utilityConfig.RolePpdPic,
  };

  constructor(public utility: Utility, private dksService: DksService,private activeRouter: ActivatedRoute,
    private route: Router) { 
    
    this.activeRouter.queryParams.subscribe((params) => {
          this.model.article = params.article;
          this.model.stage = params.stage;
          this.model.kind = params.kind;
          this.model.vern = params.vern;
  });}

  ngOnInit() {
    //this.model.upusr = this.utility.getToken("unique_name");

  }
  //上傳 PDF DTR QC
  uploadPdfDtrQc(files: FileList, model: DtrFgt) {
    console.log(model);
    //"application/pdf" "image/jpeg"
    if (
      !this.utility.checkFileMaxFormat(files.item(0), 1128659 * 2, "application/pdf")
    ) {
      this.utility.alertify.confirm(
        "Sweet Alert",
        this.hintMsg.uploadPdf,
        () => {}
      );
      return; //exit function
    }
    var formData = new FormData();
    formData.append("file", files.item(0));
    formData.append("article", model.article);
    formData.append("stage", model.stage);
    formData.append("kind", model.kind);
    formData.append("vern", model.vern.toString());

    this.utility.spinner.show();
    this.dksService.editDtrQc(formData).subscribe(
      (res) => {
        this.utility.spinner.hide();
        //找到該筆model 把資料回填
        model.fileName = res.fileName;
      },
      (error) => {
        this.utility.spinner.hide();
        this.utility.alertify.error("Add failed !!!!");
      }
    );
  }

  //刪除 PDF DTR QC
  removePdfDtrQc(model: DtrFgt) {
    var formData = new FormData();
    formData.append("file", null); // upload null present delete
    formData.append("article", model.article);
    formData.append("stage", model.stage);
    formData.append("kind", model.kind);
    formData.append("vern", model.vern.toString());
    this.utility.alertify.confirm(
      "Sweet Alert",
      "Are you sure to Delete this pdf of article:" +
        model.article +
        ", stage:" +
        model.stage +
        ", kind:" +
        model.kind +
        ", version No:" +
        model.vern +      
        ".",
      () => {
        this.utility.spinner.show();
        this.dksService.editDtrQc(formData).subscribe(
          () => {
            this.utility.spinner.hide();
            this.utility.alertify.success("Delete succeed!");
            model.fileName = ""; // make the url of pdf to blank
          },
          (error) => {
            this.utility.spinner.hide();
            this.utility.alertify.error("Delete failed !!!!");
          }
        );
      }
    );
  }
  viewPdf(model: DtrFgt) {
    let url ="";
    window.open(url);
  }
}
