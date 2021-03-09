import { Pagination } from "./pagination";

export class SF340Schedule extends Pagination {
  factory: string;
  season: string;
  bpVer: string;
  cwaDateS : string;
  cwaDateE : string;
  /**
   *default set of searching parameters
   */
  constructor() {
    super();
    this.factory = "C";//預設翔鴻程
    this.season = "";
    this.bpVer = "All";
    this.cwaDateS ="";
    this.cwaDateE ="";
    this.isPaging = true; //開分頁
  }

  public setPagination(pagination: Pagination) {
    this.currentPage = pagination.currentPage;
    this.itemsPerPage = pagination.itemsPerPage;
    this.totalItems = pagination.totalItems;
    this.totalPages = pagination.totalPages;
  }
}
