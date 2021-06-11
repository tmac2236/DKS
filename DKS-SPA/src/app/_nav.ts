import { INavData } from '@coreui/angular';
import { Injectable } from '@angular/core';
import { AuthService } from './core/_services/auth.service';
import { utilityConfig } from './core/utility/utility-config';

export const navItems: INavData[] = [];

@Injectable({
  providedIn: 'root'  // <- ADD THIS
})
export class NavItem {

  navItems: INavData[] = [];
  theUserRoles = this.authService.getUserRole();
  constructor(private authService: AuthService){}

  getNav() {
    //grandFather
    this.navItems = [];

    //#region "navEngineer"
    //father
    const navEngineer = {
      name: '10. Engineer',
      url: '/engineer',
      icon: 'fa fa-cogs',
      children: []
    };
    //children
    const navEngineer_F340 = {
      name: '10.1 Eng-F340',
      url: '/engineer/engF340',
      class: 'menu-margin',
    };
    //children -> father
    if (this.theUserRoles.includes(utilityConfig.RoleSysAdm)) {
      navEngineer.children.push(navEngineer_F340);
    }
    //father -> grandfather
    if (navEngineer.children.length > 0) {
      this.navItems.push(navEngineer);
    }
    //#endregion  "navEngineer"

        //#region "navExcel"
    //father
    const navExcel = {
      name: '11. Excel',
      url: '/excel',
      icon: 'fa fa-cogs',
      children: []
    };
    //children
    const navExcel_Compare = {
      name: '11.1 Compare',
      url: '/excel/compare',
      class: 'menu-margin',
    };
    const navExcel_Macro = {
      name: '11.1 Macro',
      url: '/excel/macro',
      class: 'menu-margin',
    };
    //children -> father
    if (this.theUserRoles.includes(utilityConfig.RoleSysAdm)) {
      navExcel.children.push(navExcel_Compare);
    }
    if (this.theUserRoles.includes(utilityConfig.RoleSysAdm)) {
      navExcel.children.push(navExcel_Macro);
    }
    //father -> grandfather
    if (navExcel.children.length > 0) {
      this.navItems.push(navExcel);
    }
    //#endregion  "navExcel"

    return this.navItems;
  }
}
