import { ModelInterface } from "./interface/model-interface";

export class DtrFgtEtd implements ModelInterface{
     factoryid :string;
     article :string;
     stage :string;
     test :string;
     qcReceive :Date;
     
     qcEtd :Date;
     remark:string;
     upusr:string;
     upday :Date;
}
