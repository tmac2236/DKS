import { ModelInterface } from "./interface/model-interface";

export class DtrF206Bom implements ModelInterface {
  modelNo: string;
  modelName: string;
  season: string;
  stage: string;
  article: string;

  partNo: string;
  partName: string;
  supplierMatId: string;
  //materiaNo: string;
  materialName: string;

  supplier: string;
  colorCode: string;
  colorName: string;
  uom: string;
  isSize: string;

  materialSize: string;
  consumption: number;

  constructor(){}
}
