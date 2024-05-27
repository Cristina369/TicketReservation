import { Component } from '@angular/core';
import { AddLocationRequest } from '../models/add-location';
import { Location } from '../models/location';
import { Observable } from 'rxjs';
import { LocationsService } from '../service/locations.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-admin-add-location',
  templateUrl: './admin-add-location.component.html',
  styleUrls: ['./admin-add-location.component.css'],
})
export class AdminAddLocationComponent {
  model: AddLocationRequest;
  locations$?: Observable<Location[]>;

  constructor(
    private locationService: LocationsService,
    private router: Router
  ) {
    this.model = {
      name: '',
      description: '',
      type: '',
      address: '',
      capacity: 0,
      locationPath: '',
    };
  }

  ngOnInit(): void {
    this.locations$ = this.locationService.getAllLocations();
    this.locations$.subscribe((locations) => {
      console.log('Here are the locations:', JSON.stringify(locations));
    });
  }

  onFormSubmit(): void {
    this.locationService.createLocation(this.model).subscribe({
      next: (response) => {
        this.router.navigateByUrl('/admin/locations');
      },
      error: (err) => {
        console.error('Error creating event:', err);
      },
    });
  }
}
