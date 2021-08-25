import { Component, OnInit } from "@angular/core";
import { interval } from "rxjs";

@Component({
  selector: "app-covid",
  templateUrl: "./covid.component.html",
  styleUrls: ["./covid.component.scss"],
})
export class CovidComponent implements OnInit {
  constructor() {}
  ngOnInit() {
    interval(1000).subscribe((x) => {
      document.getElementById("timer").innerHTML = this.getCountDay();
    });
  }

  getCountDay() {
    let ms = new Date().getTime() - new Date("2021-06-01T00:00:00").getTime();
    let days = Math.floor(ms / (24 * 60 * 60 * 1000));
    let daysms = ms % (24 * 60 * 60 * 1000);
    let hours = Math.floor(daysms / (60 * 60 * 1000));
    let hoursms = ms % (60 * 60 * 1000);
    let minutes = Math.floor(hoursms / (60 * 1000));
    let minutesms = ms % (60 * 1000);
    let sec = Math.floor(minutesms / 1000);
    return  days + " Day " + hours + " Hour " + minutes + " Min " + sec + " Sec ";
  }
}
