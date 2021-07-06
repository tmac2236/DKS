// Angular
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

// Theme Routing
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule,FormsModule } from '@angular/forms';
import { ModalModule } from 'ngx-bootstrap/modal';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { EngineerRoutingModule } from './engineer-routing.module';
import { NgxSpinnerModule } from 'ngx-spinner';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { EngComponent } from './eng/eng.component';
import { EngAuthorizeComponent } from './eng-authorize/eng-authorize.component';
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
  declarations: [EngAuthorizeComponent,EngComponent],
})
export class EngineerModule {}
