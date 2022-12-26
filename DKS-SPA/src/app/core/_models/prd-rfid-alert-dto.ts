import { ModelInterface } from "./interface/model-interface";

export class PrdRfidAlertDto implements ModelInterface{
     gate :string;
     area: string;
     time :Date;
     epc  :string;
     seq  :string;
     reason  :string;
     updater  :string;
     updateTime  :string;
     checked:boolean;  //only 4 前端
}
