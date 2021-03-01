import { Pagination } from "./pagination";

export class SF340Schedule extends Pagination {
  season: string;
  bpVer: string;
  cwaDate : string;
  /**
   *default set of searching parameters
   */
  constructor() {
    super();
    this.season = "";
    this.bpVer = "";
    this.cwaDate ="";
    this.isPaging = true; //開分頁
  }

  public setPagination(pagination: Pagination) {
    this.currentPage = pagination.currentPage;
    this.itemsPerPage = pagination.itemsPerPage;
    this.totalItems = pagination.totalItems;
    this.totalPages = pagination.totalPages;
  }
}
