import { HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { map } from 'rxjs/operators';
import { Utility } from "../utility/utility";
import { F406iDto } from "../_models/f406i-dto";
import { F428SampleNoDetail } from "../_models/f428-sample-no-detail";
import { PaginatedResult } from "../_models/pagination";
import { SF428SampleNoDetail } from "../_models/s-f428-sample-no-detail";
import { StockDetailByMaterialNo } from "../_models/stock-detail-by-material-no";
import { SF406i } from "../_models/s_f406i";

@Injectable({
  providedIn: "root",
})
export class WarehouseService {
  constructor(private utility: Utility) {}

  getMaterialNoBySampleNoForWarehouse(sF428SampleNoDetail: SF428SampleNoDetail): Observable<PaginatedResult<F428SampleNoDetail[]>> {
    
    const paginatedResult: PaginatedResult<F428SampleNoDetail[]> = new PaginatedResult<F428SampleNoDetail[]>();

    let params = new HttpParams();
    params = params.append('IsPaging', sF428SampleNoDetail.isPaging.toString());
    if (sF428SampleNoDetail.currentPage != null && sF428SampleNoDetail.itemsPerPage != null) {
      params = params.append('pageNumber', sF428SampleNoDetail.currentPage.toString());
      params = params.append('pageSize', sF428SampleNoDetail.itemsPerPage.toString());
      //params = params.append('orderBy', sAttendance.orderBy);
    }
    params = params.append('sampleNo', sF428SampleNoDetail.sampleNo.toString());

    return this.utility.http
      .get<F428SampleNoDetail[]>(this.utility.baseUrl + 'wareHouse/getMaterialNoBySampleNoForWarehouse' , {
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


  getStockDetailByMaterialNo(sF428SampleNoDetail: SF428SampleNoDetail) {
    return this.utility.http.post<StockDetailByMaterialNo[]>(
      this.utility.baseUrl + 'wareHouse/getStockDetailByMaterialNo',
      sF428SampleNoDetail
    );
  }

  addStockDetailByMaterialNo(sF428SampleNoDetail: SF428SampleNoDetail) {
    return this.utility.http.post<StockDetailByMaterialNo[]>(
      this.utility.baseUrl + 'wareHouse/addStockDetailByMaterialNo',
      sF428SampleNoDetail
    );
  }

  searchF406i(
    sF406i: SF406i
  ): Observable<PaginatedResult<F406iDto[]>> {
    const paginatedResult: PaginatedResult<F406iDto[]> =
      new PaginatedResult<F406iDto[]>();

    let params = new HttpParams();
    params = params.append("IsPaging", sF406i.isPaging.toString());
    if (
      sF406i.currentPage != null &&
      sF406i.itemsPerPage != null
    ) {
      params = params.append(
        "pageNumber",
        sF406i.currentPage.toString()
      );
      params = params.append(
        "pageSize",
        sF406i.itemsPerPage.toString()
      );
      //params = params.append('orderBy', sAttendance.orderBy);
    }
    params = params.append("StockNo", sF406i.stockNo.toString());

    return this.utility.http
      .get<F406iDto[]>(
        this.utility.baseUrl + "wareHouse/getF406iDto",
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

}
