import { Pagination } from "./pagination";
import { utilityConfig } from "../utility/utility-config";

export class SF406i extends Pagination {

  stockNo: string;
  /**
   *default set of searching parameters
   */
  constructor() {
    super();
    this.isPaging = false; //開分頁
    this.stockNo = "";
  }

  public setPagination(pagination: Pagination) {
    this.currentPage = pagination.currentPage;
    this.itemsPerPage = pagination.itemsPerPage;
    this.totalItems = pagination.totalItems;
    this.totalPages = pagination.totalPages;
  }
}
