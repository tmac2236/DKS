import { Pagination } from "./pagination";
import { utilityConfig } from "../utility/utility-config";

export class SDtrFgtShoes extends Pagination {

  article: string;
  /**
   *default set of searching parameters
   */
  constructor() {
    super();
    this.isPaging = false; //開分頁
    this.article = "";
  }

  public setPagination(pagination: Pagination) {
    this.currentPage = pagination.currentPage;
    this.itemsPerPage = pagination.itemsPerPage;
    this.totalItems = pagination.totalItems;
    this.totalPages = pagination.totalPages;
  }
}
