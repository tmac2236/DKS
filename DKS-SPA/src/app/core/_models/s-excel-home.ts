import { Pagination } from "./pagination";

export class SExcelHome extends Pagination {
  article: string;


  /**
   *default set of searching parameters
   */
  constructor() {
    super();
    this.article = "";
  }

}
