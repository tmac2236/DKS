import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";

// Import Containers
import { DefaultLayoutComponent } from "./containers";
import { AuthGuard } from "./core/_guards/auth.guard";
import { DictionaryComponent } from "./views/dictionary/dictionary.component";
import { F340PpdComponent } from "./views/dks/F340/F340-ppd/F340-ppd.component";
import { F340Component } from "./views/dks/F340/F340.component";
import { F420Component } from "./views/dks/F420/F420.component";
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
    path: "picture",
    component: PictureComponent,
  },
  {
    path: "F340",
    component: F340Component,
  },
  {
    path: "F340-PPD",
    component: F340PpdComponent,
  },
  {
    path: "F420",
    component: F420Component,
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
        path: "kanban",
        loadChildren: () =>
          import("./views/kanban/kanban.module").then((m) => m.KanbanModule),
      },
    ],
  },
  { path: "**", component: P404Component },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
