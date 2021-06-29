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
  excelTypeList:string[] = ['P206_BOM_Export'];
  selectedExcel:string = "";
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
    
    var url = "";
    var fileName = "";
    switch (this.selectedExcel) {
      case "P206_BOM_Export":
          if(this.sExcelHome.article.trim() === ""){
            this.utility.alertify.confirm(
              "System Notice",
              "You have to key article first !",
              () => {}
            );
            return;
          }
          url = this.utility.baseUrl + "excel/getP206DataByArticle";
          fileName = "P206_BOM_Export";
          break;
          default: { 
            this.utility.alertify.confirm(
              "System Notice",
              "Please select one type of excel !",
              () => {}
            );
            return;
         } 
    }

    this.utility.exportFactory(url, fileName, this.sExcelHome);
  }
}
