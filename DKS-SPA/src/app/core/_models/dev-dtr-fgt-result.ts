import { ModelInterface } from "./interface/model-interface";

export class DevDtrFgtResult implements ModelInterface {
  article: string;
  stage: string;
  kind: string;
  type: string;
  modelNo: string;

  modelName: string;
  labNo: string;
  result: string;
  partNo: string;
  partName: string;

  fileName: string;
  remark: string;
  upday: Date;
  upusr: string;

  constructor(){}
}
