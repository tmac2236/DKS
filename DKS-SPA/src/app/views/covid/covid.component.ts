import { Component, OnInit } from "@angular/core";
import { from, Observable, Subject, throwError } from "rxjs";
import { interval, of } from "rxjs";
import { concat, concatAll, delay } from "rxjs/operators";
import { Utility } from "../../core/utility/utility";
import { CommonService } from "../../core/_services/common.service";
import * as Chart from 'chart.js'

@Component({
  selector: "app-covid",
  templateUrl: "./covid.component.html",
  styleUrls: ["./covid.component.scss"],
})
export class CovidComponent implements OnInit {

  constructor(public utility: Utility, private commonService:CommonService) {}

  title = 'angular8chartjs';
  canvas: any;
  ctx: any;
  seasonList:string[] = [];
  numList: number[] = [];
  bgcList: string[] = [];

  async ngAfterViewInit() {
    await this.getSeasonNumDto();
    this.canvas = document.getElementById('myChart');
    this.ctx = this.canvas.getContext('2d');
    let myChart = new Chart(this.ctx, {
      type: 'pie',
      data: {
          labels: this.seasonList,
          datasets: [{
              label: '# of Votes',
              data: this.numList,
              backgroundColor: this.bgcList,
              borderWidth: 1
          }]
      },
      options: {
        responsive: false
      }
    });
  }
  
  ngOnInit() {
    /* 
    //Test rxjs concatAll()
    let s1 = of( 1, 2, 3).pipe(delay(1000));
    let s2 = of( 4, 5, 6).pipe(delay(3000));
    let s3 = throwError('This is an error!');
    let s4 = of( 4, 5, 6).pipe(delay(2000));

    const obs:Array<Observable<number>> = new Array<Observable<number>>();
    obs.push(s1);
    obs.push(s2);
    obs.push(s3);
    obs.push(s4);

    const array = from(obs);
    const request = array.pipe(concatAll());
    const sub$ = request.subscribe(
        v =>console.log(v),  
        e => console.log(e),
        ()=> console.log("Api Compelete!")
    );
    */

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
  async getSeasonNumDto(){
    await this.commonService
    .getSeasonNumDto()
    .then(
      (res) => {
        res.map((x)=>{
          this.seasonList.push(x.k);
          this.numList.push( Number(x.v) );

          //generate random color start
          let randomR = Math.floor((Math.random() * 130) + 100);
          let randomG = Math.floor((Math.random() * 130) + 100);
          let randomB = Math.floor((Math.random() * 130) + 100);
          let bgc = "rgb(" + randomR + ", " + randomG + ", " + randomB + ")";
          this.bgcList.push(bgc);
          //generate random color end

        })
      },
      (error) => {
        this.utility.alertify.confirm(
          "System Notice",
          "Syetem is busy, please try later.",
          () => {}
        );
      }
    );
  }
}
