import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { forkJoin, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { EventModel } from '../models/event-model';
import { SeatRequest } from '../models/seat-request';
import { Seat } from '../models/seat';

@Injectable({
  providedIn: 'root',
})
export class EventsService {
  constructor(private http: HttpClient) {}

  getAllEvents(): Observable<EventModel[]> {
    return this.http.get<EventModel[]>(`${environment.apiBaseUrl}/api/Events`);
  }

  getEventById(id: string): Observable<EventModel> {
    return this.http.get<EventModel>(
      `${environment.apiBaseUrl}/api/Events/GetEventById?id=${id}`
    );
  }

  getSeatByPosition(seat: SeatRequest): Observable<any> {
    const { row, zone, number, eventId } = seat;
    const queryParams = `row=${row}&zone=${zone}&number=${number}&eventId=${eventId}`;
    const url = `${environment.apiBaseUrl}/api/Seats/GetSeatByPosition?${queryParams}`;

    return this.http.get<any>(url);
  }

  getSeatsByEventId(eventId: string): Observable<any[]> {
    const url = `${environment.apiBaseUrl}/api/Seats/GetSeatsByEventAndLocation?eventId=${eventId}`;
    return this.http.get<any[]>(url);
  }
}
