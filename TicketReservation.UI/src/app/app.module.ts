import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './features/home/home/home.component';
import { NavbarComponent } from './core/navbar/navbar/navbar.component';
import { ListAllEventsComponent } from './features/events/list-all-events/list-all-events.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { EventDetailsComponent } from './features/events/event-details/event-details.component';
import { AddReservationComponent } from './features/reservation/add-reservation/add-reservation.component';
import { LoginComponent } from './features/auth/login/login/login.component';
import { AuthInterceptor } from './core/interceptors/auth.interceptor';
import { AdminAllEventsComponent } from './features/admin/events/admin-all-events/admin-all-events.component';
import { AdminEditEventComponent } from './features/admin/events/admin-edit-event/admin-edit-event.component';
import { AdminAddEventComponent } from './features/admin/events/admin-add-event/admin-add-event.component';
import { ListCustomersComponent } from './features/admin/customers/list-customers/list-customers.component';
import { AdminAllLocationsComponent } from './features/admin/locations/admin-all-locations/admin-all-locations.component';
import { AdminAddLocationComponent } from './features/admin/locations/admin-add-location/admin-add-location.component';
import { AdminEditLocationComponent } from './features/admin/locations/admin-edit-location/admin-edit-location.component';
import { AdminAllReservationsComponent } from './features/admin/reservations/admin-all-reservations/admin-all-reservations.component';
import { ImageSelectorComponent } from './shared/components/image-selector/image-selector/image-selector.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    NavbarComponent,
    ListAllEventsComponent,
    EventDetailsComponent,
    AddReservationComponent,
    LoginComponent,
    AdminAllEventsComponent,
    AdminEditEventComponent,
    AdminAddEventComponent,
    ListCustomersComponent,
    AdminAllLocationsComponent,
    AdminAddLocationComponent,
    AdminEditLocationComponent,
    AdminAllReservationsComponent,
    ImageSelectorComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true,
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
