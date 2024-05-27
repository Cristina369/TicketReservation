import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './features/home/home/home.component';
import { ListAllEventsComponent } from './features/events/list-all-events/list-all-events.component';
import { EventDetailsComponent } from './features/events/event-details/event-details.component';
import { AddReservationComponent } from './features/reservation/add-reservation/add-reservation.component';
import { LoginComponent } from './features/auth/login/login/login.component';
import { AdminAllEventsComponent } from './features/admin/events/admin-all-events/admin-all-events.component';
import { AdminEditEventComponent } from './features/admin/events/admin-edit-event/admin-edit-event.component';
import { AdminAddEventComponent } from './features/admin/events/admin-add-event/admin-add-event.component';
import { ListCustomersComponent } from './features/admin/customers/list-customers/list-customers.component';
import { AdminAllLocationsComponent } from './features/admin/locations/admin-all-locations/admin-all-locations.component';
import { AdminAddLocationComponent } from './features/admin/locations/admin-add-location/admin-add-location.component';
import { AdminEditLocationComponent } from './features/admin/locations/admin-edit-location/admin-edit-location.component';
import { AdminAllReservationsComponent } from './features/admin/reservations/admin-all-reservations/admin-all-reservations.component';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
  },
  {
    path: 'login',
    component: LoginComponent,
  },
  {
    path: 'events',
    component: ListAllEventsComponent,
  },
  {
    path: 'event-details/:id',
    component: EventDetailsComponent,
  },
  {
    path: 'buy-ticket',
    component: AddReservationComponent,
  },
  {
    path: 'admin/events',
    component: AdminAllEventsComponent,
  },
  {
    path: 'admin/events/add',
    component: AdminAddEventComponent,
  },
  {
    path: 'admin/events/:id',
    component: AdminEditEventComponent,
  },
  {
    path: 'admin/costumers',
    component: ListCustomersComponent,
  },
  {
    path: 'admin/locations',
    component: AdminAllLocationsComponent,
  },
  {
    path: 'admin/locations/add',
    component: AdminAddLocationComponent,
  },
  {
    path: 'admin/locations/:id',
    component: AdminEditLocationComponent,
  },
  {
    path: 'admin/reservations',
    component: AdminAllReservationsComponent,
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
