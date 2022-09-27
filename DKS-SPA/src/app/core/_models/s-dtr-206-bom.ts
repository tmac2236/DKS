import { Pagination } from "./pagination";

export class SDtrF206Bom extends Pagination {

  article: string;

  constructor() {
    super();
    this.isPaging = false; // 不開分頁
  }
  
  public setPagination(pagination: Pagination) {
    this.currentPage = pagination.currentPage;
    this.itemsPerPage = pagination.itemsPerPage;
    this.totalItems = pagination.totalItems;
    this.totalPages = pagination.totalPages;
  }
}
