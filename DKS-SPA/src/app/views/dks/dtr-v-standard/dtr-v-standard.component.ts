import { Component, OnInit, ViewChild } from "@angular/core";
import { ModalDirective } from "ngx-bootstrap/modal";
import { Utility } from "../../../core/utility/utility";
import { DtrService } from "../../../core/_services/dtr.service";
import { SDevDtrVisStandard } from "../../../core/_models/s-dev-dtr-vis-standard";
import { DevDtrVisStandard } from "../../../core/_models/dev-dtr-vis-standard";

@Component({
  selector: "app-dtr-v-standard",
  templateUrl: "./dtr-v-standard.component.html",
  styleUrls: ["./dtr-v-standard.component.scss"],
})
export class DtrVStandardComponent implements OnInit {
  @ViewChild("addDtrVSModal") public addDtrVSModal: ModalDirective;
  //for hint
  hintMsg: any = {
    uploadPdf: "Please upload pdf file and size cannot over 2 Mb.",
  };
  sDevDtrVisStandard: SDevDtrVisStandard = new SDevDtrVisStandard();
  validArticle: boolean = false;
  result: DevDtrVisStandard[] = [];
  addAModel: DevDtrVisStandard = new DevDtrVisStandard();
  bufferFile: File | null = null; // upload

  constructor(public utility: Utility, private dtrService: DtrService) {}

  ngOnInit() {
    this.utility.initUserRole(this.sDevDtrVisStandard);
    this.addAModel.upusr = this.sDevDtrVisStandard.loginUser;
  }
  //搜尋
  async search() {
    this.validArticle = true;

    let flo = new DevDtrVisStandard(); //fake
    flo.id = "10";
    this.result.push(flo); //fake
    let flo1 = new DevDtrVisStandard(); //fake
    flo1.id = "2";
    this.result.push(flo1); //fake
  }
  //Add dtr vs pdf
  openAddVisStandardModal(type: string) {
    this.cleanModel();
    if (type == "addDtrVSModal") {
      this.openModal("addDtrVSModal");
      this.addAModel.season = this.sDevDtrVisStandard.season;
      this.addAModel.article = this.sDevDtrVisStandard.article;
      //get the max number in list
      let maxVal = Math.max.apply(
        Math,
        this.result.map(function (o) {
          return o.id;
        })
      );
      maxVal += 1;
      this.addAModel.id = maxVal;
    }
  }
  openModal(type: string) {
    if (type == "addDtrVSModal") this.addDtrVSModal.show();
  }
  closeModal(type: string) {
    if (type == "addDtrVSModal") this.addDtrVSModal.hide();
  }
  initAddBtn() {
    this.validArticle = false;
  }
  cleanModel() {}
  //save the file to memory
  handleFileInput(files: FileList) {
    //"application/pdf" "image/jpeg"
    if (
      !this.utility.checkFileMaxFormat(
        files.item(0),
        1128659 * 2,
        "application/pdf"
      )
    ) {
      this.utility.alertify.confirm(
        "Sweet Alert",
        this.hintMsg.uploadPdf,
        () => {}
      );
      return; //exit function
    }
    this.bufferFile = files.item(0);
  }
  //上傳pdf and Comment
  savePdfNComment() {
    var formData = new FormData();
    formData.append("file", this.bufferFile);
    formData.append("season", this.addAModel.season);
    formData.append("article", this.addAModel.article);
    formData.append("id", this.addAModel.id);
    formData.append("loginUser", this.sDevDtrVisStandard.loginUser);
    this.utility.spinner.show();
    this.dtrService.addVSfile(formData).subscribe(
      (res) => {
        this.utility.spinner.hide();
        //找到該筆model 把資料回填
        this.closeModal("addFgtResult"); //新增一筆record後上傳pdf成功關閉modal
      },
      (error) => {
        this.utility.spinner.hide();
        this.utility.alertify.error("Add failed !!!!");
      }
    );
  }
  viewPdf(item: object) {
    let dataUrl =
      "../assets/F340PpdPic/VisStandard/" +
      item["article"] +
      "/" +
      item["fileName"];
    window.open(dataUrl);
  }
}
