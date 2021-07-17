import { Pagination } from "./pagination";

export class SDevDtrFgtResult extends Pagination {

  article: string;
  modelNo: string;
  modelName: string;

  constructor() {

    super();
    this.isPaging = false; // 不開分頁
    this.article ="";
    this.modelNo ="";
    this.modelName ="";
  }
  
  public setPagination(pagination: Pagination) {

    this.currentPage = pagination.currentPage;
    this.itemsPerPage = pagination.itemsPerPage;
    this.totalItems = pagination.totalItems;
    this.totalPages = pagination.totalPages;
  }
}
