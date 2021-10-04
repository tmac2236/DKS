import { ModelInterface } from "./interface/model-interface";

export class ArticleSeason implements ModelInterface {

  season: string; 
  article: string;
  modelNo: string;
  modelName: string;
  developerId:string;

  devTeamId:string;
  factoryId: string;
  pkArticle: string;
  stage: string;
  
  constructor(){

  }
}
