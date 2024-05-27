import { Component, OnDestroy, OnInit } from '@angular/core';
import { AddEventRequest } from '../models/add-event';
import { Observable, Subscription } from 'rxjs';
import { Router } from '@angular/router';
import { Location } from '../../locations/models/location';
import { LocationsService } from '../../locations/service/locations.service';
import { AdminEventsService } from '../service/admin-events.service';
import { ImageService } from 'src/app/shared/components/services/image.service';

@Component({
  selector: 'app-admin-add-event',
  templateUrl: './admin-add-event.component.html',
  styleUrls: ['./admin-add-event.component.css'],
})
export class AdminAddEventComponent implements OnInit, OnDestroy {
  model: AddEventRequest;
  locations$?: Observable<Location[]>;
  isImageSelectorVisible: boolean = false;

  imageSelectorSubscription?: Subscription;

  constructor(
    private eventService: AdminEventsService,
    private router: Router,
    private locationService: LocationsService,
    private imageService: ImageService
  ) {
    this.model = {
      name: '',
      description: '',
      eventDate: '',
      startTime: '',
      finishTime: '',
      priceRangeEvent: '',
      priceRangeForTickets: '',
      eventType: '',
      imageEvent: '',
      selectedLocation: '',
    };
  }

  ngOnInit(): void {
    this.locations$ = this.locationService.getAllLocations();
    this.locations$.subscribe((locations) => {
      console.log('Here are the locations:', JSON.stringify(locations));
    });

    this.imageSelectorSubscription = this.imageService
      .onSelectImage()
      .subscribe({
        next: (selectedImage) => {
          this.model.imageEvent = selectedImage.imageUrl;
          this.closeImageSelector();
        },
      });
  }

  onFormSubmit(): void {
    this.eventService.createEvent(this.model).subscribe({
      next: (response) => {
        this.router.navigateByUrl('/admin/events');
      },
      error: (err) => {
        console.error('Error creating event:', err);
      },
    });
  }

  openImageSelector(): void {
    this.isImageSelectorVisible = true;
  }

  closeImageSelector(): void {
    this.isImageSelectorVisible = false;
  }

  ngOnDestroy(): void {
    this.imageSelectorSubscription?.unsubscribe();
  }
}
