import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { Utility } from "../../core/utility/utility";
import { SExcelHome } from "../../core/_models/s-excel-home";
@Component({
  selector: "app-dictionary",
  templateUrl: "./dictionary.component.html",
  styleUrls: ["./dictionary.component.scss"],
})
export class DictionaryComponent implements OnInit {

  sExcelHome: SExcelHome = new SExcelHome();

  constructor(
    private utility: Utility,
    private route: ActivatedRoute
  ) {
    this.route.queryParams.subscribe((params) => {
      //this.article = params.article;

    });
  }

  ngOnInit() {}
  export() {
    const url = this.utility.baseUrl + "excel/getP206DataByArticle";
    this.utility.exportFactory(url, "P206Data", this.sExcelHome);
  }
}
