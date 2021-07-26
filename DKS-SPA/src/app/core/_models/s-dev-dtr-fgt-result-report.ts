import { utilityConfig } from "../utility/utility-config";
import { Pagination } from "./pagination";

export class SDevDtrFgtResultReport extends Pagination {

  devSeason: string;      //DEV use

  buyPlanSeason: string;  //DHO use
  factory:string;         //DHO use

  reportType: string;
  article: string;
  cwaDateS: string;
  cwaDateE: string;
  modelNo: string;
  modelName: string;
  

  constructor() {
    super();

    this.devSeason = "";

    this.buyPlanSeason = "";
    this.factory = utilityConfig.conditionAll; //預設翔鴻程

    this.reportType = "";
    this.article = "";
    this.cwaDateS = "";
    this.cwaDateE = "";
    this.modelNo = "";
    this.modelName = "";

    this.isPaging = true; //開分頁

  }
  
  public setPagination(pagination: Pagination) {

    this.currentPage = pagination.currentPage;
    this.itemsPerPage = pagination.itemsPerPage;
    this.totalItems = pagination.totalItems;
    this.totalPages = pagination.totalPages;
  }
}
