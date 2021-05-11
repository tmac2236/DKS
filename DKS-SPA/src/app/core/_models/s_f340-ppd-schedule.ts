import { Pagination } from "./pagination";
import { utilityConfig } from "../utility/utility-config";

export class SF340PpdSchedule extends Pagination {
  factory: string;
  season: string;
  bpVer: string;
  cwaDateS: string;
  cwaDateE: string;
  article: string;
  modelNo: string;
  modelName: string;
  /**
   *default set of searching parameters
   */
  constructor() {
    super();
    this.factory = utilityConfig.factory; //預設翔鴻程
    this.season = "";
    this.bpVer = "";
    this.cwaDateS = "";
    this.cwaDateE = "";
    this.isPaging = true; //開分頁
    this.article = "";
    this.modelNo = "";
    this.modelName = "";
  }

  public setPagination(pagination: Pagination) {
    this.currentPage = pagination.currentPage;
    this.itemsPerPage = pagination.itemsPerPage;
    this.totalItems = pagination.totalItems;
    this.totalPages = pagination.totalPages;
  }
}
