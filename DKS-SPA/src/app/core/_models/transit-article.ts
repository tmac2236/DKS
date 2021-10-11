import { ModelInterface } from "./interface/model-interface";

export class TransitArticle implements ModelInterface {

  article: string;
  modelNo: string;
  modelNoFrom: string;
  devTeamId:string;
  factoryId: string;
  factoryIdFrom: string;
  pkArticle: string;
  stage: string;
  
  updateUser:string;
  constructor(){

  }
}
