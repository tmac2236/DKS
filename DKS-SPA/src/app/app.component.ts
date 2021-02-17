import { Component, OnInit } from "@angular/core";
import { Router, NavigationEnd } from "@angular/router";
import { LanguageService } from "./core/_services/language.service";

@Component({
  // tslint:disable-next-line
  selector: "body",
  template: "<router-outlet></router-outlet>",
})
export class AppComponent implements OnInit {
  constructor(
    private router: Router,
    private languageService: LanguageService
  ) {}

  ngOnInit() {
    this.languageService.setInitState();//初始化多國語
    this.router.events.subscribe((evt) => {
      if (!(evt instanceof NavigationEnd)) {
        return;
      }
      window.scrollTo(0, 0);
    });
  }
}
