export class Pagination {
  currentPage: number;
  itemsPerPage: number;
  totalItems: number;
  totalPages: number;
  orderBy: string;
  isPaging: boolean;
  loginUser: string;
  role:string;
  userId:string;
  /**
   * default setting of page
   */
  constructor() {
    (this.currentPage = 1),
      (this.itemsPerPage = 10),//設定頁數
      (this.totalItems = 0),
      (this.totalPages = 0);
    this.orderBy = null;
    this.isPaging = false;//開啟分頁與否
  }
}

export class PaginatedResult<T> {
  result: T;
  pagination: Pagination;
}
