import { Injectable } from '@angular/core';
import { Reservation } from '../models/reservation';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ReservationRequest } from '../models/add-reservation';

@Injectable({
  providedIn: 'root',
})
export class ReservationsService {
  constructor(private http: HttpClient) {}

  getReservationById(id: string): Observable<Reservation> {
    return this.http.get<Reservation>(
      `${environment.apiBaseUrl}/api/Reservation/${id}`
    );
  }

  getAllReservations(): Observable<Reservation[]> {
    return this.http.get<Reservation[]>(
      `${environment.apiBaseUrl}/api/Reservation`
    );
  }

  deleteReservation(id: string): Observable<Reservation> {
    return this.http.delete<Reservation>(
      `${environment.apiBaseUrl}/api/Reservation/${id}`
    );
  }

  createReservation(model: ReservationRequest): Observable<void> {
    return this.http.post<void>(
      `${environment.apiBaseUrl}/api/Reservation`,
      model
    );
  }

  updateReservation(
    id: string,
    updateReservation: ReservationRequest
  ): Observable<Reservation> {
    return this.http.put<Reservation>(
      `${environment.apiBaseUrl}/api/Reservation/${id}?addAuth=true`,
      updateReservation
    );
  }
}
