export class Pagination {
  currentPage: number;
  itemsPerPage: number;
  totalItems: number;
  totalPages: number;
  orderBy: string;
  isPaging: boolean;
  /**
   * default setting of page
   */
  constructor() {
    (this.currentPage = 1),
      (this.itemsPerPage = 10),
      (this.totalItems = 0),
      (this.totalPages = 0);
    this.orderBy = null;
    this.isPaging = false;
  }
}

export class PaginatedResult<T> {
  result: T;
  pagination: Pagination;
}
