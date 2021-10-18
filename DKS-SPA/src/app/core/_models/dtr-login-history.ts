import { ModelInterface } from "./interface/model-interface";

export class DtrLoginHistory implements ModelInterface {
  iD:number;
  systemName:string;
  account: string;
  pcName : string;
  iP : string;
  loginTime: Date;
}


