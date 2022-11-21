import { HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { map, timeout } from "rxjs/operators";
import { Utility } from "../utility/utility";
import { utilityConfig } from "../utility/utility-config";
import { DevSysSet } from "../_models/dev-sys-set";
import { F340SchedulePpd } from "../_models/f340-schedule-ppd";
import { F340Schedule } from "../_models/f340-schedule.ts";
import { PaginatedResult } from "../_models/pagination";
import { PrdRfidAlertDto } from "../_models/prd-rfid-alert-dto";
import { SF340PpdSchedule } from "../_models/s_f340-ppd-schedule";
import { SF340Schedule } from "../_models/s_f340-schedule";
import { SRfidMaintain } from "../_models/s_rfid_maintain";

@Injectable({
  providedIn: "root",
})
export class SystemService {
  constructor(private utility: Utility) {}

  findAll() {
    return this.utility.http.get<DevSysSet[]>(
      this.utility.baseUrl +
        "System/findAll"
    );
  }
  eidtSysSet(model: DevSysSet){
    return this.utility.http.post<string>(
      this.utility.baseUrl +
        "System/eidtSysSet",
        model
    );
  }
  getRfidAlert(
    sRfidMaintain: SRfidMaintain
  ): Observable<PaginatedResult<PrdRfidAlertDto[]>> {
    const paginatedResult: PaginatedResult<PrdRfidAlertDto[]> =
      new PaginatedResult<PrdRfidAlertDto[]>();

    let params = new HttpParams();
    params = params.append("IsPaging", sRfidMaintain.isPaging.toString());
    if (
      sRfidMaintain.currentPage != null &&
      sRfidMaintain.itemsPerPage != null
    ) {
      params = params.append(
        "pageNumber",
        sRfidMaintain.currentPage.toString()
      );
      params = params.append(
        "pageSize",
        sRfidMaintain.itemsPerPage.toString()
      );
    }
    params = params.append("time", sRfidMaintain.reactime.toString());


    return this.utility.http
      .get<PrdRfidAlertDto[]>(
        this.utility.baseUrl + "system/getRfidAlert",
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
  setRfidAlerts(prdRfidAlertDtos: PrdRfidAlertDto[],reason: string,updater: string) {
    return this.utility.http.post(
      this.utility.baseUrl + "system/setRfidAlert/" + reason + "/" + updater,
      prdRfidAlertDtos
    );

  }


}
