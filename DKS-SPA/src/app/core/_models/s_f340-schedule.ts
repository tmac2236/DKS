import { Pagination } from "./pagination";
import { utilityConfig } from "../..//core/utility/utility-config";

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
    this.factory = utilityConfig.factory;//預設翔鴻程
    this.season = "";
    this.bpVer = utilityConfig.conditionAll;
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
