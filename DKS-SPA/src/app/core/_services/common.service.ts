import { Injectable } from "@angular/core";
import { Utility } from "../utility/utility";
import { ArticleSeason } from "../_models/article-season";
import { BasicCodeDto } from "../_models/basic-code-dto";
import { TupleDto } from "../_models/tuple-dto";


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
    ).toPromise();
  }
  //get article、modelNo、modelName
  getArticle(modelNo: string, article:string, modelName:string, factoryId:string) {
    return this.utility.http.get<object[]>(
      this.utility.baseUrl + "common/getArticle?modelNo=" + modelNo +"&article=" + article + "&modelName=" + modelName +"&factoryId=" + factoryId
    ).toPromise();
  }
  //get article、modelNo、modelName、season、develperName、devTeamId
  getArticleSeason(season: string, article:string, factoryId:string ) {
    return this.utility.http.get<ArticleSeason[]>(
      this.utility.baseUrl + "common/getArticleSeason?season=" + season +"&article=" + article +"&factoryId=" + factoryId
    ).toPromise();
  }
  //get F104 Basic code detail by typeNo
  getBasicCodeDto(typeNo: string) {
    return this.utility.http.get<BasicCodeDto[]>(
      this.utility.baseUrl + "common/getBasicCodeDto?typeNo=" + typeNo 
    ).toPromise();
  }
  //get F104 Basic code detail by typeNo
  getSeasonNumDto() {
    return this.utility.http.get<TupleDto[]>(
      this.utility.baseUrl + "test/getSeasonNum"
    ).toPromise();
  }
}
