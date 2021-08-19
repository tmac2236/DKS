import { ModelInterface } from "./interface/model-interface";

export class BasicCodeDto implements ModelInterface {

  param : string;
  key : string;

  valueZh : string;
  valueEn : string;

  memoZh1 : string;
  memoZh2 : string;
  memoZh3 : string;
  memoZh4 : string;
  
  memoEn1: string;
  memoEn2: string;
  memoEn3: string;
  memoEn4: string;

  constructor(){}
}
