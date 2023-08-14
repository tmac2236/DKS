import { ModelInterface } from "./interface/model-interface";

export class DevBomFile implements ModelInterface {
  factory: string;
  devTeamId: string;
  season: string;
  modelNo: string;
  modelName: string;
  article: string;
  stage: string;
  ver: number;
  fileName: string;
  remark: string ;
  apply: string;
  upDay: Date ;
  upUsr: string;
  
  constructor(){
    
    this.ver = 0;

  }
}
