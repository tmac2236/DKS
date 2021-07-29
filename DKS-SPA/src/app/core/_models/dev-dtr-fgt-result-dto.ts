import { ModelInterface } from "./interface/model-interface";

export class DevDtrFgtResultDto implements ModelInterface {
  article: string;
  stage: string;
  kind: string;
  type: string;
  modelNo: Date;

  modelName: string;
  labNo: string;
  result: string;
  partNo: string;
  partName: string;

  fileName: string;
  remark: string;
  upday: Date;
  upusr: string;
  treatmentCode: string;

  treatmentZh: string;
  treatmentEn: string;

  vern: string;       //配合和新增畫面一樣物件
  devSeason: string;  //配合和新增畫面一樣物件
  cwaDate: string;    //配合和新增畫面一樣物件
  constructor(){}
}
