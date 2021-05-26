import { ModelInterface } from "./interface/model-interface";

export class F340SchedulePpd implements ModelInterface {
  factory : string;
  buyPlanSeason : string;
  versionNo : string;
  devSeason : string;
  devTeam : string;

  article : string;
  cwaDeadline : string;
  modelNo : string;
  modelName : string;
  confirmDate : string;

  devStatus : string;
  dropDate : string;
  releaseType : string;
  sampleNo : string;
  hpPartNo : string;

  supsName : string;
  treatMent : string;
  partName : string;
  processDate : string;
  cardDate : string;

  proStatusId:string;
  ppdRemark:string;
}
