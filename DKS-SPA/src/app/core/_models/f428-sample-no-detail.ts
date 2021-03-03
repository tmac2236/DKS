import { ModelInterface } from "./interface/model-interface";

export class F428SampleNoDetail implements ModelInterface{
    partNo :string;
    partName :string;
    materialName :string;
    materialNo :string; 
    ssbMatPid :string;

    supName :string; 
    colorName :string; 
    colorCode :string; 
    uom :string; 
    cons :string; 

    total :string; 
    status :string; 
    checkStockNo :string; 
    checkUser :string; 
    checkTime :string; 
}
