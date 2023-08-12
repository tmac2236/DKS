import { ModelInterface } from "./interface/model-interface";

export class DevBomFileDetailDto implements ModelInterface{
     factoryId: string;
     devTeamId: string;
     teamName: string;
     season: string;
     modelNo: string;

     modelName: string;
     article: string;
     stage: string;
     actDate: string;
     fileName: string;

     ver: number;
     remark: string;
     apply: string;
     id: number;

     remarkButton: string;
     downloadButton: string;
     applyButton: string;
     uploadButton: string;
}
