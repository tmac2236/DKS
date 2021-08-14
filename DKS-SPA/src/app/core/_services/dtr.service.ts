import { HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { map, timeout } from "rxjs/operators";
import { Utility } from "../utility/utility";
import { utilityConfig } from "../utility/utility-config";
import { DevDtrFgtResult } from "../_models/dev-dtr-fgt-result";
import { DevDtrFgtResultDto } from "../_models/dev-dtr-fgt-result-dto";
import { DevDtrVisStandard } from "../_models/dev-dtr-vis-standard";
import { DevSysSet } from "../_models/dev-sys-set";
import { PaginatedResult } from "../_models/pagination";
import { SDevDtrFgtResult } from "../_models/s-dev-dtr-fgt-result";
import { SDevDtrFgtResultReport } from "../_models/s-dev-dtr-fgt-result-report";
import { SDevDtrVisStandard } from "../_models/s-dev-dtr-vis-standard";

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
  getDevDtrFgtResultReport(
    sDevDtrFgtResultReport: SDevDtrFgtResultReport
  ): Observable<PaginatedResult<DevDtrFgtResultDto[]>> {
    const paginatedResult: PaginatedResult<DevDtrFgtResultDto[]> =
      new PaginatedResult<DevDtrFgtResultDto[]>();

    let params = new HttpParams();
    params = params.append("IsPaging", sDevDtrFgtResultReport.isPaging.toString());
    if (
      sDevDtrFgtResultReport.currentPage != null &&
      sDevDtrFgtResultReport.itemsPerPage != null
    ) {
      params = params.append(
        "pageNumber",
        sDevDtrFgtResultReport.currentPage.toString()
      );
      params = params.append(
        "pageSize",
        sDevDtrFgtResultReport.itemsPerPage.toString()
      );
      //params = params.append('orderBy', sAttendance.orderBy);
    }
    params = params.append("devSeason", sDevDtrFgtResultReport.devSeason.toString());

    params = params.append("buyPlanSeason", sDevDtrFgtResultReport.buyPlanSeason.toString());
    params = params.append("factory", sDevDtrFgtResultReport.factory.toString());

    params = params.append("reportType", sDevDtrFgtResultReport.reportType.toString());
    params = params.append("article", sDevDtrFgtResultReport.article.toString());
    params = params.append("cwaDateS", sDevDtrFgtResultReport.cwaDateS.toString());
    params = params.append("cwaDateE", sDevDtrFgtResultReport.cwaDateE.toString());
    params = params.append("modelNo", sDevDtrFgtResultReport.modelNo.toString());
    params = params.append("modelName", sDevDtrFgtResultReport.modelName.toString());

    return this.utility.http
      .get<DevDtrFgtResultDto[]>(
        this.utility.baseUrl + "dtr/getDevDtrFgtResultReport",
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
  getArticle4Fgt(modelNo: string, article:string, modelName:string) {
    return this.utility.http.get<object[]>(
      this.utility.baseUrl + "dtr/getArticle4Fgt?modelNo=" + modelNo +"&article=" + article + "&modelName=" + modelName
    ).toPromise();
  }
  getArticleSeason(season: string, article:string) {
    return this.utility.http.get<object[]>(
      this.utility.baseUrl + "dtr/getArticleSeason?season=" + season +"&article=" + article
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
  getDevDtrVsReport(
    sDevDtrVisStandard: SDevDtrVisStandard
  ): Observable<PaginatedResult<DevDtrVisStandard[]>> {
    const paginatedResult: PaginatedResult<DevDtrVisStandard[]> =
      new PaginatedResult<DevDtrVisStandard[]>();

    let params = new HttpParams();
    params = params.append("IsPaging", sDevDtrVisStandard.isPaging.toString());
    if (
      sDevDtrVisStandard.currentPage != null &&
      sDevDtrVisStandard.itemsPerPage != null
    ) {
      params = params.append(
        "pageNumber",
        sDevDtrVisStandard.currentPage.toString()
      );
      params = params.append(
        "pageSize",
        sDevDtrVisStandard.itemsPerPage.toString()
      );
      //params = params.append('orderBy', sAttendance.orderBy);
    }
    params = params.append("season", sDevDtrVisStandard.season.toString());
    params = params.append("article", sDevDtrVisStandard.article.toString());

    return this.utility.http
      .get<DevDtrVisStandard[]>(
        this.utility.baseUrl + "dtr/getDevDtrVsReport",
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
  addVSfile(dtrVS: FormData) {
    console.log("dtr.service addVSfile:", dtrVS);
    return this.utility.http.post<boolean>(
      this.utility.baseUrl + "dtr/addVSfile",
      dtrVS
    );
  }
  deleteVSResult(devDtrVisStandard: DevDtrVisStandard) {
    return this.utility.http.post<boolean>(
      this.utility.baseUrl + 'dtr/deleteVSResult',
      devDtrVisStandard
    );
  }
}
