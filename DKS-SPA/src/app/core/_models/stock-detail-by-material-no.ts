import { ModelInterface } from "./interface/model-interface";

export class StockDetailByMaterialNo implements ModelInterface{
    partNo :string;
    kind :string;
    materiaNo :string;
    orderStage :string;
    article :string;
    modelNo :string;

    stockNo :string;
    location :string;
    materialQty:string;
    testResult :string;
    inBoxDate :string;
}
