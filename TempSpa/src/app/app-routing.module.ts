import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { PlugdetailComponent } from './components/plugdetail/plugdetail.component';

const routes: Routes = [
  { path: 'home', component: DashboardComponent },
  { path: "plugs/:id", component: PlugdetailComponent},
  { path: '**', redirectTo: 'home' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
