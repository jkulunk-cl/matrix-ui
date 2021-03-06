import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FontAwesomeModule} from '@fortawesome/angular-fontawesome';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavbarComponent } from './navbar/navbar.component';
import { SpeedbarComponent } from './speedbar/speedbar.component';
import { NavComponent } from './nav/nav.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { FooterComponent } from './footer/footer.component';
import { TabsComponent } from './tabs/tabs.component';
import { TableComponent } from './table/table.component';
import { NotificationsComponent } from './notifications/notifications.component';
import { FormsComponent } from './forms/forms.component';
import { UnsubscribeComponent } from './unsubscribe/unsubscribe.component';
import { UnsubscribeCompleteComponent } from './unsubscribe-complete/unsubscribe-complete.component';
import { TourComponent } from './tour/tour.component';
import { TypographyComponent } from './typography/typography.component';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    SpeedbarComponent,
    NavComponent,
    DashboardComponent,
    FooterComponent,
    TabsComponent,
    TableComponent,
    NotificationsComponent,
    FormsComponent,
    UnsubscribeComponent,
    UnsubscribeCompleteComponent,
    TourComponent,
    TypographyComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FontAwesomeModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
