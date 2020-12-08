import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { AlertifyService } from "../../core/_services/alertify.service";
import { AuthService } from "../../core/_services/auth.service";
import { navItems } from "../../_nav";

@Component({
  selector: "app-dashboard",
  templateUrl: "./default-layout.component.html",
})
export class DefaultLayoutComponent {
  public sidebarMinimized = false;
  public navItems = navItems;

  constructor(
    public authService: AuthService,
    private alertify: AlertifyService,
    private router: Router
  ) {}

  toggleMinimize(e) {
    this.sidebarMinimized = e;
  }
  logout() {
    localStorage.removeItem("token");
    localStorage.removeItem("user");
    this.authService.decodedToken = null;
    this.authService.currentUser = null;
    this.alertify.message("logged out");
    this.router.navigate([""]);
  }
}
