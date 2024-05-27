import { Component } from '@angular/core';
import { EventModel } from '../models/event-model';
import { Observable, Subscription } from 'rxjs';
import { AdminEventsService } from '../service/admin-events.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-admin-all-events',
  templateUrl: './admin-all-events.component.html',
  styleUrls: ['./admin-all-events.component.css'],
})
export class AdminAllEventsComponent {
  id: string | null = null;
  eventsM$?: Observable<EventModel[]>;

  deleteEventSubscription?: Subscription;

  constructor(
    private adminEventService: AdminEventsService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.eventsM$ = this.adminEventService.getAllEvents();

    this.eventsM$.subscribe((eventsM) => {
      eventsM.forEach((eventM) => {
        if (eventM.id) {
          console.log(`Event Id here: ${eventM.id}`);
        } else {
          console.log('Event is undefined.');
        }
      });
    });
  }

  onDelete(id: string): void {
    if (id) {
      this.deleteEventSubscription = this.adminEventService
        .deleteEvent(id)
        .subscribe({
          next: (response) => {
            this.router
              .navigateByUrl('/', { skipLocationChange: true })
              .then(() => {
                this.router.navigate(['/admin/events']);
              });
          },
        });
    }
  }

  ngOnDestroy(): void {
    this.deleteEventSubscription?.unsubscribe();
  }
}
