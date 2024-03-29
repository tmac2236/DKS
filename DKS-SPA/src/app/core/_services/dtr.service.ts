import { HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { map, timeout } from "rxjs/operators";
import { Utility } from "../utility/utility";
import { utilityConfig } from "../utility/utility-config";
import { AddDevDtrFgtResultDto } from "../_models/add-dev-dtr-fgt-result-dto";
import { DevDtrFgtResult } from "../_models/dev-dtr-fgt-result";
import { DevDtrFgtResultDto } from "../_models/dev-dtr-fgt-result-dto";
import { DevDtrVisStandard } from "../_models/dev-dtr-vis-standard";
import { DevDtrVsList } from "../_models/dev-dtr-vs-list";
import { DtrF206Bom } from "../_models/dtr-f206-bom";
import { DtrFgtEtdDto } from "../_models/dtr-fgt-etd-dto";
import { DtrFgtShoes } from "../_models/dtr-fgt-shoes";
import { DtrLoginHistory } from "../_models/dtr-login-history";
import { DtrLoginHistoryDto } from "../_models/dtr-login-history-dto";
import { PaginatedResult } from "../_models/pagination";
import { SDevDtrFgtResult } from "../_models/s-dev-dtr-fgt-result";
import { SDevDtrFgtResultReport } from "../_models/s-dev-dtr-fgt-result-report";
import { SDevDtrVisStandard } from "../_models/s-dev-dtr-vis-standard";
import { SDevDtrVsList } from "../_models/s-dev-dtr-vs-list";
import { SDtrF206Bom } from "../_models/s-dtr-206-bom";
import { SampleTrackReportDto } from "../_models/sample-track-report-dto";
import { SDtrLoginHistory } from "../_models/s_dtr-login-history";
import { SDtrFgtShoes } from "../_models/s_dtr_fgt_shoes";
import { SSampleTrackReport } from "../_models/s_sample-track-report";
import { TransitArticle } from "../_models/transit-article";

@Injectable({
  providedIn: "root",
})
export class DtrService {
  constructor(private utility: Utility) {}


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
    params = params.append("factoryId", sDevDtrFgtResult.factoryId.toString());

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

  addDevDtrFgtResult(devDtrFgtResult: DevDtrFgtResult) {
    return this.utility.http.post<boolean>(
      this.utility.baseUrl + 'dtr/addDevDtrFgtResult',
      devDtrFgtResult
    );
  }
  updateDevDtrFgtResult(devDtrFgtResult: DevDtrFgtResult) {
    return this.utility.http.post<boolean>(
      this.utility.baseUrl + 'dtr/updateDevDtrFgtResult',
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
    params = params.append("factoryId", sDevDtrVisStandard.factoryId.toString());

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
  getDevDtrList(
    sDevDtrVsList: SDevDtrVsList
  ): Observable<PaginatedResult<DevDtrVsList[]>> {
    const paginatedResult: PaginatedResult<DevDtrVsList[]> =
      new PaginatedResult<DevDtrVsList[]>();

    let params = new HttpParams();
    params = params.append("IsPaging", sDevDtrVsList.isPaging.toString());
    if (
      sDevDtrVsList.currentPage != null &&
      sDevDtrVsList.itemsPerPage != null
    ) {
      params = params.append(
        "pageNumber",
        sDevDtrVsList.currentPage.toString()
      );
      params = params.append(
        "pageSize",
        sDevDtrVsList.itemsPerPage.toString()
      );
      //params = params.append('orderBy', sAttendance.orderBy);
    }
    params = params.append("season", sDevDtrVsList.season.toString());
    params = params.append("article", sDevDtrVsList.article.toString());
    params = params.append("modelNo", sDevDtrVsList.modelNo.toString());
    params = params.append("modelName", sDevDtrVsList.modelName.toString());
    params = params.append("developerId", sDevDtrVsList.developerId.toString());
    params = params.append("devTeamId", sDevDtrVsList.devTeamId.toString());
    return this.utility.http
      .get<DevDtrVsList[]>(
        this.utility.baseUrl + "dtr/getDevDtrList",
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
  searchDtrLoginHistory(
    sCondition: SDtrLoginHistory
  ): Observable<PaginatedResult<DtrLoginHistory[]>> {
    const paginatedResult: PaginatedResult<DtrLoginHistory[]> =
      new PaginatedResult<DtrLoginHistory[]>();

    let params = new HttpParams();
    params = params.append("IsPaging", sCondition.isPaging.toString());
    if (
      sCondition.currentPage != null &&
      sCondition.itemsPerPage != null
    ) {
      params = params.append(
        "pageNumber",
        sCondition.currentPage.toString()
      );
      params = params.append("pageSize", sCondition.itemsPerPage.toString());
    }
    params = params.append("systemName", sCondition.systemName.toString());
    params = params.append("factoryId", sCondition.factoryId.toString());

    params = params.append("loginTimeS", sCondition.loginTimeS.toString());
    params = params.append("loginTimeE", sCondition.loginTimeE.toString());

    return this.utility.http
      .get<DtrLoginHistory[]>(this.utility.baseUrl + "dtr/getDtrLoginHistory", {
        observe: "response",
        params,
      })
      .pipe(
        timeout(utilityConfig.httpTimeOut),
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
  copyArraySave(vs: DevDtrVisStandard,fileName: string) {
    console.log("dks.service copyArraySave:", vs);
    return this.utility.http.post(
      this.utility.baseUrl + "dtr/copyeVSfile/"+fileName,
      vs
    );
  }

  deleteVSResult(devDtrVisStandard: DevDtrVisStandard) {
    return this.utility.http.post<boolean>(
      this.utility.baseUrl + 'dtr/deleteVSResult',
      devDtrVisStandard
    );
  }
  getAFgtByLabNo(labNo: string) {
    return this.utility.http.get<DevDtrFgtResult>(
      this.utility.baseUrl + 'dtr/getAFgtByLabNo?labNo='+ labNo
    );
  }
  transitArticle(transitArticle: TransitArticle) {
    console.log("dtr.service transitArticle:", transitArticle);
    return this.utility.http.post<string>(
      this.utility.baseUrl + "dtr/transitArticle",
      transitArticle
    );
  }
  dtrLoginHistory(model: DtrLoginHistoryDto) {
    console.log("dtr.service dtrLoginHistory:", model);
    return this.utility.http.post(this.utility.baseUrl + "auth/loginRecord", model);
  }
  //get F104 Basic code detail by typeNo
  checkEditFgtIsValid( factoryId:string, article:string , stage:string, kind:string ) {
    return this.utility.http.get<boolean>(
      this.utility.baseUrl + "dtr/checkEditFgtIsValid?factoryId="+ factoryId +"&article=" + article + "&stage=" + stage +"&kind=" + kind
    ).toPromise();
  }
  //檢查是否Dtr是否有重複:check (type:article、modelNo、modelName)+ stage + kind + factoryId can not be duplicated
  checkFgtIsValid(type:string, typeVal:string, stage:string, kind:string, factoryId:string){
    return this.utility.http.get<boolean>(
    this.utility.baseUrl + "dtr/checkFgtIsValid?type="+ type +"&typeVal=" + typeVal +"&stage=" + stage + "&kind=" + kind +"&factoryId=" + factoryId
    ).toPromise();
  }  

  qcSentMailDtrFgtResult( stage:string, modelNo:string , article:string, labNo:string, remark:string, type:string, reason:string ) {
    console.log("dtr.service qcSentMailDtrFgtResult:");
    return this.utility.http.get<boolean>(
      this.utility.baseUrl + "dtr/qcSentMailDtrFgtResult?stage="+ stage +"&modelNo=" + modelNo + "&article=" + article + "&labNo=" + labNo
      + "&remark="+ remark + "&type=" + type + "&reason=" + reason);
  }
  getSampleTrackDto(
    sSampleTrackReport: SSampleTrackReport
  ): Observable<PaginatedResult<SampleTrackReportDto[]>> {
    const paginatedResult: PaginatedResult<SampleTrackReportDto[]> =
      new PaginatedResult<SampleTrackReportDto[]>();

    let params = new HttpParams();
    params = params.append("IsPaging", sSampleTrackReport.isPaging.toString());
    if (
      sSampleTrackReport.currentPage != null &&
      sSampleTrackReport.itemsPerPage != null
    ) {
      params = params.append(
        "pageNumber",
        sSampleTrackReport.currentPage.toString()
      );
      params = params.append(
        "pageSize",
        sSampleTrackReport.itemsPerPage.toString()
      );

    }

    //params = params.append("article", sSampleTrackReport.article.toString());
    //params = params.append("modelNo", sSampleTrackReport.modelNo.toString());
    //params = params.append("modelName", sSampleTrackReport.modelName.toString());

    return this.utility.http
      .get<SampleTrackReportDto[]>(
        this.utility.baseUrl + "dtr/getSampleTrackDto",
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
  sentMailSampleTrack(){
    return this.utility.http.get<string[]>(
      this.utility.baseUrl + 'dtr/sentMailSampleTrack'
    );
  }
  getDtrFgtEtdDto(
    sDtrFgtShoes: SDtrFgtShoes
  ): Observable<PaginatedResult<DtrFgtEtdDto[]>> {
    const paginatedResult: PaginatedResult<DtrFgtEtdDto[]> =
      new PaginatedResult<DtrFgtEtdDto[]>();

    let params = new HttpParams();
    params = params.append("IsPaging", sDtrFgtShoes.isPaging.toString());
    if (
      sDtrFgtShoes.currentPage != null &&
      sDtrFgtShoes.itemsPerPage != null
    ) {
      params = params.append(
        "pageNumber",
        sDtrFgtShoes.currentPage.toString()
      );
      params = params.append(
        "pageSize",
        sDtrFgtShoes.itemsPerPage.toString()
      );

    }

    return this.utility.http
      .get<DtrFgtEtdDto[]>(
        this.utility.baseUrl + "dtr/getDtrFgtEtdDto",
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
  editDtrFgtEtds(dtrFgtEtdDtos: DtrFgtEtdDto[], userId:string) {
    console.log("dtr.service editDtrFgtEtds:", dtrFgtEtdDtos);
    return this.utility.http.post(
      this.utility.baseUrl + "dtr/editDtrFgtEtds/"+ userId,
      dtrFgtEtdDtos
    );
  }
  searchDtrF206Bom(
    sCondition: SDtrF206Bom
  ): Observable<PaginatedResult<DtrF206Bom[]>> {
    const paginatedResult: PaginatedResult<DtrF206Bom[]> =
      new PaginatedResult<DtrF206Bom[]>();

    let params = new HttpParams();
    params = params.append("IsPaging", sCondition.isPaging.toString());
    if (
      sCondition.currentPage != null &&
      sCondition.itemsPerPage != null
    ) {
      params = params.append(
        "pageNumber",
        sCondition.currentPage.toString()
      );
      params = params.append("pageSize", sCondition.itemsPerPage.toString());
    }
    params = params.append("article", sCondition.article.toString());


    return this.utility.http
      .get<DtrF206Bom[]>(this.utility.baseUrl + "dtr/getDtrF206Bom", {
        observe: "response",
        params,
      })
      .pipe(
        timeout(utilityConfig.httpTimeOut),
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
    
}
