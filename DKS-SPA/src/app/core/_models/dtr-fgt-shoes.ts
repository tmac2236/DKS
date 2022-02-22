import { ModelInterface } from "./interface/model-interface";

export class DtrFgtShoes implements ModelInterface{
     factoryid :string;
     sampleno :string;
     article :string;
     stage :string;
     test :string;

     qcReceive :Date;
     qcEtd :Date;
     upday :Date;
}
