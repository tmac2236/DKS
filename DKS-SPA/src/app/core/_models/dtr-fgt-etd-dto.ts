import { ModelInterface } from "./interface/model-interface";

export class DtrFgtEtdDto implements ModelInterface{
     factoryId :string;
     article :string;
     stage :string;
     test :string;
     qcReceive :Date;
     
     qcEtd :Date;
     remark :string;
     etdUser :string;
     etdDate :Date;
     labNo :string;

     fgtUser:string;
     fgtDate :Date;
}
