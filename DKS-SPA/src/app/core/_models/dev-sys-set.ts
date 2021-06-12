import { ModelInterface } from "./interface/model-interface";

export class DevSysSet implements ModelInterface {
  syskey : string;
  sysval : string;
  datatype: string;
  upusr : string;
  uptime : Date;
}
