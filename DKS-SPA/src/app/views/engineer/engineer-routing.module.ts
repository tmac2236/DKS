import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { EngAuthorizeComponent } from "./eng-authorize/eng-authorize.component";
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
        path: 'eng-authorize',
        component: EngAuthorizeComponent,
        data: {
          title: 'Eng-Authorize',
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
