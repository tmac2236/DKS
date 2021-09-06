import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";

// Import Containers
import { DefaultLayoutComponent } from "./containers";
import { utilityConfig } from "./core/utility/utility-config";
import { AuthGuard } from "./core/_guards/auth.guard";
import { AuthGuardRole } from "./core/_guards/auth.guard-role";
import { CovidComponent } from "./views/covid/covid.component";
import { DictionaryComponent } from "./views/dictionary/dictionary.component";
import { DtrFgtResultComponentComponent } from "./views/dks/dtr-fgt-result-component/dtr-fgt-result-component.component";
import { DtrFgtResultReportComponent } from "./views/dks/dtr-fgt-result-report/dtr-fgt-result-report.component";
import { DtrVStandardListComponent } from "./views/dks/dtr-v-standard-list/dtr-v-standard-list.component";
import { DtrVStandardComponent } from "./views/dks/dtr-v-standard/dtr-v-standard.component";
import { F340PpdComponent } from "./views/dks/F340/F340-ppd/F340-ppd.component";
import { F340Component } from "./views/dks/F340/F340.component";
import { F428EditComponent } from "./views/dks/F428/F428-edit/F428-edit.component";
import { F428Component } from "./views/dks/F428/F428.component";

import { P404Component } from "./views/error/404.component";
import { P500Component } from "./views/error/500.component";
import { HomePageComponent } from "./views/home-page/home-page.component";
import { PictureComponent } from "./views/picture/picture.component";

export const routes: Routes = [
  {
    path: "",
    //redirectTo: 'excel',
    //pathMatch: 'full',
    component: HomePageComponent,
  },
  {
    path: "404",
    component: P404Component,
    data: {
      title: "Page 404",
    },
  },
  {
    path: "500",
    component: P500Component,
    data: {
      title: "Page 500",
    },
  },
  {
    path: "Covid",
    component: CovidComponent,
  },  
  {
    path: "picture",
    component: PictureComponent,
  },
  {
    path: "F340",
    component: F340Component,
  },
  {
    path: "F340-PPD",
    canActivate: [AuthGuardRole],
    component: F340PpdComponent,
    data: {
      roles: [utilityConfig.RolePpdPic, utilityConfig.RolePpdLook],
    },
  },
  {
    path: "F428",
    canActivate: [AuthGuard],
    component: F428Component,
  },
  {
    path: "F428-edit",
    canActivate: [AuthGuard],
    component: F428EditComponent,
  },
  {
    path: "dictionary",
    component: DictionaryComponent,
  },
  {
    path: "DTR-FGT-Result",
    canActivate: [AuthGuardRole],
    component: DtrFgtResultComponentComponent,
    data: {
      roles: [utilityConfig.RoleFgtLabReport],
    },
  },
  {
    path: "DTR-FGT-Result-Report",
    canActivate: [AuthGuardRole],
    component: DtrFgtResultReportComponent,
    data: {
      roles: [utilityConfig.RoleFgtLabReport],
    },
  },
  {
    path: "DTR-Vs-Maintain",
    component: DtrVStandardComponent,
  },
  {
    path: "DTR-Vs-List",
    canActivate: [AuthGuardRole],
    component: DtrVStandardListComponent,
    data: {
      roles: [utilityConfig.RoleSysAdm],
    },
  },
  {
    path: "",
    component: DefaultLayoutComponent,
    data: {
      title: "Home",
    },
    children: [
      {
        path: "excel",
        loadChildren: () =>
          import("./views/excel/excel.module").then((m) => m.ExcelModule),
      },
      {
        path: "engineer",
        canActivate: [AuthGuardRole],
        loadChildren: () =>
          import("./views/engineer/engineer.module").then(
            (m) => m.EngineerModule
          ),
        data: {
          roles: [utilityConfig.RoleSysAdm],
        },
      },
    ],
  },
  { path: "**", component: P404Component },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {
  constructor() {
    //alert("CCC");
  }
}
