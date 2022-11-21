import { ModelInterface } from "./interface/model-interface";

export class DevGateLogDataLog implements ModelInterface {
  seq: number;
  reason: string;
  updater: string;
  flag: string;


  constructor(){
    this.seq = 0;
    this.reason = "";
    this.updater = "";
    this.flag = "";
  }
}
