import { ModelInterface } from "./interface/model-interface";

export class F340Schedule implements ModelInterface {
  buyPlanSeason: string;
  versionNo: string;
  devSeason: string;
  devTeam: string;
  article: string;

  cwaDeadline: string;
  modelNo: string;
  modelName: string;
  orderStag: string;
  sampleNo: string;

  devStatus: string;
  dropDate: string;
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
}
