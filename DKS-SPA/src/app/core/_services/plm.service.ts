import { HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { map, timeout } from "rxjs/operators";
import { Utility } from "../utility/utility";
import { DevDtrFgtResultDto } from "../_models/dev-dtr-fgt-result-dto";
import { PaginatedResult } from "../_models/pagination";
import { PlmPart } from "../_models/plm-part";
import { SPlmPart } from "../_models/s_plm-part";


@Injectable({
  providedIn: "root",
})
export class PlmService {
  constructor(private utility: Utility) {}
  getPlmPart(
    sPlmPart: SPlmPart
  ): Observable<PaginatedResult<PlmPart[]>> {
    const paginatedResult: PaginatedResult<PlmPart[]> =
      new PaginatedResult<PlmPart[]>();

    let params = new HttpParams();
    params = params.append("IsPaging", sPlmPart.isPaging.toString());
    if (
      sPlmPart.currentPage != null &&
      sPlmPart.itemsPerPage != null
    ) {
      params = params.append(
        "pageNumber",
        sPlmPart.currentPage.toString()
      );
      params = params.append(
        "pageSize",
        sPlmPart.itemsPerPage.toString()
      );
    }
    params = params.append("partno", sPlmPart.partno.toString());
    params = params.append("partnamecn", sPlmPart.partnamecn.toString());
    params = params.append("partnameen", sPlmPart.partnameen.toString());
    params = params.append("location", sPlmPart.location.toString());

    return this.utility.http
      .get<PlmPart[]>(
        this.utility.baseUrl + "plm/getPlmPart",
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
  checkPartNoIsExist(partno:string){
    return this.utility.http.get<PlmPart>(
      this.utility.baseUrl + 'plm/checkPartNoIsExist?partno='+ partno
    ).toPromise();
  }
  addPlmPart(model: PlmPart) {
    return this.utility.http.post<boolean>(
      this.utility.baseUrl + 'plm/addPlmPart',
      model
    );
  }
  updatePlmPart(model: PlmPart) {
    return this.utility.http.post<boolean>(
      this.utility.baseUrl + 'plm/updatePlmPart',
      model
    );
  }  
  deletePlmPart(model: PlmPart) {
    return this.utility.http.post<boolean>(
      this.utility.baseUrl + 'plm/deletePlmPart',
      model
    );
  }
}
