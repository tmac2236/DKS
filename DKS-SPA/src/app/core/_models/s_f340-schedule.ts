import { Pagination } from "./pagination";

export class SF340Schedule extends Pagination {
  season: string;
  bpVer: string;

  /**
   *default set of searching parameters
   */
  constructor() {
    super();
    this.season = "";
    this.bpVer = "";
  }

  public setPagination(pagination: Pagination) {
    this.currentPage = pagination.currentPage;
    this.itemsPerPage = pagination.itemsPerPage;
    this.totalItems = pagination.totalItems;
    this.totalPages = pagination.totalPages;
  }
}
