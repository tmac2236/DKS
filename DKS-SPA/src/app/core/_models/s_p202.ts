import { Pagination } from "./pagination";
import { utilityConfig } from "../utility/utility-config";

export class SP202 extends Pagination {

  season: string;
  brand:string;
  modelName: string;
  modelNo:string;
  article:string;
  /**
   *default set of searching parameters
   */
  constructor() {
    super();
    this.isPaging = false; //開分頁
    this.season = "";
    this.brand = "";
    this.modelName = "";
    this.modelNo = "";
    this.article = "";   
  }

  public setPagination(pagination: Pagination) {
    this.currentPage = pagination.currentPage;
    this.itemsPerPage = pagination.itemsPerPage;
    this.totalItems = pagination.totalItems;
    this.totalPages = pagination.totalPages;
  }
}
