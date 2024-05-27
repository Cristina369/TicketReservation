import { Component } from '@angular/core';
import { Location } from '../models/location';
import { Observable, Subscription } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { LocationsService } from '../service/locations.service';
import { AddLocationRequest } from '../models/add-location';

@Component({
  selector: 'app-admin-edit-location',
  templateUrl: './admin-edit-location.component.html',
  styleUrls: ['./admin-edit-location.component.css'],
})
export class AdminEditLocationComponent {
  id: string | null = null;
  model?: Location;
  locations$?: Observable<Location[]>;
  selectedLocation?: string;

  routeSubscription?: Subscription;
  updateLocationSubscription?: Subscription;
  getLocationSubscription?: Subscription;

  constructor(
    private route: ActivatedRoute,
    private locationService: LocationsService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.routeSubscription = this.route.paramMap.subscribe({
      next: (params) => {
        this.id = params.get('id');

        if (this.id) {
          this.getLocationSubscription = this.locationService
            .getLocationById(this.id)
            .subscribe({
              next: (response) => {
                this.model = response;
              },
            });
        }
      },
    });
  }

  onFormSubmit(): void {
    if (this.model && this.id) {
      var updateEvent: AddLocationRequest = {
        name: this.model.name,
        description: this.model.description,
        type: this.model.type,
        address: this.model.address,
        capacity: this.model.capacity,
        locationPath: this.model.locationPath,
      };

      this.updateLocationSubscription = this.locationService
        .updateLocation(this.id, updateEvent)
        .subscribe({
          next: (response) => {
            this.router.navigateByUrl('admin/locations');
          },
        });
    }
  }

  ngOnDestroy(): void {
    this.routeSubscription?.unsubscribe();
    this.updateLocationSubscription?.unsubscribe();
    this.getLocationSubscription?.unsubscribe();
  }
}
