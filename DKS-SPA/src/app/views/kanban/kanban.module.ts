// Angular
import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { KanbanComponent } from "./kanban.component";

// Theme Routing
import { KanbanRoutingModule } from "./kanban-routing.module";

import { NgxSpinnerModule } from "ngx-spinner";
import { HttpClientModule } from "@angular/common/http";
import { FormsModule } from "@angular/forms";

@NgModule({
  imports: [
    CommonModule,
    NgxSpinnerModule,
    HttpClientModule,
    FormsModule,
    KanbanRoutingModule,
  ],
  declarations: [KanbanComponent],
})
export class KanbanModule {}
