import { Pagination } from "./pagination";

export class SF428Edit {
  status: string;
  chkStockNo: string;  //用/分格的字串
  sampleNo: string;
  materialNo: string;


  /**
   *default set of searching parameters
   */
  constructor() {
    this.status = "";
    this.materialNo = "";
  }

}
