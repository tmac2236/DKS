import { Injectable } from "@angular/core";
import { Utility } from "../utility/utility";


@Injectable({
  providedIn: "root",
})
export class CommonService {
  constructor(private utility: Utility) {}

  //get partname、partno、treatmentCode、treatmentEn、treatmentZh
  getPartName(article: string, stage: string) {
    return this.utility.http.get<object[]>(
      this.utility.baseUrl +
        "common/getPartName?article=" +
        article +
        "&stage=" +
        stage
    ).toPromise();
  }

  //get buyplan list by factory and season
  searchBPVerList(season: string, factory: string) {
    return this.utility.http.get<string[]>(
      this.utility.baseUrl +
        "common/getBPVersionBySeason?season=" +
        season +
        "&factory=" +
        factory
    );
  }
  //get article、modelNo、modelName
  getArticle(modelNo: string, article:string, modelName:string) {
    return this.utility.http.get<object[]>(
      this.utility.baseUrl + "common/getArticle?modelNo=" + modelNo +"&article=" + article + "&modelName=" + modelName
    ).toPromise();
  }
  //get article、modelNo、modelName、season、develperName、devTeamId
  getArticleSeason(season: string, article:string) {
    return this.utility.http.get<object[]>(
      this.utility.baseUrl + "common/getArticleSeason?season=" + season +"&article=" + article
    ).toPromise();
  }
}
