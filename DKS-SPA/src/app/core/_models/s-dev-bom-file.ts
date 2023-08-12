import { Pagination } from "./pagination";

export class SDevBomFile extends Pagination {

  factoryId: string;
  season: string;
  modelNo: string;
  modelName: string;
  article: string;
  team: string;

  constructor() {

    super();
    this.isPaging = false; //開分頁
    
    this.factoryId = "";
    this.season = "";
    this.modelNo = "";
    this.modelName = "";
    this.article = "";
    this.team = "";
  }
  public setPagination(pagination: Pagination) {

    this.currentPage = pagination.currentPage;
    this.itemsPerPage = pagination.itemsPerPage;
    this.totalItems = pagination.totalItems;
    this.totalPages = pagination.totalPages;
  }
}
