import { ModelInterface } from "./interface/model-interface";

export class F340Schedule implements ModelInterface {

  factory: string;
  buyPlanSeason: string;
  versionNo: string;
  devSeason: string;
  devTeam: string;

  article: string;
  activationDate: string;
  modelNo: string;
  modelName: string;
  orderStag: string;

  sampleNo: string;
  smsSampleNo: string;
  devStatus: string;
  dropDate: string;
  memo: string;

  hpFlag: string;
  hpSampleNo: string;
  f340SampleNo: string;
  releaseType: string;
  createDate: string;

  pdmDate: string;
  devUpDate: string;
  devBtmDate: string;
  ttUpDate: string;
  ttBtmDate: string;
  
  releaseDate: string;
  ttRejectReason: string;
  ttRejectDate: string;
  ttRejectCount: string;
  cwaDeadline: string;

}
