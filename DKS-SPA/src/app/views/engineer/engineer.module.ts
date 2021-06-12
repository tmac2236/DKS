// Angular
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

// Theme Routing
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule,FormsModule } from '@angular/forms';
import { ModalModule } from 'ngx-bootstrap/modal';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { EngineerRoutingModule } from './engineer-routing.module';
import { EngF340Component } from './eng-F340/eng-F340.component';
import { NgxSpinnerModule } from 'ngx-spinner';
import { PaginationModule } from 'ngx-bootstrap/pagination';
@NgModule({
  imports: [
    CommonModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    ModalModule,
    NgxSpinnerModule,
    PaginationModule,    
    EngineerRoutingModule,
  ],
  declarations: [EngF340Component],
})
export class EngineerModule {}
