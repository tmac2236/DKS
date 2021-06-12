import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { EngF340Component } from "./eng-F340/eng-F340.component";
import { EngComponent } from "./eng/eng.component";

const routes: Routes = [
  {
    path: '',
    data: {
      title: 'Engineer',
    },
    children: [
      {
        path: '',
        redirectTo: 'engF340',
      },
      {
        path: 'eng',
        component: EngComponent,
        data: {
          title: 'Eng',
        },
      },      
      {
        path: 'engF340',
        component: EngF340Component,
        data: {
          title: 'Eng-F340',
        },
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class EngineerRoutingModule {}
