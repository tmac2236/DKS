import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { Utility } from "../../core/utility/utility";
import { Picture } from "../../core/_models/picture";
import { DksService } from "../../core/_services/dks.service";

@Component({
  selector: "app-picture",
  templateUrl: "./picture.component.html",
  styleUrls: ["./picture.component.scss"],
})
export class PictureComponent implements OnInit {
  fakeArray = new Array<string>("1", "2", "3", "4", "5", "6");

  article = "";
  user: string;
  imageObject: Array<Picture> = new Array<Picture>();
  constructor(
    private route: ActivatedRoute,
    private dksService: DksService,
    private utility: Utility
  ) {
    this.route.queryParams.subscribe((params) => {
      this.article = params.article;
      this.user = params.A0Lfn93DlC;
    });
  }

  ngOnInit() {
    this.loadpictures();
  }

  handleFileInput(files: FileList, no: string) {
    if (!this.checkFileFormat(files.item(0))){
      this.utility.alertify.confirm(
        "Please upload jpg file and size cannot over 1 Mb.",
        () => {});
      
      return; //exit function
    }
    
    var formData = new FormData(); //共用請小心
    formData.append("article", this.article);
    formData.append("user", this.user);
    formData.append("file", files.item(0));
    formData.append("no", no);
    this.save(formData);
  }
  checkFileFormat(file: File) {
    var isLegal = true;
    if (file.type != "image/jpeg") isLegal = false;
    if (file.size >= 1128659) isLegal = false; //最大上傳1MB
    return isLegal;
  }
  save(data: FormData) {
    this.dksService.uploadPicByArticle(data).subscribe(
      () => {
        this.utility.alertify.success("Add succeed!");
        location.reload();
      },
      (error) => {
        this.utility.alertify.error("Add failed !!!!");
      }
    );
  }

  remove(no: string) {
    var formData = new FormData(); //共用請小心
    formData.append("article", this.article);
    formData.append("user", this.user);
    formData.append("file", null);
    formData.append("no", no);
    this.utility.alertify.confirm(
      "Are you sure to Delete this picture ?",
      () => {
        this.dksService.deletePicByArticle(formData).subscribe(
          () => {
            location.reload();
            this.utility.alertify.success("Delete succeed!");
          },
          (error) => {
            this.utility.alertify.error("Delete failed !!!!");
          }
        );
      }
    );
  }

  loadpictures() {
    for (var i = 1; i <= 6; i++) {
      //六張圖
      var pic = new Picture();
      let imgurl =
        "../../../assets/ArticlePics/" +
        this.article +
        "/" +
        this.article +
        "_" +
        i +
        ".jpg";
      pic.image = imgurl;
      pic.thumbImage = imgurl;
      this.imageObject.push(pic);
    }
  }
}
