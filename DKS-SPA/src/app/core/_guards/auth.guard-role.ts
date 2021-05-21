import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router } from "@angular/router";
import { AuthService } from "../_services/auth.service";
import { AlertifyService } from "../_services/alertify.service";

@Injectable({
  providedIn: "root",
})

//
// e.g. GM0000000001	系統管理者                                                                                                                                                                                                                                                                                                                                                                                                                                                  
//

export class AuthGuardRole implements CanActivate {
  constructor(
    private authService: AuthService,
    private router: Router,
    private alertify: AlertifyService
  ) {}

  canActivate(route: ActivatedRouteSnapshot): boolean {
    let roles = route.data.roles as Array<string>;
    console.log("This page only accept role : " + roles);
    for(let role of roles){
      if (this.authService.checkIsThatGroup(role)) {
        return true;
      }
    }
    this.alertify.error("This account didn't have the auth !!!");
    this.router.navigate([""]); // redirect to login in
    return false;
  }
}
