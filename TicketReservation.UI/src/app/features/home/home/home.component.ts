import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { EventModel } from '../../events/models/event-model';
import { Observable } from 'rxjs';
import { EventsService } from '../../events/service/events.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {
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
