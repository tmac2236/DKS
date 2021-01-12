import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Utility } from "../utility/utility";
import { Attendance } from "../_models/attendance";
import { ChangeWorker } from "../_models/change-worker";
import { NoOperationList } from "../_models/no-operation-list";
import { PaginatedResult } from "../_models/pagination";
import { QueryPDModel } from "../_models/query-pd-model";
import { ReportDataPass } from "../_models/report-data-pass";
import { SQueryPDModel } from "../_models/s-query-pd-model";
import { SReportDataPass } from "../_models/s_report-data-pass";
import { map } from "rxjs/operators";
import { Observable } from "rxjs";
import { SAttendance } from "../_models/s_attendance";
import { SelectLean } from "../_models/select-lean";
import { SelectModelByLean } from "../_models/select-model-by-lean";

@Injectable({
  providedIn: "root",
})
export class DksService {
  constructor(private utility: Utility) {}

  searchConvergence (season: string, stage: string) {
    return this.utility.http.get<string[]>(
      this.utility.baseUrl + "dks/searchConvergence?season=" + season +"&stage=" +stage
    );
  }

}
