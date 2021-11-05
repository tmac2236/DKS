import { ModelInterface } from "./interface/model-interface";

export class DtrLoginHistory implements ModelInterface {
  systemName:string;
  factoryId:string;
  account: string;
  workNo: string;
  Name : string;
  iP : string;
  loginTime: Date;
}


