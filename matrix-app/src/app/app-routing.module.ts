import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';
import { TabsComponent } from './tabs/tabs.component';
import { UnsubscribeComponent } from './unsubscribe/unsubscribe.component';
import { UnsubscribeCompleteComponent } from './unsubscribe-complete/unsubscribe-complete.component';
import { TourComponent } from './tour/tour.component';
import { FormsComponent } from './forms/forms.component';
import { TypographyComponent } from './typography/typography.component';
import { TableComponent } from './table/table.component';

const routes: Routes = [
  { path: 'dashboard', component: DashboardComponent},
  { path: 'component/tabs', component: TabsComponent},
  { path: 'unsubscribe', component: UnsubscribeComponent},
  { path: 'unsubscribed', component: UnsubscribeCompleteComponent},
  { path: 'tour', component: TourComponent},
  { path: 'forms', component: FormsComponent},
  { path: 'typography', component: TypographyComponent},
  { path: 'table', component: TableComponent},
  { path: 'tabs', component: TabsComponent},
  { path: '', redirectTo: '/dashboard', pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
