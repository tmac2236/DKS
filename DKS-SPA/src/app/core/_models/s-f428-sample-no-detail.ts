import { Pagination } from "./pagination";

export class SF428SampleNoDetail extends Pagination {
  sampleNo: string;
  materialNo: string;

  //use for F42-edit 
  status: string;
  chkStockNo:string; //使用/當作陣列用
  /**
   *default set of searching parameters
   */
  constructor() {
    super();
    this.sampleNo = "";
    this.materialNo = "";
  }

  public setPagination(pagination: Pagination) {
    this.currentPage = pagination.currentPage;
    this.itemsPerPage = pagination.itemsPerPage;
    this.totalItems = pagination.totalItems;
    this.totalPages = pagination.totalPages;
  }
}
