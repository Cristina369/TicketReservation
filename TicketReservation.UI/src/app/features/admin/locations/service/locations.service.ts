import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Location } from '../models/location';
import { AddLocationRequest } from '../models/add-location';
import { PopulateLocationRequest } from '../models/populate-location';

@Injectable({
  providedIn: 'root',
})
export class LocationsService {
  constructor(private http: HttpClient) {}

  createLocation(model: AddLocationRequest): Observable<void> {
    return this.http.post<void>(
      `${environment.apiBaseUrl}/api/Locations`,
      model
    );
  }

  getAllLocations(): Observable<Location[]> {
    return this.http.get<Location[]>(`${environment.apiBaseUrl}/api/Locations`);
  }

  getLocationById(id: string): Observable<Location> {
    return this.http.get<Location>(
      `${environment.apiBaseUrl}/api/Locations/${id}`
    );
  }

  updateLocation(
    id: string,
    updateLocation: AddLocationRequest
  ): Observable<Location> {
    return this.http.put<Location>(
      `${environment.apiBaseUrl}/api/Locations/${id}?addAuth=true`,
      updateLocation
    );
  }

  deleteLocation(id: string): Observable<Location> {
    return this.http.delete<Location>(
      `${environment.apiBaseUrl}/api/Locations/${id}`
    );
  }

  populateLocation(model: PopulateLocationRequest): Observable<void> {
    return this.http.post<void>(
      `${environment.apiBaseUrl}/api/Seats/PopulateSeats`,
      model
    );
  }
}
