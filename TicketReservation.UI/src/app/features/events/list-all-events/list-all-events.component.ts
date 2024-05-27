import { Component, OnDestroy, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { EventModel } from '../models/event-model';
import { EventsService } from '../service/events.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-list-all-events',
  templateUrl: './list-all-events.component.html',
  styleUrls: ['./list-all-events.component.css'],
})
export class ListAllEventsComponent implements OnInit {
  id: string | null = null;
  eventsM$?: Observable<EventModel[]>;
  defaultImage: string = '../../../../assets/defaultImage.jpg';

  constructor(private eventService: EventsService, private router: Router) {}

  ngOnInit(): void {
    this.eventsM$ = this.eventService.getAllEvents();

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
}
