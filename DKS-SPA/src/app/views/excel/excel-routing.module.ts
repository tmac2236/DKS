import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CompareComponent } from './compare/compare.component';

const routes: Routes = [
  {
    path: '',
    data: {
      title: 'Excel'
    },
    children: [
      {
        path: '',
        redirectTo: 'compare'
      },
      {
        path: 'compare',
        component: CompareComponent,
        data: {
          title: 'compare'
        }
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ExcelRoutingModule {}
