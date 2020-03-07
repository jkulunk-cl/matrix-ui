import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';
import { TabsComponent } from './tabs/tabs.component';
import { UnsubscribeComponent} from './unsubscribe/unsubscribe.component';

const routes: Routes = [
  { path: 'dashboard', component: DashboardComponent},
  { path: 'component/tabs', component: TabsComponent},
  { path: 'unsubscribe', component: UnsubscribeComponent},
  { path: '', redirectTo: '/dashboard', pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
