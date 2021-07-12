import { ModelInterface } from "./interface/model-interface";

export class DtrFgt implements ModelInterface {
  article: string;
  stage: string;
  kind: string;
  vern: number;
  labNo: string;
  pass: string;
  fail: string;
  upday: Date;
  fileName: string;
  upusr: string;

}
