import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { HomeComponent } from "./components/home/home.component";
import { SpecificAppComponent } from "./components/specific-app/specific-app.component";

const routes: Routes = [
  {
    path: "",
    component: HomeComponent
  },
  {
    path: "app/:name",
    component: SpecificAppComponent
  },
  {
    path: "**",
    pathMatch: "full",
    redirectTo: ""
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
