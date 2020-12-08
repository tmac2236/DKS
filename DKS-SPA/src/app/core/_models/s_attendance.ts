import { Pagination } from './pagination';

export class SAttendance extends Pagination {
  workDate: string;
  pdc: string;
  building: string;
  lean: string;
  team: string;
  onDuty: string;
  offDuty: string;
  totalDuty: string;
  onDutyPercent: string;
  offDutyPercent: string;

  /**
   *default set of searching parameters
   */
  constructor() {
    super();
    this.workDate = null;
    this.pdc = null;
    this.building = null;
    this.lean = null;
    this.team = null;
    this.onDuty = null;
    this.offDuty = null;
    this.totalDuty = null;
    this.onDutyPercent = null;
    this.offDutyPercent = null;
  }

  public setPagination(pagination: Pagination) {
    this.currentPage = pagination.currentPage;
    this.itemsPerPage = pagination.itemsPerPage;
    this.totalItems = pagination.totalItems;
    this.totalPages = pagination.totalPages;
  }

}
