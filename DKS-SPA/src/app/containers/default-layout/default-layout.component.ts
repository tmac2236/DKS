import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { JwtHelperService } from '@auth0/angular-jwt';
import { User } from '../../core/_models/user';
import { AlertifyService } from "../../core/_services/alertify.service";
import { AuthService } from "../../core/_services/auth.service";
import { navItems } from "../../_nav";

@Component({
  selector: "app-dashboard",
  templateUrl: "./default-layout.component.html",
})
export class DefaultLayoutComponent implements OnInit {
  public sidebarMinimized = false;
  public navItems = navItems;
  user: string;
  jwtHelper = new JwtHelperService();

  constructor(
    public authService: AuthService,
    private alertify: AlertifyService,
    private router: Router
  ) {}
  ngOnInit() {
    const jwtTtoken  = localStorage.getItem('token');
    //console.log(this.jwtHelper.decodeToken(jwtTtoken));
    this.user = this.jwtHelper.decodeToken(jwtTtoken)['unique_name'];
  }
  toggleMinimize(e) {
    this.sidebarMinimized = e;
  }
  logout() {
    localStorage.removeItem("token");
    this.alertify.message("logged out");
    this.router.navigate([""]);
  }

  loggedIn() {
    return this.authService.loggedIn();
  }
}
