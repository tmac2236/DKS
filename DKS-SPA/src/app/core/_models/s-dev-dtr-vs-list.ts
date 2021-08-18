import { Pagination } from "./pagination";

export class SDevDtrVsList extends Pagination {

  article: string;
  season: string;
  modelNo: string;
  modelName: string;
  developerId: string;
  devTeamId: string;

  constructor() {
    super();
    this.isPaging = true; // 不開分頁
    this.article ="";
    this.season ="";
    this.modelNo ="";
    this.modelName ="";
    this.developerId ="";
    this.devTeamId ="";
  }
  
  public setPagination(pagination: Pagination) {
    this.currentPage = pagination.currentPage;
    this.itemsPerPage = pagination.itemsPerPage;
    this.totalItems = pagination.totalItems;
    this.totalPages = pagination.totalPages;
  }
}
