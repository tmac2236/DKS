import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { CompareComponent } from "./compare/compare.component";
import { MacroComponent } from "./macro/macro.component";

const routes: Routes = [
  {
    path: '',
    data: {
      title: 'Excel',
    },
    children: [
      {
        path: '',
        redirectTo: 'compare',
      },
      {
        path: 'compare',
        component: CompareComponent,
        data: {
          title: 'Compare',
        },
      },
      {
        path: 'macro',
        component: MacroComponent,
        data: {
          title: 'Macro',
        },
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ExcelRoutingModule {}
