import { Injectable } from "@angular/core";
import { CanActivate, Router } from "@angular/router";
import { AuthService } from "../_services/auth.service";
import { AlertifyService } from "../_services/alertify.service";

@Injectable({
  providedIn: "root",
})
export class AuthGuard implements CanActivate {
  constructor(
    private authService: AuthService,
    private router: Router,
    private alertify: AlertifyService
  ) {}

  canActivate(): boolean {
    if (this.authService.loggedIn()) {
      return true;
    }
    this.alertify.confirm(
      "Sweet Alert",
      "Please login in first ! System is going to redirect to DKS.",
      () => {
        //window.location.href ="http://10.4.0.39:8080/ArcareEng/login.jsp";
      }
    );
    return false;
  }
}
