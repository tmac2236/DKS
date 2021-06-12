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
import { SF340PpdSchedule } from "../_models/s_f340-ppd-schedule";
import { SF340Schedule } from "../_models/s_f340-schedule";

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
}
