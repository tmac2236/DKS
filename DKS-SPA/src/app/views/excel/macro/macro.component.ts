import { Component, OnInit } from "@angular/core";

@Component({
  selector: "app-macro",
  templateUrl: "./macro.component.html",
  styleUrls: ["./macro.component.scss"],
})
export class MacroComponent implements OnInit {
  result = ["Apple", "Orange", "Banana", "Coconut", "Grape", "Apple"];
  resultFilter = [];
  constructor() {}

  ngOnInit() {
    //this.resultFilter = this.result.filter((x) => x === "Apple").sort();
    this.resultFilter = this.result;
  }
}
