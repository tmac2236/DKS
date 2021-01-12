import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { Utility } from "../../core/utility/utility";
import { DksService } from "../../core/_services/dks.service";

@Component({
  selector: "app-dictionary",
  templateUrl: "./dictionary.component.html",
  styleUrls: ["./dictionary.component.scss"],
})
export class DictionaryComponent implements OnInit {
  season: string;
  stage: string;
  result: string[];

  constructor(
    private utility: Utility,
    private dksService: DksService,
    private route: ActivatedRoute
  ) {
    this.route.queryParams.subscribe((params) => {
      this.season = params.season;
      this.stage = params.stage;
    });
  }

  ngOnInit() {}
  search() {
    this.utility.spinner.show();
    this.dksService.searchConvergence(this.season, this.stage).subscribe(
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
