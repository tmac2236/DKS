import { HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { map, timeout } from "rxjs/operators";
import { Utility } from "../utility/utility";
import { utilityConfig } from "../utility/utility-config";
import { DevDtrFgtResult } from "../_models/dev-dtr-fgt-result";
import { DevDtrFgtResultDto } from "../_models/dev-dtr-fgt-result-dto";
import { DevSysSet } from "../_models/dev-sys-set";
import { PaginatedResult } from "../_models/pagination";
import { SDevDtrFgtResult } from "../_models/s-dev-dtr-fgt-result";

@Injectable({
  providedIn: "root",
})
export class DtrService {
  constructor(private utility: Utility) {}

  getPartName4DtrFgt(article: string, stage: string) {
    return this.utility.http.get<object[]>(
      this.utility.baseUrl +
        "dtr/getPartName4DtrFgt?article=" +
        article +
        "&stage=" +
        stage
    ).toPromise();
  }

  editPdfDevDtrFgtResult(devDtrFgtResult: FormData) {
    console.log("dtr.service editPdfDevDtrFgtResult:", devDtrFgtResult);
    return this.utility.http.post<DevDtrFgtResult>(
      this.utility.baseUrl + "dtr/editPdfDevDtrFgtResult",
      devDtrFgtResult
    );
  }
  searchDevDtrFgtResult(
    sDevDtrFgtResult: SDevDtrFgtResult
  ): Observable<PaginatedResult<DevDtrFgtResultDto[]>> {
    const paginatedResult: PaginatedResult<DevDtrFgtResultDto[]> =
      new PaginatedResult<DevDtrFgtResultDto[]>();

    let params = new HttpParams();
    params = params.append("IsPaging", sDevDtrFgtResult.isPaging.toString());
    if (
      sDevDtrFgtResult.currentPage != null &&
      sDevDtrFgtResult.itemsPerPage != null
    ) {
      params = params.append(
        "pageNumber",
        sDevDtrFgtResult.currentPage.toString()
      );
      params = params.append(
        "pageSize",
        sDevDtrFgtResult.itemsPerPage.toString()
      );
      //params = params.append('orderBy', sAttendance.orderBy);
    }
    params = params.append("article", sDevDtrFgtResult.article.toString());
    params = params.append("modelNo", sDevDtrFgtResult.modelNo.toString());
    params = params.append("modelName", sDevDtrFgtResult.modelName.toString());

    return this.utility.http
      .get<DevDtrFgtResultDto[]>(
        this.utility.baseUrl + "dtr/getDevDtrFgtResultByModelArticle",
        {
          observe: "response",
          params,
        }
      )
      .pipe(
        map((response) => {
          paginatedResult.result = response.body;
          if (response.headers.get("Pagination") != null) {
            paginatedResult.pagination = JSON.parse(
              response.headers.get("Pagination")
            );
          }
          return paginatedResult;
        })
      );
  }
  getArticle4Fgt(modelNo: string, article:string) {
    return this.utility.http.get<object[]>(
      this.utility.baseUrl + "dtr/getArticle4Fgt?modelNo=" + modelNo +"&article=" + article
    ).toPromise();
  }
  addDevDtrFgtResult(devDtrFgtResult: DevDtrFgtResult) {
    return this.utility.http.post<boolean>(
      this.utility.baseUrl + 'dtr/addDevDtrFgtResult',
      devDtrFgtResult
    );
  }
  deleteDevDtrFgtResult(devDtrFgtResult: DevDtrFgtResult) {
    return this.utility.http.post<boolean>(
      this.utility.baseUrl + 'dtr/deleteDevDtrFgtResult',
      devDtrFgtResult
    );
  }
}
