import { HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { map, timeout } from "rxjs/operators";
import { Utility } from "../utility/utility";
import { utilityConfig } from "../utility/utility-config";
import { F340SchedulePpd } from "../_models/f340-schedule-ppd";
import { F340Schedule } from "../_models/f340-schedule.ts";
import { PaginatedResult } from "../_models/pagination";
import { SF340PpdSchedule } from "../_models/s_f340-ppd-schedule";
import { SF340Schedule } from "../_models/s_f340-schedule";
import { SDevBomFile } from "../_models/s-dev-bom-file";
import { DevBomFile } from "../_models/dev-bom-file";
import { DevBomFileDetailDto } from "../_models/dev-bom-file-detail-dto";
import { DevTeamByLoginDto } from "../_models/dev-team-by-login-dto";

@Injectable({
  providedIn: "root",
})
export class DksService {
  constructor(private utility: Utility) {}

  searchF340Process(
    sF340Schedule: SF340Schedule
  ): Observable<PaginatedResult<F340Schedule[]>> {
    const paginatedResult: PaginatedResult<F340Schedule[]> =
      new PaginatedResult<F340Schedule[]>();

    let params = new HttpParams();
    params = params.append("IsPaging", sF340Schedule.isPaging.toString());
    if (
      sF340Schedule.currentPage != null &&
      sF340Schedule.itemsPerPage != null
    ) {
      params = params.append(
        "pageNumber",
        sF340Schedule.currentPage.toString()
      );
      params = params.append("pageSize", sF340Schedule.itemsPerPage.toString());
      //params = params.append('orderBy', sAttendance.orderBy);
    }
    params = params.append("dataType", sF340Schedule.dataType.toString());
    params = params.append("factory", sF340Schedule.factory.toString());
    params = params.append("season", sF340Schedule.season.toString());
    params = params.append("bpVer", sF340Schedule.bpVer.toString());
    params = params.append("cwaDateS", sF340Schedule.cwaDateS.toString());
    params = params.append("cwaDateE", sF340Schedule.cwaDateE.toString());

    return this.utility.http
      .get<F340Schedule[]>(this.utility.baseUrl + "dks/getF340_Process", {
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

  searchF340PpdProcess(
    sF340PpdSchedule: SF340PpdSchedule
  ): Observable<PaginatedResult<F340SchedulePpd[]>> {
    const paginatedResult: PaginatedResult<F340SchedulePpd[]> =
      new PaginatedResult<F340SchedulePpd[]>();

    let params = new HttpParams();
    params = params.append("IsPaging", sF340PpdSchedule.isPaging.toString());
    if (
      sF340PpdSchedule.currentPage != null &&
      sF340PpdSchedule.itemsPerPage != null
    ) {
      params = params.append(
        "pageNumber",
        sF340PpdSchedule.currentPage.toString()
      );
      params = params.append(
        "pageSize",
        sF340PpdSchedule.itemsPerPage.toString()
      );
      //params = params.append('orderBy', sAttendance.orderBy);
    }
    params = params.append("factory", sF340PpdSchedule.factory.toString());
    params = params.append("season", sF340PpdSchedule.season.toString());
    params = params.append("bpVer", sF340PpdSchedule.bpVer.toString());
    params = params.append("cwaDateS", sF340PpdSchedule.cwaDateS.toString());
    params = params.append("cwaDateE", sF340PpdSchedule.cwaDateE.toString());
    params = params.append("article", sF340PpdSchedule.article.toString());
    params = params.append("modelNo", sF340PpdSchedule.modelNo.toString());
    params = params.append("modelName", sF340PpdSchedule.modelName.toString());
    params = params.append("ubType", sF340PpdSchedule.ubType.toString());

    return this.utility.http
      .get<F340SchedulePpd[]>(this.utility.baseUrl + "dks/getF340_ProcessPpd", {
        observe: "response",
        params,
      })
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

  editPicF340Ppd(f340PpdPic: FormData) {
    console.log("dks.service editPicF340Ppd:", f340PpdPic);
    return this.utility.http.post<F340SchedulePpd>(
      this.utility.baseUrl + "dks/editPicF340Ppd",
      f340PpdPic
    );
  }
  editPdfF340Ppd(f340PpdPic: FormData) {
    console.log("dks.service editPdfF340Ppd:", f340PpdPic);
    return this.utility.http.post<F340SchedulePpd>(
      this.utility.baseUrl + "dks/editPdfF340Ppd",
      f340PpdPic
    );
  }
  /* Pending
  editF340Ppds(f340Ppds: F340SchedulePpd[]) {
    console.log("dks.service editF340Ppds:", f340Ppds);
    return this.utility.http.post(
      this.utility.baseUrl + "dks/editF340Ppds",
      f340Ppds
    );
  }
  */
  //post two parameter to http post
  editF340Ppd(f340Ppd: F340SchedulePpd,type: string) {
    console.log("dks.service editF340Ppd:", f340Ppd);
    return this.utility.http.post(
      this.utility.baseUrl + "dks/editF340Ppd/"+type,
      f340Ppd
    );
  }
  sentMailF340PpdByArticle(sF340PpdSchedule: SF340PpdSchedule) {
    console.log("dks.service sentMailF340PpdByArticle:", sF340PpdSchedule);
    return this.utility.http.post(
      this.utility.baseUrl + "dks/sentMailF340PpdByArticle",
      sF340PpdSchedule
    );
  }
  
  saveUBDate(f340Ppd: F340SchedulePpd) {
    console.log("dks.service saveUBDate:", f340Ppd);
    return this.utility.http.post(
      this.utility.baseUrl + "dks/saveUBDate",
      f340Ppd
    );
  }
  getDevBomFile(
    sDevBomFile: SDevBomFile
  ): Observable<PaginatedResult<DevBomFileDetailDto[]>> {
    const paginatedResult: PaginatedResult<DevBomFileDetailDto[]> =
      new PaginatedResult<DevBomFileDetailDto[]>();

    let params = new HttpParams();
    params = params.append("IsPaging", sDevBomFile.isPaging.toString());
    if (
      sDevBomFile.currentPage != null &&
      sDevBomFile.itemsPerPage != null
    ) {
      params = params.append(
        "pageNumber",
        sDevBomFile.currentPage.toString()
      );
      params = params.append(
        "pageSize",
        sDevBomFile.itemsPerPage.toString()
      );

    }
    params = params.append("season", sDevBomFile.season.toString());
    params = params.append("modelNo", sDevBomFile.modelNo.toString());
    params = params.append("modelName", sDevBomFile.modelName.toString());
    params = params.append("article", sDevBomFile.article.toString());
    params = params.append("team", sDevBomFile.team.toString());
    params = params.append("factoryId", sDevBomFile.factoryId.toString());
    
    return this.utility.http
      .get<DevBomFileDetailDto[]>(
        this.utility.baseUrl + "bom/getDevBomFileDetailDto",
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

  addBOMfile(bomFile: FormData) {
    console.log("dks.service addBOMfile:", bomFile);
    return this.utility.http.post<boolean>(
      this.utility.baseUrl + "bom/addBOMfile",
      bomFile
    );
  }
  applyBOMfile(f: FormData) {
    console.log("dks.service applyBOMfile:", f);
    return this.utility.http.post<DevBomFile>(
      this.utility.baseUrl + "bom/applyBOMfile",
      f
    );
  }
  getDevTeamByLoginDto(login: string) {
    return this.utility.http.get<DevTeamByLoginDto[]>(
      this.utility.baseUrl + 'bom/getDevTeamByLoginDto?login='+ login
    );
  }
  checkHPSD138(article: string, ecrNo:string) {
    return this.utility.http.get<object[]>(
      this.utility.baseUrl + "bom/checkHPSD138?article=" + article +"&ecrNo=" + ecrNo
    ).toPromise();
  }  
}
