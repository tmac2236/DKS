import { ModelInterface } from "./interface/model-interface";

export class SampleTrackReportDto implements ModelInterface{
     sampleNo :string;
     article :string;
     modelNo :string;
     modelName :string;
     team :string;

     stage :string;
     test :string;
     barCodeNo :string;
     devTransDate :Date;
     devTransSn :string;

     devKeeper :string;
     devTransferer :string;
     overDays: number;
}
