import { ModelInterface } from "./interface/model-interface";

export class DevDtrVisStandard implements ModelInterface {
  season: string; 
  article: string;
  id: string;
  remark: string;
  filename:string;

  factoryid:string;

  upday: Date;
  upusr: string;

  constructor(){
    this.remark = "";
  }
}
