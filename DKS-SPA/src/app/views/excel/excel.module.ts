// Angular
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { CompareComponent } from './compare/compare.component';

// Theme Routing
import { ExcelRoutingModule } from './excel-routing.module';
import { MacroComponent } from './macro/macro.component';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule,FormsModule } from '@angular/forms';
import { ModalModule } from 'ngx-bootstrap/modal';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
@NgModule({
  imports: [
    CommonModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    ExcelRoutingModule,
    ModalModule,
    TooltipModule,
  ],
  declarations: [CompareComponent, MacroComponent],
})
export class ExcelModule {}
