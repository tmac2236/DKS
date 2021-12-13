import { Pagination } from "./pagination";

export class SPlmPart extends Pagination {

  partno: string;
  partnamecn: string;
  partnameen: string;
  location: string;

  constructor() {

    super();
    this.isPaging = true; //開分頁
    this.partno = "";
    this.partnamecn = "";
    this.partnameen = "";
    this.location = "";
  }
  public setPagination(pagination: Pagination) {

    this.currentPage = pagination.currentPage;
    this.itemsPerPage = pagination.itemsPerPage;
    this.totalItems = pagination.totalItems;
    this.totalPages = pagination.totalPages;
  }
}
