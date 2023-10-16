import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";

// Import Containers
import { DefaultLayoutComponent } from "./containers";
import { utilityConfig } from "./core/utility/utility-config";
import { AuthGuard } from "./core/_guards/auth.guard";
import { AuthGuardRole } from "./core/_guards/auth.guard-role";
import { CovidComponent } from "./views/covid/covid.component";
import { DictionaryComponent } from "./views/dictionary/dictionary.component";
import { DtrF206BomComponent } from "./views/dks/dtr-f206-bom/dtr-f206-bom.component";
import { DtrFgtResultComponentComponent } from "./views/dks/dtr-fgt-result-component/dtr-fgt-result-component.component";
import { DtrFgtResultReportComponent } from "./views/dks/dtr-fgt-result-report/dtr-fgt-result-report.component";
import { DtrFgtShoesComponent } from "./views/dks/dtr-fgt-shoes/dtr-fgt-shoes.component";
import { DtrLoginHistoryComponent } from "./views/dks/dtr-login-history/dtr-login-history.component";
import { DtrVStandardListComponent } from "./views/dks/dtr-v-standard-list/dtr-v-standard-list.component";
import { DtrVStandardComponent } from "./views/dks/dtr-v-standard/dtr-v-standard.component";
import { F205TransComponent } from "./views/dks/F205/F205-trans/F205-trans.component";
import { F340PpdComponent } from "./views/dks/F340/F340-ppd/F340-ppd.component";
import { F340Component } from "./views/dks/F340/F340.component";
import { F406IComponent } from "./views/dks/F406I/F406I.component";
import { F428EditComponent } from "./views/dks/F428/F428-edit/F428-edit.component";
import { F428Component } from "./views/dks/F428/F428.component";
import { P202Component } from "./views/dks/P202/P202.component";
import { PlmPartComponent } from "./views/dks/plm/plm-part/plm-part.component";
import { MaintainRfidComponent } from "./views/dks/rfid/maintain-rfid/maintain-rfid.component";
import { SampleTrackReportComponent } from "./views/dks/rfid/sample-track-report/sample-track-report.component";

import { P404Component } from "./views/error/404.component";
import { P500Component } from "./views/error/500.component";
import { HomePageComponent } from "./views/home-page/home-page.component";
import { PictureComponent } from "./views/picture/picture.component";
import { BomManageComponent } from "./views/dks/bom-manage/bom-manage.component";
import { F203CopyComponent } from "./views/dks/F203/F203-copy/F203-copy.component";

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
    path: "F205-Trans",
    component: F205TransComponent,
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
      roles: [utilityConfig.RoleFgtLabReport,utilityConfig.RoleSysAdm],
    },
  },
  {
    path: "DTR-FGT-Result-Report",
    canActivate: [AuthGuardRole],
    component: DtrFgtResultReportComponent,
    data: {
      roles: [utilityConfig.RoleFgtLabReport,utilityConfig.DtrQcSup],
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
    path: "BOM-Manage",
    component: BomManageComponent,
  },  
  {
    path: "DTR-Login-History",
    component: DtrLoginHistoryComponent,
  },
  {
    path: "DtrF206Bom",
    component: DtrF206BomComponent,
  },  
  {
    path: "PLM-Part",
    canActivate: [AuthGuardRole],
    component: PlmPartComponent, 
    data: {
      roles: [utilityConfig.PlmUpload],
    },
  },
  {
    path: "Sample-Track-Report",
    component: SampleTrackReportComponent,
  },
  {
    path: "DTR-FGT-Shoes",
    component: DtrFgtShoesComponent,     
  },
  {
    path: "P202",
    component: P202Component,     
  },
  {
    path: "F406I",
    component: F406IComponent, 
  }, 
  {
    path:"Maintain-RFID",
    component:MaintainRfidComponent,
  },
  {
    path: "F203-Copy",
    component: F203CopyComponent,
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
