import { Pagination } from "./pagination";

export class SDevBomFile extends Pagination {

  factoryId: string;
  season: string;
  modelNo: string;
  modelName: string;
  article: string;
  team: string;

  userTeam:string;

  constructor() {

    super();
    this.isPaging = true; //開分頁
    
    this.factoryId = "";
    this.season = "";
    this.modelNo = "";
    this.modelName = "";
    this.article = "";
    this.team = "";
    this.userTeam = "N";
  }
  public setPagination(pagination: Pagination) {
    this.currentPage = pagination.currentPage;
    this.itemsPerPage = pagination.itemsPerPage;
    this.totalItems = pagination.totalItems;
    this.totalPages = pagination.totalPages;
  }
}
