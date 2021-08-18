import { ModelInterface } from "./interface/model-interface";

export class DevDtrVsList implements ModelInterface {
  season: string; 
  article: string;
  modelNo: string;
  modelName: string;
  developerId:string;
  devTeamId:string;

  upday: Date;
  upusr: string;

  constructor(){

  }
}
