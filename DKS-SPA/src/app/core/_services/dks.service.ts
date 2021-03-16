import { HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { map } from 'rxjs/operators';
import { Utility } from "../utility/utility";
import { F340SchedulePpd } from "../_models/f340-schedule-ppd";
import { F340Schedule } from "../_models/f340-schedule.ts";
import { PaginatedResult } from "../_models/pagination";
import { SF340PpdSchedule } from "../_models/s_f340-ppd-schedule";
import { SF340Schedule } from "../_models/s_f340-schedule";

@Injectable({
  providedIn: "root",
})
export class DksService {
  constructor(private utility: Utility) {}

  searchF340Process(sF340Schedule: SF340Schedule): Observable<PaginatedResult<F340Schedule[]>> {
    
    const paginatedResult: PaginatedResult<F340Schedule[]> = new PaginatedResult<F340Schedule[]>();

    let params = new HttpParams();
    params = params.append('IsPaging', sF340Schedule.isPaging.toString());
    if (sF340Schedule.currentPage != null && sF340Schedule.itemsPerPage != null) {
      params = params.append('pageNumber', sF340Schedule.currentPage.toString());
      params = params.append('pageSize', sF340Schedule.itemsPerPage.toString());
      //params = params.append('orderBy', sAttendance.orderBy);
    }
    params = params.append('factory', sF340Schedule.factory.toString());
    params = params.append('season', sF340Schedule.season.toString());
    params = params.append('bpVer', sF340Schedule.bpVer.toString());
    params = params.append('cwaDateS', sF340Schedule.cwaDateS.toString());
    params = params.append('cwaDateE', sF340Schedule.cwaDateE.toString());

    return this.utility.http
      .get<F340Schedule[]>(this.utility.baseUrl + 'dks/getF340_Process' , {
        observe: 'response',
        params,
      })
      .pipe(
        map((response) => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(
              response.headers.get('Pagination')
            );
          }
          return paginatedResult;
        })
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
  searchBPVerList(season : string, factory: string){
    return this.utility.http.get<string[]>(
      this.utility.baseUrl +
        "dks/getBPVersionBySeason?season=" +
        season +'&factory=' + factory
    );
  }

  searchF340PpdProcess(sF340PpdSchedule: SF340PpdSchedule): Observable<PaginatedResult<F340SchedulePpd[]>> {
    
    const paginatedResult: PaginatedResult<F340SchedulePpd[]> = new PaginatedResult<F340SchedulePpd[]>();

    let params = new HttpParams();
    params = params.append('IsPaging', sF340PpdSchedule.isPaging.toString());
    if (sF340PpdSchedule.currentPage != null && sF340PpdSchedule.itemsPerPage != null) {
      params = params.append('pageNumber', sF340PpdSchedule.currentPage.toString());
      params = params.append('pageSize', sF340PpdSchedule.itemsPerPage.toString());
      //params = params.append('orderBy', sAttendance.orderBy);
    }
    params = params.append('factory', sF340PpdSchedule.factory.toString());
    params = params.append('season', sF340PpdSchedule.season.toString());
    params = params.append('bpVer', sF340PpdSchedule.bpVer.toString());
    params = params.append('cwaDateS', sF340PpdSchedule.cwaDateS.toString());
    params = params.append('cwaDateE', sF340PpdSchedule.cwaDateE.toString());

    return this.utility.http
      .get<F340SchedulePpd[]>(this.utility.baseUrl + 'dks/getF340_ProcessPpd' , {
        observe: 'response',
        params,
      })
      .pipe(
        map((response) => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(
              response.headers.get('Pagination')
            );
          }
          return paginatedResult;
        })
      );
  }
  
}
