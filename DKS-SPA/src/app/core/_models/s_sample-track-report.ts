import { Pagination } from "./pagination";
import { utilityConfig } from "../utility/utility-config";

export class SSampleTrackReport extends Pagination {
  factory: string;
  article: string;
  modelNo: string;
  modelName: string;
  /**
   *default set of searching parameters
   */
  constructor() {
    super();
    this.isPaging = false; //開分頁
    this.factory = "";
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
