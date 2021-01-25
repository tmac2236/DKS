import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";

// Import Containers
import { DefaultLayoutComponent } from "./containers";
import { DictionaryComponent } from "./views/dictionary/dictionary.component";
import { F420Component } from "./views/dks/F420/F420.component";

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
    data: {
      title: "Picture",
    },
  },
  {
    path: "F420",
    component: F420Component,
    data: {
      title: "F420",
    },
  },
  {
    path: "dictionary",
    component: DictionaryComponent,
    data: {
      title: "dictionary",
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
