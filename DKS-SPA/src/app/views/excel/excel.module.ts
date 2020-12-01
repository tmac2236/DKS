// Angular
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { CompareComponent } from './compare/compare.component';


// Theme Routing
import { ExcelRoutingModule } from './excel-routing.module';

@NgModule({
  imports: [
    CommonModule,
    ExcelRoutingModule
  ],
  declarations: [
    CompareComponent,
  ]
})
export class ExcelModule { }
