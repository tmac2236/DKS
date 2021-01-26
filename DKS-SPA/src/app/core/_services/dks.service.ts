import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Utility } from "../utility/utility";
import { ArticlePic } from "../_models/article-pic";

@Injectable({
  providedIn: "root",
})
export class DksService {
  constructor(private utility: Utility) {}

  searchF340Process(startDate: string, endDate: string) {
    return this.utility.http.get<object[]>(
      this.utility.baseUrl +
        "dks/getF340_Process?startDate=" +
        startDate +
        "&endDate=" +
        endDate
    );
  }


  searchConvergence(season: string, stage: string) {
    return this.utility.http.get<string[]>(
      this.utility.baseUrl +
        "dks/searchConvergence?season=" +
        season +
        "&stage=" +
        stage
    );
  }
  uploadPicByArticle(articlePic: FormData) {
    console.log("dks.service upload:", articlePic);
    return this.utility.http.post(
      this.utility.baseUrl + "picture/uploadPicByArticle",
      articlePic
    );
  }
  deletePicByArticle(articlePic: FormData) {
    console.log("dks.service delete:", articlePic);
    return this.utility.http.post(
      this.utility.baseUrl + "picture/deletePicByArticle",
      articlePic
    );
  }
  checkF420Valid(excel: FormData) {
    console.log("dks.service checkF420Valid :", excel);
    return this.utility.http.post(
      this.utility.baseUrl + "dks/checkF420Valid",
      excel
    );
  }

  uploadF420Excel(f420Excel: FormData) {
    console.log("dks.service uploadF420Excel f420Excel:", f420Excel);
    return this.utility.http.post(
      this.utility.baseUrl + "f420/uploadF420Excel",
      f420Excel
    );
  }
}
