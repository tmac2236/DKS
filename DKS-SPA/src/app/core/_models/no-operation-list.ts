export interface NoOperationList {
  noOperations: NoOperation[];
}

export interface NoOperation {
  startDate: Date;
  lean: string;
  status: string;
  iOrder: number;
  onDuty: number;
}
