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
  sort:number;  //0817
  fileName: string;
  remark: string ;
  apply: string;
  ecrno: string;  //0817
  upDay: Date ;
  upUsr: string;
  pdmApply: string;  //0817
  pdmUpusr: string;  //0817
  pdmUpday: Date;  //0817 1216
  
  articleList: string;  //1216
  
  constructor(){
    
    this.ver = 0;

  }
}
