import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Utility } from "../utility/utility";
import { ArticlePic } from "../_models/article-pic";

@Injectable({
  providedIn: "root",
})
export class DksService {
  constructor(private utility: Utility) {}

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
}
