import { Pagination } from "./pagination";

export class SF203Copy extends Pagination {

  srfIdF: string;
  srfIdT: string;

  stageT: string;

  constructor() {
    super();
    this.isPaging = false; // 不開分頁
    this.srfIdF = "";
    this.srfIdT = "";
    
    this.stageT = "";
  }
  
  public setPagination(pagination: Pagination) {
    this.currentPage = pagination.currentPage;
    this.itemsPerPage = pagination.itemsPerPage;
    this.totalItems = pagination.totalItems;
    this.totalPages = pagination.totalPages;
  }
}
