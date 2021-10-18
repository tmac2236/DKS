import { Pagination } from "./pagination";
import { utilityConfig } from "../utility/utility-config";

export class SDtrLoginHistory extends Pagination {
  systemName:string;
  account: string;
  loginTimeS : string;
  loginTimeE : string;
  /**
   *default set of searching parameters
   */
  constructor() {
    super();
    this.systemName = ""; 
    this.account = "";
    this.loginTimeS ="";
    this.loginTimeE ="";
    this.isPaging = true; //開分頁
  }

  public setPagination(pagination: Pagination) {
    this.currentPage = pagination.currentPage;
    this.itemsPerPage = pagination.itemsPerPage;
    this.totalItems = pagination.totalItems;
    this.totalPages = pagination.totalPages;
  }
}
