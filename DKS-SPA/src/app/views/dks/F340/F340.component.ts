import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { Utility } from "../../../core/utility/utility";
import { DksService } from "../../../core/_services/dks.service";

@Component({
  selector: "app-F340",
  templateUrl: "./F340.component.html",
  styleUrls: ["./F340.component.scss"],
})
export class F340Component implements OnInit {
  
  startDate: string;
  endDate: string;
  result: object[];
  constructor(
    private utility: Utility,
    private dksService: DksService,
  ) {}

  ngOnInit() {
    const newDate = new Date();
    this.startDate = this.utility.datepiper.transform(
      newDate.setDate(newDate.getDate()-7),//前七天
      'yyyy-MM-dd'
    );
    this.endDate = this.utility.datepiper.transform(
      new Date(),
      'yyyy-MM-dd'
    );

  }
  search() {
    this.utility.spinner.show();
    this.dksService.searchF340Process(this.startDate, this.endDate).subscribe(
      (res) => {
        this.result = res;
        this.utility.spinner.hide();
      },
      (error) => {
        this.utility.spinner.hide();
        this.utility.alertify.error(error);
      }
    );
  }
}
