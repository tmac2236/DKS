export interface Attendance {
  workDate: Date;
  pdc: string;
  building: string;
  lean: string;
  team: string;

  onDuty: number;
  offDuty: number;
  totalDuty: number;
  onDutyPercent: number;
  offDutyPercent: number;
}