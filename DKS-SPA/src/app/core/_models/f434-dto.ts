import { ModelInterface } from "./interface/model-interface";

export class F434Dto implements ModelInterface {

  ssbMatPid:string;
  stockNo:string;
  wareCode:string;
  location:string;
  materialNo:string;

  materialName:string;
  color: string ;
  shoeSize:string;
  unit: string;
  materialQty: number;
  
  season:string;
  stage:string;
  orderStage: string; 
  devTeam: string;
  modelNameMemo: string;

  article:string;
  fmcaTestResult:string;
  singleTestResult:string;
  insertDate:Date;
  memo:string;

  orderNumber:string;
}
