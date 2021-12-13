import { ModelInterface } from "./interface/model-interface";

export class PlmPart implements ModelInterface {
  partno: string;
  partnameen: string;
  partnamecn: string;
  partnamevn: string;
  location: string;

  rename: string;
  partgroup: string;
  insertuser: string;
  insertdate: string;
  changeuser: string;

  changedate: string;

  constructor(){

  }
}
