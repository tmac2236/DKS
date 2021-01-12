import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { AuthGuard } from "../../core/_guards/auth.guard";
import { KanbanComponent } from "./kanban.component";

const routes: Routes = [
  {
    path: "",
    data: {
      title: "Kanban",
    },
    children: [
      {
        path: "",
        redirectTo: "kanban",
      },
      {
        path: "kanban",
        //canActivate: [AuthGuard],
        component: KanbanComponent,
        data: {
          title: "Kanban",
        },
      },

    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class KanbanRoutingModule {}
