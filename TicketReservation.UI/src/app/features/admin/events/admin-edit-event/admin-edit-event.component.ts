import { Component } from '@angular/core';
import { EventModel } from '../models/event-model';
import { Observable, Subscription } from 'rxjs';
import { Location } from '../../locations/models/location';
import { ActivatedRoute, Router } from '@angular/router';
import { LocationsService } from '../../locations/service/locations.service';
import { AddEventRequest } from '../models/add-event';
import { AdminEventsService } from '../service/admin-events.service';
import { ImageService } from 'src/app/shared/components/services/image.service';

@Component({
  selector: 'app-admin-edit-event',
  templateUrl: './admin-edit-event.component.html',
  styleUrls: ['./admin-edit-event.component.css'],
})
export class AdminEditEventComponent {
  id: string | null = null;
  model?: EventModel;
  events$?: Observable<EventModel[]>;
  locations$?: Observable<Location[]>;
  selectedLocation?: string;
  isImageSelectorVisible: boolean = false;

  routeSubscription?: Subscription;
  updateEventSubscription?: Subscription;
  getEventSubscription?: Subscription;
  imageSelectSubscription?: Subscription;

  constructor(
    private route: ActivatedRoute,
    private eventService: AdminEventsService,
    private locationService: LocationsService,
    private imageService: ImageService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.locations$ = this.locationService.getAllLocations();

    this.routeSubscription = this.route.paramMap.subscribe({
      next: (params) => {
        this.id = params.get('id');

        if (this.id) {
          this.getEventSubscription = this.eventService
            .getEventById(this.id)
            .subscribe({
              next: (response) => {
                this.model = response;
                this.selectedLocation = response.locationId.toString();
              },
            });
        }

        this.imageSelectSubscription = this.imageService
          .onSelectImage()
          .subscribe({
            next: (response) => {
              if (this.model) {
                this.model.imageEvent = response.imageUrl;
                this.isImageSelectorVisible = false;
              }
            },
          });
      },
    });
  }

  onFormSubmit(): void {
    if (this.model && this.id && this.selectedLocation) {
      var updateEvent: AddEventRequest = {
        name: this.model.name,
        description: this.model.description,
        eventDate: this.model.eventDate,
        startTime: this.model.startTime,
        finishTime: this.model.finishTime,
        priceRangeEvent: this.model.priceRangeEvent,
        priceRangeForTickets: this.model.priceRangeForTickets,
        eventType: this.model.eventType,
        imageEvent: this.model.imageEvent,
        selectedLocation: this.selectedLocation,
      };

      this.updateEventSubscription = this.eventService
        .updateEvent(this.id, updateEvent)
        .subscribe({
          next: (response) => {
            this.router.navigateByUrl('admin/events');
          },
        });
    }
  }

  openImageSelector(): void {
    this.isImageSelectorVisible = true;
  }

  closeImageSelector(): void {
    this.isImageSelectorVisible = false;
  }

  ngOnDestroy(): void {
    this.routeSubscription?.unsubscribe();
    this.updateEventSubscription?.unsubscribe();
    this.getEventSubscription?.unsubscribe();
    this.imageSelectSubscription?.unsubscribe();
  }
}
