import { ModelInterface } from "./interface/model-interface";

export class SrfArticleDto implements ModelInterface {

  srfId: string; 
  modelNo: string;
  stage: string;
  article: string;

  checked:boolean;  //only 4 前端
  
  constructor(){

  }
}
