import { ModelInterface } from "./interface/model-interface";

export class DevTeamByLoginDto implements ModelInterface{
     workNo: string;
     name: string;
     login: string;
     devTeamNo: string;
     team: string;
}
