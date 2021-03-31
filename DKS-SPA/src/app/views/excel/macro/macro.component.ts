import { Component, OnInit, ViewChild } from "@angular/core";
import { ModalDirective } from "ngx-bootstrap/modal";

export class MacroModal {
  name: string;
  no: number;
  date: Date;
}

@Component({
  selector: "app-macro",
  templateUrl: "./macro.component.html",
  styleUrls: ["./macro.component.scss"],
})
export class MacroComponent implements OnInit {

  @ViewChild('infoModal') public infoModal: ModalDirective;
  
  macroModal: MacroModal = new MacroModal();
  result = ["Apple", "Orange", "Banana", "Coconut", "Grape"];
  resultFilter = [];
  constructor() {}

  ngOnInit() {
    //this.resultFilter = this.result.filter((x) => x === "Apple").sort();
    this.resultFilter = this.result;
  }
  ngBtn(item: string,kind: string){
    alert(item+kind);
    this.infoModal.show();
  }
  closeNcleanModal(){
    this.macroModal = new MacroModal();
    this.infoModal.hide();
  }
}
