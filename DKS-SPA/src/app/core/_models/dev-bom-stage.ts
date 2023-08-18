import { ModelInterface } from "./interface/model-interface";

export class DevBomStage implements ModelInterface {
  factory: string;
  stage: string;
  sort:number;  
  upDay: Date ;
  upUsr: string;

  constructor(){
    this.sort = 0;
  }
}
