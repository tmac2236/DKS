import { Pagination } from "./pagination";

export class SF205Trans extends Pagination {

  article: string;
  stage: string;


  constructor() {
    super();
    this.isPaging = false; // 不開分頁
    this.article ="";
    this.stage ="";

  }
  
  public setPagination(pagination: Pagination) {
    this.currentPage = pagination.currentPage;
    this.itemsPerPage = pagination.itemsPerPage;
    this.totalItems = pagination.totalItems;
    this.totalPages = pagination.totalPages;
  }
}
